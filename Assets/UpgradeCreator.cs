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
    private static bool areUpgradesCreated = false;
    private static string Location = "Assets/UpgradesStuff/Upgrades/RandomizedUpgrades/";
    private static CraftingMaterial AureaAnima;
    static List<StoreUpgradeData?> brokenUpgrades, commonUpgrades, uncommonUpgrades, rareUpgrades, epicUpgrades, legendaryUpgrades;
    private static int Iterations;
    private static readonly int MaxIterations = 300000;
    private static Stopwatch sw = new();

    //[MenuItem("Upgrades/CreateUpgrades")]
    public static void CreateUpgrades()
    {
        if (areUpgradesCreated) return;
        sw.Reset();
        sw.Start();
        //if (GeneratedStoreUpgrades != null) return;
        Iterations = 0;
        var totalStats = Enum.GetValues(typeof(StatsTypes)) as StatsTypes[];
        StatsTypes[] statTypes = new StatsTypes[totalStats.Length - 2];
        int index = 0;
        for (int i = 0; i < totalStats.Length; i++)
        {
            var statType = totalStats[i];
            if (statType == StatsTypes.CurrentHealth 
            || statType == StatsTypes.SlowdownMultiplier 
            || statType == StatsTypes.HarvestMultiplier 
            || statType == StatsTypes.BuffBooster) continue;
            statTypes[index] = statType;
            index++;
        }
        var rarityLevels = Enum.GetValues(typeof(UpgradeRarity)) as UpgradeRarity[];
        brokenUpgrades = new(MaxIterations / 5);
        commonUpgrades = new(MaxIterations / 4);
        uncommonUpgrades = new(MaxIterations / 3);
        rareUpgrades = new(MaxIterations / 2);
        epicUpgrades = new(MaxIterations / 2);
        legendaryUpgrades = new(MaxIterations);

        foreach (var rarity in rarityLevels)
        {
            foreach (var primaryStat in statTypes)
            {
                int amountOfStats = (rarity) switch
                {
                    UpgradeRarity.Broken => 1,
                    UpgradeRarity.Common => Random.Range(1, 3),
                    UpgradeRarity.Uncommon => (Random.Range(1, 4) > 2) ? 2 : 1,
                    UpgradeRarity.Rare => (Random.Range(2, 4)),
                    UpgradeRarity.Epic => (Random.Range(1, 6) > 2) ? 3 : 2,
                    UpgradeRarity.Legendary => (Random.Range(1, 6) > 2) ? Random.Range(3, 5) : Random.Range(2, 5),
                    _ => 1
                };
                //StatsTypes[] types = new StatsTypes[amountOfStats];
                if (amountOfStats == 1)
                {
                    CreateUpgrade(rarity, primaryStat);
                    continue;
                }
                foreach (var secondaryStat in statTypes)
                {
                    if (secondaryStat == primaryStat) continue;
                    if (amountOfStats == 2)
                    {
                        CreateUpgrade(rarity, primaryStat, secondaryStat);
                        continue;
                    };
                    foreach (var terciaryStat in statTypes)
                    {
                        if (terciaryStat == secondaryStat || terciaryStat == primaryStat) continue;
                        if (amountOfStats == 3)
                        {

                            CreateUpgrade(rarity, primaryStat, secondaryStat, terciaryStat);
                            continue;
                        };
                        foreach (var quaternyStat in statTypes)
                        {
                            if (quaternyStat == terciaryStat || quaternyStat == secondaryStat || quaternyStat == primaryStat) continue;
                            CreateUpgrade(rarity, primaryStat, secondaryStat, terciaryStat, quaternyStat);
                            continue;
                        }
                    }
                }

            }
        }
        Debug.Log("<b>Created " + Iterations + "  upgrades</b>");
        areUpgradesCreated = true;
        foreach (UpgradeRarity upgradeRarity in rarityLevels)
        {
            var list = GetList(upgradeRarity);
            list.RemoveAll(item => item == null);
        }
        sw.Stop();
        Debug.Log("Elapsed time from get random upgrade operation:   " + sw.ElapsedMilliseconds + " ms");
    }

    static void CreateUpgrade(UpgradeRarity rarity, params StatsTypes[] statsTypes)
    {
        if (Iterations >= MaxIterations) return;
        var primaryStat = statsTypes[0];
        StatsTypes? secondaryStat = null, terciaryStat = null, quaternaryStat = null;
        if (statsTypes.Length > 1)
        {
            secondaryStat = statsTypes[1];
            if (statsTypes.Length > 2)
            {
                terciaryStat = statsTypes[2];
                if (statsTypes.Length > 3)
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
            UpgradeRarity.Common => Random.Range(40, 52),
            UpgradeRarity.Uncommon => Random.Range(25, 40),
            UpgradeRarity.Rare => Random.Range(15, 30),
            UpgradeRarity.Epic => Random.Range(25, 40),
            UpgradeRarity.Legendary => Random.Range(20, 50),
            _ => 20
        };
        int maxNegativeAmount = statsTypes.Length - 1;
        if (maxNegativeAmount == 0) negativeChances = 0;
        int negativeCount = 0;
        for (int i = 0; i < statsTypes.Length; i++)
        {
            var value = GetUpgradeEffectAmount(rarity);
            var isNegative = Random.Range(0, 100) > negativeChances;
            if (isNegative && negativeCount < maxNegativeAmount)
            {
                value = Mathf.RoundToInt((float)value * Random.Range(-0.35f, -0.175f));
                value = Mathf.Clamp(value, -75, 0);
                negativeCount++;
            }
            newValues[i] = new StatModificationValue(true, 1f, value);
        }
        Sprite icon = null;
        for (int i = 0; i < newValues.Length; i++)
        {
            if (newValues[i].Percentage < 0) continue;
            var newValue = ApplyNegativeBalancing(newValues[i].Percentage, newValues.Length, negativeCount);
            newValues[i].ChangePercentage(newValue);
            if (icon == null)
            {
                icon = StoreUpgradeIcons.GetUpgradeIcon(statsTypes[i]);
            }
        }

        var cost = GetUpgradeCost(rarity);
        StoreUpgradeData upgrade = new(name, icon, rarity, statsTypes, newValues, cost);

        List<StoreUpgradeData?> list = GetList(rarity);
        if (Iterations >= list.Count)
        {
            list.Add(upgrade);
            Iterations++;
            return;
        }
        list[Iterations] = upgrade;
        Iterations++;
        /*upgrade.name = name;
        upgrade.Initialize(name, icon, rarity, statsTypes, newValues, cost);
        AssetDatabase.CreateAsset(upgrade, Location+name+".asset");*/


    }

    static List<StoreUpgradeData?> GetList(UpgradeRarity rarity)
    {
        if (brokenUpgrades == null) CreateUpgrades();
        return (rarity) switch
        {
            UpgradeRarity.Broken => brokenUpgrades,
            UpgradeRarity.Common => commonUpgrades,
            UpgradeRarity.Uncommon => uncommonUpgrades,
            UpgradeRarity.Rare => rareUpgrades,
            UpgradeRarity.Epic => epicUpgrades,
            UpgradeRarity.Legendary => legendaryUpgrades,
            _ => commonUpgrades
        };
    }

    static int ApplyNegativeBalancing(int value, int totalNumberOfStats, int negativeAmounts)
    {
        float negativeDiff = (float)(negativeAmounts / totalNumberOfStats);
        float negativeBalancing = negativeDiff switch
        {
            > 0f and < 0.35f => 1.25f,
            >= 0.35f and < 0.55f => 2.55f,
            >= 0.55f and < 1f => 4.55f,
            _ => 1f
        };
        return Mathf.RoundToInt(value * negativeBalancing);
    }

    static int GetUpgradeEffectAmount(UpgradeRarity rarity)
    {
        float rarityMultiplier = rarity switch
        {
            UpgradeRarity.Broken => Random.Range(0.33f, 1.1f),
            UpgradeRarity.Common => Random.Range(0.9f, 1.65f),
            UpgradeRarity.Uncommon => Random.Range(2f, 3f),
            UpgradeRarity.Rare => Random.Range(4f, 6.5f),
            UpgradeRarity.Epic => Random.Range(7.5f, 10.5f),
            UpgradeRarity.Legendary => Random.Range(10.5f, 17.5f),
            _ => 1f
        };

        int percentage = Mathf.RoundToInt(10 * rarityMultiplier);
        return percentage;
    }

    static UpgradeCost GetUpgradeCost(UpgradeRarity rarity)
    {
        if (AureaAnima == null)
            AureaAnima = Resources.Load<CraftingMaterial>("Assets/CraftingMaterials/AureaAnima.asset");
        var xpCost = rarity switch
        {
            UpgradeRarity.Broken => Random.Range(3, 10),
            UpgradeRarity.Common => Random.Range(10, 30),
            UpgradeRarity.Uncommon => Random.Range(40, 50),
            UpgradeRarity.Rare => Random.Range(50, 100),
            UpgradeRarity.Epic => Random.Range(100, 201),
            UpgradeRarity.Legendary => Random.Range(300, 501),
            _ => 0
        };
        var upgradeCost = new UpgradeCost(AureaAnima, xpCost);
        return upgradeCost;
    }



    public static int GetRandomUpgradeIndex(UpgradeRarity rarity, int skippeableIndex)
    {
        List<StoreUpgradeData?> list = GetList(rarity);
        int index = HelperMethods.RandomNumberExcept(0, list.Count, skippeableIndex);
        return index;
    }
    public static int GetRandomUpgradeIndex(UpgradeRarity rarity)
    {
        List<StoreUpgradeData?> list = GetList(rarity);
        int index = Random.Range(0, list.Count);
        return index;
    }


    public static StoreUpgradeData GetUpgradeFromList(UpgradeRarity rarity, int index)
    {
        var list = GetList(rarity);
        return list[index].Value;
    }

}
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