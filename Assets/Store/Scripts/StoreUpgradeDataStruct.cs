using UnityEngine;
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