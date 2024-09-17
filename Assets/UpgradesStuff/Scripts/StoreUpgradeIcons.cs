using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoreUpgradeIcons
{
    private static Sprite healthIcon, speedIcon, toolIcon, defenseIcon, buffIcon, swordIcon, summonIcon, bowIcon;
    private static readonly string iconsPath = "StoreUpgradeIcons/";
    private static bool loaded = false;
    public static Dictionary<StatsTypes, Sprite> _storeUpgradesIcons;

    static void LoadResources()
    {
        if(loaded) return;
        LoadSprite(ref healthIcon, "HeartIcon");
        LoadSprite(ref speedIcon, "SpeedIcon");
        LoadSprite(ref toolIcon, "ToolIcon");
        LoadSprite(ref defenseIcon, "DefenseIcon");
        LoadSprite(ref buffIcon, "BuffIcon");
        LoadSprite(ref swordIcon, "SwordIcon");
        LoadSprite(ref summonIcon, "SummonIcon");
        LoadSprite(ref bowIcon, "BowIcon");
        loaded = true;
        void LoadSprite(ref Sprite sprite, string fileName)
        {
            sprite = Resources.Load<Sprite>(iconsPath + fileName);
        }
        _storeUpgradesIcons = new()
        {
            {StatsTypes.MaxHealth, healthIcon},
            {StatsTypes.CurrentHealth, healthIcon},
            {StatsTypes.Speed, speedIcon},
            {StatsTypes.SlowdownMultiplier, speedIcon},
            {StatsTypes.DashSpeed, speedIcon},
            {StatsTypes.DashCooldown, speedIcon},
            {StatsTypes.DashInvulnerabilityTime, speedIcon},
            {StatsTypes.PickUpRange, toolIcon},
            {StatsTypes.ToolsRange, toolIcon},
            {StatsTypes.ToolsDamage, toolIcon},
            {StatsTypes.ToolsCooldown, toolIcon},
            {StatsTypes.MaxAmountOfTools, toolIcon},
            {StatsTypes.StunResistance, defenseIcon},
            {StatsTypes.KnockbackResistance, defenseIcon},
            {StatsTypes.DamageResistance, defenseIcon},
            {StatsTypes.BuffBooster, buffIcon},
            {StatsTypes.DebuffResistance, buffIcon},
            {StatsTypes.Faith, buffIcon},
            {StatsTypes.HarvestMultiplier, toolIcon},
            {StatsTypes.DamageMultiplier, swordIcon},
            {StatsTypes.DamageAddition, swordIcon},
            {StatsTypes.AttackRange, swordIcon},
            {StatsTypes.AttackCooldown, swordIcon},
            {StatsTypes.AttackKnockback, swordIcon},
            {StatsTypes.ProjectilesCount, bowIcon},
            {StatsTypes.ProjectilesSpeed, bowIcon},
            {StatsTypes.SummonDamage, summonIcon},
            {StatsTypes.SummonSpeed, summonIcon},
            {StatsTypes.AttackSpeed, swordIcon},
            {StatsTypes.CriticalChance, swordIcon},
            {StatsTypes.CriticalDamageMultiplier, swordIcon},
        };
    }

    public static Sprite GetUpgradeIcon(StatsTypes statType)
    {
        LoadResources();
        return _storeUpgradesIcons[statType];
    }


}
