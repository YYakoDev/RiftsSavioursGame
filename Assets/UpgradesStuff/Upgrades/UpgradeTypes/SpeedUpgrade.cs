using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = MenuPath + "SpeedUp")]
public class SpeedUpgrade : SOUpgradeBase
{
    [SerializeField, Range(0,100)]private int _speedUpAmount = 1;

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        upgradesManager.SpeedUp(_speedUpAmount);
    }
}
