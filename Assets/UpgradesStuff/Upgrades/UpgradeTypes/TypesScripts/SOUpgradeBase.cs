using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOUpgradeBase : ScriptableObject
{
    public const string MenuPath = "ScriptableObjects/Upgrades/";
    UpgradeGroup _parent;

    [SerializeField]private string _name;
    [SerializeField]private string _effectDescription;
    [SerializeField]private Sprite _sprite;
    [SerializeField]private UpgradeCost[] _costs;
    //[SerializeField]private CraftingMaterial[] _craftingMaterials;

    //properties
    public string Name => _name;
    public string EffectDescription => _effectDescription;
    public Sprite Sprite => _sprite;
    public UpgradeCost[] Costs => _costs;

    private void OnValidate() {
        if(_costs?.Length >= 3)
        {
            Array.Resize<UpgradeCost>(ref _costs, 3);
        }
    }
    public void SetGroup(UpgradeGroup group)
    {
        _parent = group;
    }

    public virtual void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        _parent.AdvanceIndex();
    }

    
}


