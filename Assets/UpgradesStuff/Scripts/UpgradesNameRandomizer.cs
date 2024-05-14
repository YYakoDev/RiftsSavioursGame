using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradesNameRandomizer
{
    struct StringMultiple
    {
        public string[] strings;
        public string text => strings[Random.Range(0, strings.Length)];
        public StringMultiple(params string[] strings)
        {
            this.strings = strings;
        }
    }
    private static Dictionary<StatsTypes, StringMultiple> itemNames = new()
    {
        {StatsTypes.MaxHealth, new StringMultiple("Soup Bowl", "Toughness Elixir", "Elixir", "Life Potion") },
        {StatsTypes.CurrentHealth, new StringMultiple("Heart Necklace", "Healing Potion") },
        {StatsTypes.Speed, new StringMultiple("Swiftness Potion", "Shoes", "Boots") },
        {StatsTypes.SlowdownMultiplier, new StringMultiple("Lost wing", "Feather") },
        {StatsTypes.DashSpeed, new StringMultiple("Sprint Potion") },
        {StatsTypes.DashCooldown, new StringMultiple("Stamina Potion") },
        {StatsTypes.DashInvulnerabilityTime, new StringMultiple("Protective Shoes") },
        {StatsTypes.PickUpRange, new StringMultiple("Magnet", "Gloves") },
        {StatsTypes.ToolsRange, new StringMultiple("Tool Handle", "Metal Detector") },
        {StatsTypes.ToolsDamage, new StringMultiple("Tool Reinforcement") },
        {StatsTypes.ToolsCooldown, new StringMultiple("Goggles") },
        {StatsTypes.MaxAmountOfTools, new StringMultiple("Tool belt") },
        {StatsTypes.StunResistance, new StringMultiple("Helmet") },
        {StatsTypes.KnockbackResistance, new StringMultiple("Wooden Shield") },
        {StatsTypes.DamageResistance, new StringMultiple("Steel coating") },
        {StatsTypes.BuffBooster, new StringMultiple("Lucky Charm") },
        {StatsTypes.DebuffResistance, new StringMultiple("Holy Water") },
        {StatsTypes.Faith, new StringMultiple("Cross") },
        {StatsTypes.HarvestMultiplier, new StringMultiple("Fertilizer")},
        {StatsTypes.DamageMultiplier, new StringMultiple("Whetstone")},
        {StatsTypes.BaseDamageAddition, new StringMultiple("Handle Reinforcement")},
        {StatsTypes.AttackSpeed, new StringMultiple("Blade Reforging")},
        {StatsTypes.AttackRange, new StringMultiple("Hilt")},
        {StatsTypes.AttackKnockback, new StringMultiple("Pommel")},
        {StatsTypes.AttackCooldown, new StringMultiple("Fuller")},
        {StatsTypes.ProjectilesCount, new StringMultiple("Quiver")},
        {StatsTypes.ProjectilesSpeed, new StringMultiple("Gunpowder")},
        {StatsTypes.SummonDamage, new StringMultiple("Voodoo Doll")},
        {StatsTypes.SummonSpeed, new StringMultiple("Totem")},
    };

    private static Dictionary<StatsTypes, StringMultiple> itemAdjectives = new()
    {
        {StatsTypes.MaxHealth, new StringMultiple("Healthy", "Lovely") },
        {StatsTypes.CurrentHealth, new StringMultiple("Beautiful", "Charming") },
        {StatsTypes.Speed, new StringMultiple("Rapid", "Hasty", "Mercury's") },
        {StatsTypes.SlowdownMultiplier, new StringMultiple("Slow", "Sluggish") },
        {StatsTypes.DashSpeed, new StringMultiple("Quick", "Brisk", "Speedy") },
        {StatsTypes.DashCooldown, new StringMultiple("Balanced", "Steady") },
        {StatsTypes.DashInvulnerabilityTime, new StringMultiple("Preventive", "Precautionary") },
        {StatsTypes.PickUpRange, new StringMultiple("Magnetic") },
        {StatsTypes.ToolsRange, new StringMultiple("Farmlike", "Fresh") },
        {StatsTypes.ToolsDamage, new StringMultiple("Rustic", "Mechanized") },
        {StatsTypes.ToolsCooldown, new StringMultiple("Rural") },
        {StatsTypes.MaxAmountOfTools, new StringMultiple("Little", "Farmish") },
        {StatsTypes.StunResistance, new StringMultiple("Head Protective")},
        {StatsTypes.KnockbackResistance, new StringMultiple("Punch Resistant") },
        {StatsTypes.DamageResistance, new StringMultiple("Steeled", "Formidable") },
        {StatsTypes.BuffBooster, new StringMultiple("Bleesed") },
        {StatsTypes.DebuffResistance, new StringMultiple("Flawless", "Serene") },
        {StatsTypes.Faith, new StringMultiple("Church's", "Hopeful") },
        {StatsTypes.HarvestMultiplier, new StringMultiple("Harvested", "Grass-Fed")},
        {StatsTypes.DamageMultiplier, new StringMultiple("Strong", "Powerful")},
        {StatsTypes.BaseDamageAddition, new StringMultiple("Warborn", "Firm")},
        {StatsTypes.AttackSpeed, new StringMultiple("Reforged", "Meteoric")},
        {StatsTypes.AttackRange, new StringMultiple("Elongated", "Large", "Extensive")},
        {StatsTypes.AttackKnockback, new StringMultiple("Stunning")},
        {StatsTypes.AttackCooldown, new StringMultiple("Fastest")},
        {StatsTypes.ProjectilesCount, new StringMultiple("Loaded", "barrelled")},
        {StatsTypes.ProjectilesSpeed, new StringMultiple("Asian", "Chinese")},
        {StatsTypes.SummonDamage, new StringMultiple("Dark", "Raven", "Dusky")},
        {StatsTypes.SummonSpeed, new StringMultiple("Necromantic", "Demon Worshipper", "Sacrificial")},
    };
    private static Dictionary<StatsTypes, StringMultiple> thirdItemName = new()
    {
        {StatsTypes.MaxHealth, new StringMultiple("of the bear", "of hearts") },
        {StatsTypes.CurrentHealth, new StringMultiple("of restoration", "and health") },
        {StatsTypes.Speed, new StringMultiple("of Mercury") },
        {StatsTypes.SlowdownMultiplier, new StringMultiple("for attacking") },
        {StatsTypes.DashSpeed, new StringMultiple("of swiftness") },
        {StatsTypes.DashCooldown, new StringMultiple("of balance") },
        {StatsTypes.DashInvulnerabilityTime, new StringMultiple("of protection") },
        {StatsTypes.PickUpRange, new StringMultiple("with magnetism") },
        {StatsTypes.ToolsRange,  new StringMultiple("from the farmer", "of the farm") },
        {StatsTypes.ToolsDamage, new StringMultiple("with farmer's strength", "from the countryside") },
        {StatsTypes.ToolsCooldown,  new StringMultiple("for work", "from a farm") },
        {StatsTypes.MaxAmountOfTools, new StringMultiple("with extra tools", "with extra tools") },
        {StatsTypes.StunResistance, new StringMultiple("with head protection") },
        {StatsTypes.KnockbackResistance, new StringMultiple("resistant to punches") },
        {StatsTypes.DamageResistance, new StringMultiple("coated with steel") },
        {StatsTypes.BuffBooster, new StringMultiple("of blessings") },
        {StatsTypes.DebuffResistance, new StringMultiple("from the holiness", "bathed in saint's tears") },
        {StatsTypes.Faith, new StringMultiple("of spirits and tears", "of luck", "della Vittoria", "gift") },
        {StatsTypes.HarvestMultiplier, new StringMultiple("for the soil")},
        {StatsTypes.DamageMultiplier, new StringMultiple("of strength", "from the war", "for combat", "della Vittoria")},
        {StatsTypes.BaseDamageAddition, new StringMultiple("of strength", "for battle")},
        {StatsTypes.AttackSpeed, new StringMultiple("reforged", "with weight reduction", "with weight reduction magic")},
        {StatsTypes.AttackRange, new StringMultiple("with extended reach", "with longer reach")},
        {StatsTypes.AttackKnockback, new StringMultiple("for stronger push", "with stronger knockback")},
        {StatsTypes.AttackCooldown, new StringMultiple("for swift blows", "to strike faster")},
        {StatsTypes.ProjectilesCount, new StringMultiple("of that famous archer", "stolen from a trench", "from the previous war")},
        {StatsTypes.ProjectilesSpeed, new StringMultiple("discovered in China", "from the alchemist")},
        {StatsTypes.SummonDamage, new StringMultiple("of the forest witch", "from the witch", "born in Warnow", "found in the forest")},
        {StatsTypes.SummonSpeed, new StringMultiple("from the sorceress", "of the priest")},
    };
    private static Dictionary<StatsTypes, StringMultiple> fourthItemName = new()
    {
        {StatsTypes.MaxHealth, new StringMultiple("from a fountain") },
        {StatsTypes.CurrentHealth, new StringMultiple("and health") },
        {StatsTypes.Speed, new StringMultiple(", found in a meteour") },
        {StatsTypes.SlowdownMultiplier, new StringMultiple("traded from a snail alien") },
        {StatsTypes.DashSpeed, new StringMultiple("that is timeless") },
        {StatsTypes.DashCooldown, new StringMultiple("that provides balance") },
        {StatsTypes.DashInvulnerabilityTime, new StringMultiple("for the protection of the weak") },
        {StatsTypes.PickUpRange, new StringMultiple("for greedy hands") },
        {StatsTypes.ToolsRange, new StringMultiple(", found in a barn") },
        {StatsTypes.ToolsDamage, new StringMultiple("from the cowhouse") },
        {StatsTypes.ToolsCooldown, new StringMultiple("crafted by a shepherd") },
        {StatsTypes.MaxAmountOfTools, new StringMultiple("with wool thread") },
        {StatsTypes.StunResistance, new StringMultiple("from deimos") },
        {StatsTypes.KnockbackResistance, new StringMultiple("fitting for a templar") },
        {StatsTypes.DamageResistance, new StringMultiple(", gifted from Vulcanus", "to protect yourself from evil") },
        {StatsTypes.BuffBooster, new StringMultiple("from the previous run") },
        {StatsTypes.DebuffResistance, new StringMultiple("that is from another timeline") },
        {StatsTypes.Faith, new StringMultiple(", gifted by the True God", "that defies godhood") },
        {StatsTypes.HarvestMultiplier, new StringMultiple("for the soil")},
        {StatsTypes.DamageMultiplier, new StringMultiple("that splits the sea", "That cuts mountains", "that destroys all evil", ", gifted from my swordmaster")},
        {StatsTypes.BaseDamageAddition, new StringMultiple("forged to kill a god", "that slays the stars")},
        {StatsTypes.AttackSpeed, new StringMultiple("that could rival the speed of a god")},
        {StatsTypes.AttackRange, new StringMultiple("to prevent invasions")},
        {StatsTypes.AttackKnockback, new StringMultiple("for feints")},
        {StatsTypes.AttackCooldown, new StringMultiple("that gives you energy", "that fills you with determination", "for those who do not backdown")},
        {StatsTypes.ProjectilesCount, new StringMultiple(", that can carry more ammunitions")},
        {StatsTypes.ProjectilesSpeed, new StringMultiple(", with gunpowder inside")},
        {StatsTypes.SummonDamage, new StringMultiple(", that was present in the dragon tournament in mars")},
        {StatsTypes.SummonSpeed, new StringMultiple("from yours truly", "stolen from a necromancer")},
    };

    private static Dictionary<UpgradeRarity, StringMultiple> RarityNames = new()
    {
        {UpgradeRarity.Broken, new StringMultiple("Discarded", "Broken", "Malfunctioning", "Imperfect", "Stolen", "Hollow", "Shattered", "Crushed", "Torn") },
        {UpgradeRarity.Common, new StringMultiple("Common", "Ordinary", "Average", "Regular", "Usual", "Quotidian") },
        {UpgradeRarity.Uncommon, new StringMultiple("Uncommon", "Scarce", "Unusual", "Unexpected", "Atypical") },
        {UpgradeRarity.Rare, new StringMultiple("Rare", "Precious", "Valuable", "Scarce", "Abnormal", "Golden") },
        {UpgradeRarity.Epic, new StringMultiple("Epic", "Mythical", "Royal", "Pure", "Invaluable", "Heroic", "Homeric", "Historical") },
        {UpgradeRarity.Legendary, new StringMultiple("Legendary", "Godly", "Fable Level", "Divine", "Glorious", "Holy", "Ancient", "Saintly") },
    };

    public static string GetName(UpgradeRarity rarity, StatsTypes modifiedStat, StatsTypes? secondayStat, StatsTypes? terciaryStat, StatsTypes? quaternyStat)
    {
        var RarityPrefix = RarityNames[rarity].text;
        var itemName = itemNames[modifiedStat].text;
        var Adjective = "";
        var thirdName = "";
        var fourthName = "";
        if(secondayStat != null) Adjective = itemAdjectives[secondayStat.Value].text;
        if(terciaryStat != null) thirdName = thirdItemName[terciaryStat.Value].text;
        if(quaternyStat != null) fourthName = fourthItemName[quaternyStat.Value].text;
        string result = $"{RarityPrefix} {Adjective} {itemName} {thirdName} {fourthName}";
        return result;
    }
}
