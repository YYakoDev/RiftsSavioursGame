using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "Pick Up Range")]
public class PickUpRangeUpgrade : SOUpgradeBase
{
    [SerializeField, Range(0,100)]private int _percentage = 10;
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
    }
}