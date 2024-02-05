using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerInventory")]
public class SOPlayerInventory : ScriptableObject
{
    Dictionary<CraftingMaterial, int> _ownedMaterials = new();
    Dictionary<SOUpgradeBase, int> _equippedUpgrades = new();
    [SerializeField]int _maxUpgradesCount = 5;
    private PlayerUpgradesManager _upgradesManager;
    public event Action onInventoryChange;
    public event Action<SOUpgradeBase> onUpgradeAdded;
    
    //properties
    public int MaxUpgradesCount 
    {
        get => _maxUpgradesCount;
        set => _maxUpgradesCount = value;
    }
    public Dictionary<CraftingMaterial, int> OwnedMaterials => _ownedMaterials;
    public Dictionary<SOUpgradeBase, int> EquippedUpgrades => _equippedUpgrades;
 

    public void Initialize(PlayerUpgradesManager upgradesManager)
    {
        //Here you can grab the materials and upgrades from the last save if there is any
        //also apply the equipped upgrades if there is any
        _ownedMaterials = new();
        _equippedUpgrades = new();
        _upgradesManager = upgradesManager;
        AssetMenuUpdators.UpdateCraftingMaterialIcons();
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
        onInventoryChange?.Invoke();
    }
    public void AddMaterial(CraftingMaterial craftingMaterial, int amount)
    {
        if(_ownedMaterials.ContainsKey(craftingMaterial))
        {
            _ownedMaterials[craftingMaterial] += amount;
        }else
        {
            _ownedMaterials.Add(craftingMaterial,amount);
        }
        onInventoryChange?.Invoke();
    }

    public void RemoveMaterial(CraftingMaterial matToRemove, int amount)
    {
        if(_ownedMaterials.ContainsKey(matToRemove)) _ownedMaterials[matToRemove] -= amount;
        
        onInventoryChange?.Invoke();
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
        onUpgradeAdded?.Invoke(upgrade);
        //Debug.Log($"added {upgrade.name} upgrade and now you have {_equippedUpgrades[upgrade]}");
        upgrade.ApplyEffect(_upgradesManager);
    }
}
