using UnityEngine;

public class SOStoreUpgrade : StatChangingUpgrade
{
    public void Initialize(StoreUpgradeData upgradeData)
    {
        _name = upgradeData.Name;
        _sprite = upgradeData.Icon;
        _costs = upgradeData.Costs;
        _statsToModify = upgradeData.StatsTypes;
        _modifications = upgradeData.Modifications;
        _rarity = upgradeData.Rarity;
        SetDescription();
    }
    public override void SetGroup(UpgradeGroup group)
    {}

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        StatChanging(upgradesManager);
    }
}