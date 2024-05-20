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
    //private static string Location = "Assets/UpgradesStuff/Upgrades/RandomizedUpgrades/";
    private static CraftingMaterial AureaAnima;
    static List<StoreUpgradeData?> upgrades;
    static int brokenUpgradesMaxIndex, commonUpgradesMaxIndex, uncommonUpgradesMaxIndex, rareUpgradesMaxIndex, epicUpgradesMaxIndex, legendaryUpgradesMaxIndex;
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
        StatsTypes[] statTypes = new StatsTypes[totalStats.Length - 7]; //THIS MINUS 7 IS EQUAL TO THE AMOUNT OF SKIPS IN THE FOR THE LOOP BELLOW IF YOU FORGET THIS YOU WILL GET REPEATED MAX HEALTH UPGRADES
        int index = 0;
        for (int i = 0; i < totalStats.Length; i++)
        {
            var statType = totalStats[i];
            if (statType == StatsTypes.CurrentHealth 
            || statType == StatsTypes.SlowdownMultiplier 
            || statType == StatsTypes.DebuffResistance 
            || statType == StatsTypes.BuffBooster
            || statType == StatsTypes.StunResistance
            || statType == StatsTypes.Faith
            || statType == StatsTypes.AttackSpeed) continue;
            statTypes[index] = statType;
            index++;
        }
        var rarityLevels = Enum.GetValues(typeof(UpgradeRarity)) as UpgradeRarity[];
        upgrades = new(MaxIterations);

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
            switch(rarity)
            {
                case UpgradeRarity.Broken:
                    brokenUpgradesMaxIndex = Iterations;
                    break;
                case UpgradeRarity.Common:
                    commonUpgradesMaxIndex = Iterations;
                    break;
                case UpgradeRarity.Uncommon:
                    uncommonUpgradesMaxIndex = Iterations;
                    break;
                case UpgradeRarity.Rare:
                    rareUpgradesMaxIndex = Iterations;
                    break;
                case UpgradeRarity.Epic:
                    epicUpgradesMaxIndex = Iterations;
                    break;
                case UpgradeRarity.Legendary:
                    legendaryUpgradesMaxIndex = Iterations;
                    break;
            }
        }
        Debug.Log("<b>Created " + Iterations + " upgrades</b>" + "  Upgrade creation duration:   " + sw.ElapsedMilliseconds + " ms");
        areUpgradesCreated = true;
        upgrades.RemoveAll(item => item == null);
        sw.Stop();
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
            bool ignoreNegative = (Random.Range(0, 5) < 1);
            var isNegative = Random.Range(0, 100) > negativeChances;
            if (isNegative && negativeCount < maxNegativeAmount && !ignoreNegative)
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

        if (Iterations >= upgrades.Count)
        {
            upgrades.Add(upgrade);
            Iterations++;
            return;
        }
        upgrades[Iterations] = upgrade;
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
            > 0f and < 0.35f => 1.55f,
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
            UpgradeRarity.Broken => Random.Range(0.55f, 1f),
            UpgradeRarity.Common => Random.Range(0.9f, 1.75f),
            UpgradeRarity.Uncommon => Random.Range(2f, 3.55f),
            UpgradeRarity.Rare => Random.Range(4f, 5.55f),
            UpgradeRarity.Epic => Random.Range(6.55f, 9.5f),
            UpgradeRarity.Legendary => Random.Range(9.5f, 15.5f),
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
            UpgradeRarity.Broken => Random.Range(3, 8),
            UpgradeRarity.Common => Random.Range(8, 21),
            UpgradeRarity.Uncommon => Random.Range(25, 45),
            UpgradeRarity.Rare => Random.Range(55, 105),
            UpgradeRarity.Epic => Random.Range(105, 201),
            UpgradeRarity.Legendary => Random.Range(200, 400),
            _ => 0
        };
        var upgradeCost = new UpgradeCost(AureaAnima, xpCost);
        return upgradeCost;
    }



    public static int GetRandomUpgradeIndex(UpgradeRarity rarity, params int[] skippeableIndexes)
    {
        if(upgrades == null) CreateUpgrades();
        int minIndex = 0;
        int maxIndex = upgrades.Count;
        switch(rarity)
        {
            case UpgradeRarity.Broken:
                maxIndex = brokenUpgradesMaxIndex;
                break;
            case UpgradeRarity.Common:
                minIndex = brokenUpgradesMaxIndex;
                maxIndex = commonUpgradesMaxIndex;
                break;
            case UpgradeRarity.Uncommon:
                minIndex = commonUpgradesMaxIndex;
                maxIndex = uncommonUpgradesMaxIndex;
                break;
            case UpgradeRarity.Rare:
                minIndex = uncommonUpgradesMaxIndex;
                maxIndex = rareUpgradesMaxIndex;
                break;
            case UpgradeRarity.Epic:
                minIndex = rareUpgradesMaxIndex;
                maxIndex = epicUpgradesMaxIndex;
                break;
            case UpgradeRarity.Legendary:
                minIndex = epicUpgradesMaxIndex;
                break;
        }

        return HelperMethods.RandomRangeExcept(minIndex, maxIndex, exceptions: skippeableIndexes);
    }

    public static StoreUpgradeData GetUpgradeFromList(int index) => upgrades[index].Value;
    
    public static int GetUpgradesCount(UpgradeRarity rarity)
    {
        if(upgrades == null) CreateUpgrades();
        return rarity switch
        {
            UpgradeRarity.Broken => brokenUpgradesMaxIndex,
            UpgradeRarity.Common => commonUpgradesMaxIndex,
            UpgradeRarity.Uncommon => uncommonUpgradesMaxIndex,
            UpgradeRarity.Rare => rareUpgradesMaxIndex,
            UpgradeRarity.Epic => epicUpgradesMaxIndex,
            UpgradeRarity.Legendary => legendaryUpgradesMaxIndex,
            _ => upgrades.Count   
        };
    }
    
}