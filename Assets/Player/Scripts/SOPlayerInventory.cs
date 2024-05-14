using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerInventory")]
public class SOPlayerInventory : ScriptableObject
{
    Dictionary<CraftingMaterial, int> _ownedMaterials = new();
    List<UpgradeGroup> _equippedUpgrades = new();
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
    public List<UpgradeGroup> EquippedUpgrades => _equippedUpgrades;
 

    public void Initialize(PlayerUpgradesManager upgradesManager, List<InventoryMaterialData> startingMaterials = null)
    {
        //Here you can grab the materials and upgrades from the last save if there is any
        //also apply the equipped upgrades if there is any
        _ownedMaterials = new();
        _equippedUpgrades = new();
        _upgradesManager = upgradesManager;
        if(startingMaterials != null)
        foreach(var inventoryItemData in startingMaterials)
        {
            if(inventoryItemData.Material == null) continue;
            AddMaterial(inventoryItemData.Material, inventoryItemData.Amount);
        }
        //AssetMenuUpdators.UpdateCraftingMaterialIcons();
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

    public int GetMaterialOwnedCount(CraftingMaterial mat)
    {
        if(_ownedMaterials.ContainsKey(mat)) return _ownedMaterials[mat];
        else return 0;
    }

    public void AddUpgrade(SOUpgradeBase upgrade)
    {
        _equippedUpgrades.Add(upgrade.GroupParent);
        onUpgradeAdded?.Invoke(upgrade);
        //Debug.Log($"added {upgrade.name} upgrade and now you have {_equippedUpgrades[upgrade]}");
        upgrade.ApplyEffect(_upgradesManager);
    }
}
