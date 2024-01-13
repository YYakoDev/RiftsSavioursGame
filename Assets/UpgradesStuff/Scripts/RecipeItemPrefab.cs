using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RecipeItemPrefab : MonoBehaviour
{

    [SerializeField]Image i_itemIcon;
    [SerializeField]TextMeshProUGUI t_craftingCost;


    private void Awake()
    {
        gameObject.CheckComponent<Image>(ref i_itemIcon);    
        gameObject.CheckComponent<TextMeshProUGUI>(ref t_craftingCost);
    }

    public void Initialize(UpgradeCost requirement, int ownedMaterials)
    {
        i_itemIcon.sprite = requirement.CraftingMaterial.Sprite;
        Color costTextColor = (ownedMaterials >= requirement.Cost) ? UIColors.GetColor() : UIColors.GetColor(UIColor.Red);
        t_craftingCost.text = $"{ownedMaterials}/{requirement.Cost}";
        t_craftingCost.color = costTextColor;
    }
    
}

