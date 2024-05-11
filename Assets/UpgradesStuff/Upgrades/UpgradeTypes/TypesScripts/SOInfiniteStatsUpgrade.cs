using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "InfiniteUpgrade")]
public class SOInfiniteUpgrade : StatChangingUpgrade
{
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        /*for (int i = 0; i < _savedProperties.Length; i++)
        {
            int index = _indexes[i];
            UpgradeStatChange statChange = _statsChanges[i];
            var property = _savedProperties[i];
            ApplyValueToProperty(property, statChange, upgradesManager);
        }*/
    }
}
