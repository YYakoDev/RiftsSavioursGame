using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeCraftingRequest
{
    SOPlayerInventory _inventory;
    SOUpgradeBase _upgrade;
    float _elapsedTime;
    float _totalDuration;
    public PrototypeCraftingRequest(SOPlayerInventory inventory, SOUpgradeBase upgrade, float duration)
    {
        _inventory = inventory;
        _upgrade = upgrade;
        _totalDuration = duration;
    }

    public void Craft()
    {
        if(_elapsedTime >= _totalDuration) return;
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= _totalDuration)
        {
            _inventory.AddUpgrade(_upgrade);
        }
    }
}
