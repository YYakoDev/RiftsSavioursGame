using UnityEngine;

public class SOStoreUpgrade : StatChangingUpgrade
{
    int _listIndex = -1;
    public int ListIndex => _listIndex;
    public new int Costs => _costs[0].Cost;
    public void Initialize(StoreUpgradeData upgradeData, int listIndex)
    {
        _name = upgradeData.Name;
        _sprite = upgradeData.Icon;
        _costs = upgradeData.Costs;
        _statsToModify = upgradeData.StatsTypes;
        _modifications = upgradeData.Modifications;
        _rarity = upgradeData.Rarity;
        _listIndex = listIndex;
        SetDescription();
    }
    public override void SetGroup(UpgradeGroup group)
    {}

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        StatChanging(upgradesManager);
    }
}