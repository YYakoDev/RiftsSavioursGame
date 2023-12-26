using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "Damage Up")]
public class DamageUpgrade : SOUpgradeTwoAdditionTypes
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
