using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "MultipleMatRecipeUpgrade")]
public class SOMultipleMatRecipeUpgrade : RecipeUpgrade
{
    [SerializeField]int _amountToGive = 2;
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        upgradesManager.AddMaterial(_material, _amountToGive);           
    }
}
