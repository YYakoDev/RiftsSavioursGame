using UnityEngine;

public class SOUpgradeTwoAdditionTypes : SOUpgradeBase
{
    [SerializeField] protected bool _useFlatAmount = false;
    [SerializeField, Range(0f,50f)]protected float _FlatAmount = 5f;
    [SerializeField, Range(0,100)]protected int _percentage = 10;
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        if(_useFlatAmount)
        {
            AddFlatAmount(upgradesManager);
        }else
        {
            AddPercentage(upgradesManager);
        }
    }
    public virtual void AddFlatAmount(PlayerUpgradesManager upgradesManager){}
    public virtual void AddPercentage(PlayerUpgradesManager upgradesManager){}
}
