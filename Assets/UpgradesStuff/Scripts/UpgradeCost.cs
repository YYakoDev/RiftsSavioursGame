using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeCost
{
    public CraftingMaterial CraftingMaterial;
    public int Cost = 1;

    public UpgradeCost(CraftingMaterial craftingMaterial, int cost)
    {
        CraftingMaterial = craftingMaterial;
        Cost = cost;
    }
}
