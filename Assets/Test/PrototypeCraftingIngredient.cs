using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrototypeCraftingIngredient : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _name, _ingredientsAmount;
    [SerializeField] Image _icon;

    public void Initialize(UpgradeCost requirement, int ownedAmount)
    {
        if(requirement != null)
        {
            _name.text = HelperMethods.AddSpacesToSentence(requirement.CraftingMaterial.name, false);
            _icon.gameObject.SetActive(true);
            _icon.sprite = requirement.CraftingMaterial.Sprite;    
            _ingredientsAmount.gameObject.SetActive(true);
            Color costTextColor = (ownedAmount >= requirement.Cost) ? UIColors.GetColor(UIColor.Black) : UIColors.GetColor(UIColor.Red);
            if(ownedAmount >= 1000) ownedAmount = 1000;
            string cost = $"{ownedAmount}/{requirement.Cost}";
            _ingredientsAmount.color = costTextColor;
            _ingredientsAmount.text = cost;
        }
        else
        {
            _name.text = string.Empty;
            _icon.gameObject.SetActive(false);
            _ingredientsAmount.gameObject.SetActive(false);
        }
    }
}
