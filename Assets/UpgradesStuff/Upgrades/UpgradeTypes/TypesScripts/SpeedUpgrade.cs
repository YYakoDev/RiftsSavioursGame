using UnityEngine;



[CreateAssetMenu(menuName = MenuPath + "SpeedUp")]
public class SpeedUpgrade : SOUpgradeBase
{
    [SerializeField, Range(0,100)]private int _speedUpPercentage = 10;

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);

    }
}
