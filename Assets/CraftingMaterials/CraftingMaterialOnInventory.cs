using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryMaterialData
{
    [SerializeField]private CraftingMaterial _crafingMaterial;
    public int Amount;
    public CraftingMaterial Material => _crafingMaterial;
    public InventoryMaterialData(CraftingMaterial mat, int amount)
    {
        _crafingMaterial = mat;
        Amount = amount;
    }
}
