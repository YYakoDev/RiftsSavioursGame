using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "RecipeUpgrade")]
public class RecipeUpgrade : SOUpgradeBase
{
    [SerializeField] CraftingMaterial _material;
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        upgradesManager.AddMaterial(_material);
    }

    private void OnValidate() {
        if(_material == null) return;
        if(_material.Sprite == null) _material.Sprite = Sprite;
    }
}