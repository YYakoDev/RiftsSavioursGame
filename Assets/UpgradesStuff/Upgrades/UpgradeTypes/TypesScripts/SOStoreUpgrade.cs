using UnityEngine;

public class SOStoreUpgrade : StatChangingUpgrade
{
    [SerializeField]protected UpgradeRarity _rarity;
    int _listIndex = -1;
    //public int ListIndex => _listIndex;
    public new int Costs => _costs[0].Cost;
    public UpgradeRarity Rarity => _rarity;
    public void Initialize(StoreUpgradeData upgradeData)
    {
        _name = upgradeData.Name;
        _sprite = upgradeData.Icon;
        _costs = upgradeData.Costs;
        _statsToModify = upgradeData.StatsTypes;
        _modifications = upgradeData.Modifications;
        _rarity = upgradeData.Rarity;
        //_listIndex = listIndex;
        SetDescription();
    }
    public override void SetGroup(UpgradeGroup group)
    {}

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        StatChanging(upgradesManager);
    }
}