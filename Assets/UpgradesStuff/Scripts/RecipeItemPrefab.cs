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

    public void Initialize(UpgradeCost cost)
    {
        i_itemIcon.sprite = cost.CraftingMaterial.Sprite;
        t_craftingCost.text = cost.Cost.ToString();
    }
    
}

