using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOUpgradeBase : ScriptableObject
{
    public const string MenuPath = "ScriptableObjects/Upgrades/";
    protected UpgradeGroup _parent;

    [SerializeField]private string _name;
    [SerializeField]private UpgradeDescription[] _descriptions;
    [SerializeField]private Sprite _sprite;
    [SerializeField]private UpgradeCost[] _costs;
    //[SerializeField]private CraftingMaterial[] _craftingMaterials;

    //properties
    public UpgradeGroup GroupParent => _parent;
    public string Name => _name;
    public UpgradeDescription[] EffectDescriptions => _descriptions;
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

[System.Serializable]
public class UpgradeDescription : System.Object
{
    [SerializeField]private string text;
    [SerializeField]UIColor color;

    public string Text => text;
    public Color Color => UIColors.GetColor(color);
}




