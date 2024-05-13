using UnityEngine;

public class SOStoreUpgrade : StatChangingUpgrade
{

    public void Initialize(string name, Sprite icon, UpgradeRarity rarity, StatsTypes[] stats, StatModificationValue[] modifications, params UpgradeCost[] costs)
    {
        _costs = costs;
        _statsToModify = stats;
        _modifications = modifications;
        _name = name;
        _rarity = rarity;
        _sprite = icon;
        SetDescription();
    }
    public override void SetGroup(UpgradeGroup group)
    {}
}