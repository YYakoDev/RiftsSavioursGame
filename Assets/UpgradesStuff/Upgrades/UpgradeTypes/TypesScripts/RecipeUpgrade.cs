using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "RecipeUpgrade")]
public class RecipeUpgrade : SOUpgradeBase
{
    [SerializeField]protected CraftingMaterial _material;
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        upgradesManager.AddMaterial(_material);
    }

    protected override void SetDescription()
    {
        base.SetDescription();
        _description = $"You will obtain 1 pieces of {_material.name}";
    }

    private void OnValidate() {
        SetSprite();
    }

    public void SetSprite()
    {
        if(_material == null) return;
        if(_material.Sprite == null) _material.Sprite = Sprite;
    }
}
