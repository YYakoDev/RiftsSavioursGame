#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
public static class UpgradeCreator
{
    private static string Location = "Assets/UpgradesStuff/Upgrades/RandomizedUpgrades/";
    private static CraftingMaterial AureaAnima;
    static StoreUpgradeData[] GeneratedStoreUpgrades;
    private static int Iterations;
    private static readonly int MaxIterations = 30000;
    private static Stopwatch sw = new();

    public static StoreUpgradeData[] StoreUpgrades
    {
        get
        {
            if(GeneratedStoreUpgrades == null)
            {
                CreateUpgrades();
            }
            return GeneratedStoreUpgrades;
        }
    }

    [MenuItem("Upgrades/CreateUpgrades")]
    public static void CreateUpgrades()
    {
        if (GeneratedStoreUpgrades != null) return;
        Iterations = 0;
        //bool folderExist = AssetDatabase.IsValidFolder(Location);
        //if(folderExist)
        //{
        //AssetDatabase.DeleteAsset("Assets/UpgradesStuff/Upgrades/RandomizedUpgrades");
        //}
        //AssetDatabase.CreateFolder("Assets/UpgradesStuff/Upgrades", "RandomizedUpgrades");
        var totalStats = Enum.GetValues(typeof(StatsTypes)) as StatsTypes[];
        StatsTypes[] statTypes = new StatsTypes[totalStats.Length - 2];
        int index = 0;
        for (int i = 0; i < totalStats.Length; i++)
        {
            var statType = totalStats[i];
            if(statType == StatsTypes.CurrentHealth || statType == StatsTypes.SlowdownMultiplier) continue;
            statTypes[index] = statType;
            index++;
        }
        var rarityLevels = Enum.GetValues(typeof(UpgradeRarity)) as UpgradeRarity[];
        GeneratedStoreUpgrades = new StoreUpgradeData[MaxIterations];
        foreach(var rarity in rarityLevels)
        {
            if(Iterations >= MaxIterations) break;
            foreach(var primaryStat in statTypes)
            {
                if(Iterations >= MaxIterations) break;
                int amountOfStats = (rarity) switch
                {
                    UpgradeRarity.Broken => 1,
                    UpgradeRarity.Common => Random.Range(1,3),
                    UpgradeRarity.Uncommon => (Random.Range(1,4)> 2) ? 2 : 1,
                    UpgradeRarity.Rare => (Random.Range(2,4)),
                    UpgradeRarity.Epic => (Random.Range(1,6) > 2) ? 3 : 2,
                    UpgradeRarity.Legendary => (Random.Range(1,6) > 2) ? Random.Range(3,5): Random.Range(2,5),
                    _ => 1
                };
                //StatsTypes[] types = new StatsTypes[amountOfStats];
                if(amountOfStats == 1)
                {
                    CreateUpgrade(rarity, primaryStat);
                    continue;
                }
                foreach(var secondaryStat in statTypes)
                {
                    if(secondaryStat == primaryStat) continue;
                    if(amountOfStats == 2)
                    {
                        CreateUpgrade(rarity, primaryStat, secondaryStat);
                        continue;
                    };
                    foreach(var terciaryStat in statTypes)
                    {
                        if(terciaryStat == secondaryStat || terciaryStat == primaryStat) continue;
                        if(amountOfStats == 3) 
                        {
                            
                            CreateUpgrade(rarity, primaryStat, secondaryStat, terciaryStat);
                            continue;
                        };
                        foreach(var quaternyStat in statTypes)
                        {
                            if(quaternyStat == terciaryStat || quaternyStat == secondaryStat || quaternyStat == primaryStat) continue;
                            CreateUpgrade(rarity, primaryStat, secondaryStat, terciaryStat, quaternyStat);
                            continue;
                        }
                    }
                }
                
            }
        }
        Debug.Log("<b>Created " + Iterations + "  upgrades</b>");
    }

    static void CreateUpgrade(UpgradeRarity rarity, params StatsTypes[] statsTypes)
    {
        if(Iterations >= MaxIterations) return;
        var primaryStat = statsTypes[0];
        StatsTypes? secondaryStat = null, terciaryStat = null, quaternaryStat = null;
        if(statsTypes.Length > 1)
        {
            secondaryStat = statsTypes[1];
            if(statsTypes.Length > 2)
            {
                terciaryStat = statsTypes[2];
                if(statsTypes.Length > 3)
                {
                    quaternaryStat = statsTypes[3];
                }
            }
        }
        var name = UpgradesNameRandomizer.GetName(rarity, primaryStat, secondaryStat, terciaryStat, quaternaryStat);
        StatModificationValue[] newValues = new StatModificationValue[statsTypes.Length];
        int negativeChances = rarity switch
        {
            UpgradeRarity.Broken => 0,
            UpgradeRarity.Common => Random.Range(40,52),
            UpgradeRarity.Uncommon => Random.Range(20,35),
            UpgradeRarity.Rare => Random.Range(15, 25),
            UpgradeRarity.Epic => Random.Range(20, 35),
            UpgradeRarity.Legendary => Random.Range(25, 41),
            _ => 20
        };
        int maxNegativeAmount = statsTypes.Length - 1;
        if(maxNegativeAmount == 0) negativeChances = 0;
        int negativeCount = 0;
        for (int i = 0; i < statsTypes.Length; i++)
        {
            var value = GetUpgradeEffectAmount(rarity);
            var isNegative = Random.Range(0, 100) > negativeChances;
            if(isNegative && negativeCount < maxNegativeAmount)
            {
                value = Mathf.RoundToInt((float)value * Random.Range(-0.4f, -0.175f));
                value = Mathf.Clamp(value, -75, 0);
                negativeCount++;
            }
            newValues[i] = new StatModificationValue(true, 1f, value);
        }
        Sprite icon = null;
        for (int i = 0; i < newValues.Length; i++)
        {
            if(newValues[i].Percentage < 0) continue;
            var newValue = ApplyNegativeBalancing(newValues[i].Percentage, newValues.Length, negativeCount);
            newValues[i].ChangePercentage(newValue);
            if(icon == null)
            {
                icon = StoreUpgradeIcons.GetUpgradeIcon(statsTypes[i]);
            }
        }

        var cost = GetUpgradeCost(rarity);
        StoreUpgradeData upgrade = new(name, icon, rarity, statsTypes, newValues, cost);
        GeneratedStoreUpgrades[Iterations] = upgrade;
        Iterations++;
        /*upgrade.name = name;
        upgrade.Initialize(name, icon, rarity, statsTypes, newValues, cost);
        AssetDatabase.CreateAsset(upgrade, Location+name+".asset");*/


    }

