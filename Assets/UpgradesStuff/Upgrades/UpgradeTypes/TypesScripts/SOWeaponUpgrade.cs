using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "Weapon Upgrade")]
public class SOWeaponUpgrade : SOUpgradeBase
{
    public override void SetGroup(UpgradeGroup group)
    {
        base.SetGroup(group);
    }

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
    }
}
