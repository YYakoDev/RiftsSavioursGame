using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "MultipleMatRecipeUpgrade")]
public class SOMultipleMatRecipeUpgrade : RecipeUpgrade
{
    [SerializeField]int _amountToGive = 2;
    protected override void SetDescription()
    {
        _description = $"You will obtain {_amountToGive} pieces of {_material.name} ";
    }
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        upgradesManager.AddMaterial(_material, _amountToGive);           
    }
}
