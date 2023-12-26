using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "AtkRange")]
public class AtkRangeUpgrade : SOUpgradeTwoAdditionTypes
{
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
    }

    public override void AddFlatAmount(PlayerUpgradesManager upgradesManager)
    {
        base.AddFlatAmount(upgradesManager);
    }
    public override void AddPercentage(PlayerUpgradesManager upgradesManager)
    {
        base.AddPercentage(upgradesManager);
    }
}

