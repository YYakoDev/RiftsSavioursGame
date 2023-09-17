using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerInventory")]
public class SOPlayerInventory : ScriptableObject
{
    Dictionary<CraftingMaterial, int> _ownedMaterials = new();
    Dictionary<SOUpgradeBase, int> _equippedUpgrades = new();
    [SerializeField]int _maxUpgradesCount = 5;

    //properties
    public int MaxUpgradesCount 
    {
        get => _maxUpgradesCount;
        set => _maxUpgradesCount = value;
    }


    public void Initialize()
    {
        //Here you can grab the materials and upgrades from the last save if there is any
        //also apply the equipped upgrades if there is any
        _ownedMaterials = new();
        _equippedUpgrades = new();
    }

    public void AddMaterial(CraftingMaterial craftingMaterial)
    {
        if(_ownedMaterials.ContainsKey(craftingMaterial))
        {
            _ownedMaterials[craftingMaterial]++;
        }else
        {
            _ownedMaterials.Add(craftingMaterial,1);
        }
    }

    public void AddUpgrade(SOUpgradeBase upgrade)
    {
        if(_equippedUpgrades.ContainsKey(upgrade))
        {
            _equippedUpgrades[upgrade]++;
        }else
        {
            _equippedUpgrades.Add(upgrade, 1);
        }

        Debug.Log($"added {upgrade.name} material and now you have {_equippedUpgrades[upgrade]}");
    }
}