    static int ApplyNegativeBalancing(int value, int totalNumberOfStats, int negativeAmounts)
    {
        float negativeDiff = (float)(negativeAmounts / totalNumberOfStats);
        float negativeBalancing = negativeDiff switch 
        {
            >0f and <0.35f => 2f,
            >=0.35f and <0.55f => 3.55f,
            >=0.55f and < 1f => 5.55f,
            _ => 1f
        };
        return Mathf.RoundToInt(value * negativeBalancing);
    }

    static int GetUpgradeEffectAmount(UpgradeRarity rarity)
    {
        float rarityMultiplier = rarity switch
        {   
            UpgradeRarity.Broken => Random.Range(0.1f, 1f),
            UpgradeRarity.Common => Random.Range(0.75f,1.5f),
            UpgradeRarity.Uncommon => Random.Range(1.75f , 2.75f),
            UpgradeRarity.Rare => Random.Range(4f, 10f),
            UpgradeRarity.Epic => Random.Range(10f, 15f),
            UpgradeRarity.Legendary => Random.Range(15f, 30f),
            _ => 1f
        };

        int percentage = Mathf.RoundToInt(10 * rarityMultiplier);
        return percentage;
    }

    static UpgradeCost GetUpgradeCost(UpgradeRarity rarity)
    {
        if(AureaAnima == null)
            AureaAnima = AssetDatabase.LoadAssetAtPath<CraftingMaterial>("Assets/CraftingMaterials/AureaAnima.asset");
        var xpCost = rarity switch
        {
            UpgradeRarity.Broken => Random.Range(3, 7),
            UpgradeRarity.Common => Random.Range(7,12),
            UpgradeRarity.Uncommon => Random.Range(17,27),
            UpgradeRarity.Rare => Random.Range(45, 76),
            UpgradeRarity.Epic => Random.Range(125, 177),
            UpgradeRarity.Legendary => Random.Range(300, 550),
            _ => 0
        };
        var upgradeCost = new UpgradeCost(AureaAnima, xpCost);
        return upgradeCost;
    }

    public static int GetRandomUpgrade(params int[] skippeableIndex)
    {
        sw.Reset();
        sw.Start();
        int index = -1;
        int randomNumber = Random.Range(0, 100);
        int[] indexesToSkip = skippeableIndex;
        UpgradeRarity rarity = randomNumber switch
        {
            <1 => UpgradeRarity.Legendary,
            <10 => UpgradeRarity.Epic,
            <20 => UpgradeRarity.Rare,
            <35 => UpgradeRarity.Uncommon,
            <75 => UpgradeRarity.Common,
            <100 => UpgradeRarity.Broken,
            _ => UpgradeRarity.Common
        };
        var length = StoreUpgrades.Length;
        for (int i = 0; i < length; i++)
        {
            if(CheckIfIntMatchesIndexes(i)) continue;
            var upgrade = StoreUpgrades[i];
            if(upgrade.Rarity != rarity) continue;
            index = i;
            break;
        }
        sw.Stop();
        Debug.Log("Elapsed time from get random upgrade operation:   " + sw.ElapsedMilliseconds +" ms");

        bool CheckIfIntMatchesIndexes(int number)
        {
            bool match = false;
            for (int i = 0; i < skippeableIndex.Length; i++)
            {
                var index = skippeableIndex[i];
                if(index == number)
                {
                    match = true;
                    break;
                }
            }
            return match;
        }

        return index;
    }


}
#endif
public struct StoreUpgradeData
{
    string _name;
    Sprite _icon;
    UpgradeRarity _rarity;
    StatsTypes[] _stats;
    StatModificationValue[] _modifications;
    UpgradeCost[] _costs;

    public string Name => _name;
    public Sprite Icon => _icon;
    public UpgradeRarity Rarity => _rarity;
    public StatsTypes[] StatsTypes => _stats;
    public StatModificationValue[] Modifications => _modifications;
    public UpgradeCost[] Costs => _costs;

    public StoreUpgradeData(string name, Sprite icon, UpgradeRarity rarity, StatsTypes[] stats, StatModificationValue[] modifications, params UpgradeCost[] costs)
    {
        _name = name;
        _icon = icon;
        _rarity = rarity;
        _stats = stats;
        _modifications = modifications;
        _costs = costs;
    }
}