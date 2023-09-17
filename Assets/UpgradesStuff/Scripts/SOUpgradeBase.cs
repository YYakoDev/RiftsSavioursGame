using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOUpgradeBase : ScriptableObject
{
    public const string MenuPath = "ScriptableObjects/Upgrades/";

    [SerializeField]private string _name;
    [SerializeField]private string _effectDescription;
    [SerializeField]private Sprite _sprite;
    [SerializeField]private UpgradeCost[] _upgradeCosts;
    //[SerializeField]private CraftingMaterial[] _craftingMaterials;

    //properties
    public string Name => _name;
    public string EffectDescription => _effectDescription;
    public Sprite Sprite => _sprite;
    public UpgradeCost[] UpgradeCosts => _upgradeCosts;

    private void OnValidate() {
        if(_upgradeCosts.Length >= 3)
        {
            Array.Resize<UpgradeCost>(ref _upgradeCosts, 3);
        }
    }

    public virtual void ApplyEffect(PlayerUpgradesInventory playerUpgrades)
    {

    }

    
}


