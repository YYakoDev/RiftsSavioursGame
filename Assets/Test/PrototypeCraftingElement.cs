using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrototypeCraftingElement : MonoBehaviour
{
    SOPlayerInventory _playerInventory;
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] RecipeItemPrefab _costPrefab;
    [SerializeField] Transform _costsContainer;
    RecipeItemPrefab[] _instantiatedCosts;


    public void Initialize(SOUpgradeBase upgrade, SOPlayerInventory inventory)
    {
        _playerInventory = inventory;
        _icon.sprite = upgrade.Sprite;
        _name.text = upgrade.Name;
        SetCosts(upgrade.Costs);
    }

    void SetCosts(UpgradeCost[] costs)
    {
        if(_instantiatedCosts == null) CreateCostItems();
        var length = costs.Length;
        if(length > 3)
        {
            Debug.Log("<color='red'> Upgrades' cost should be only 3 elements </color>");
            length = 3;
        }
        for (int i = 0; i < length; i++)
        {
            var costData = costs[i];
            var ownedAmount = _playerInventory.GetMaterialOwnedCount(costData.CraftingMaterial);
            _instantiatedCosts[i].Initialize(costData, ownedAmount);
            _instantiatedCosts[i].gameObject.SetActive(true);
        }
    }


    void CreateCostItems()
    {
        _instantiatedCosts = new RecipeItemPrefab[3];
        for (int i = 0; i < 3; i++)
        {
            _instantiatedCosts[i] = Instantiate(_costPrefab, _costsContainer);
            _instantiatedCosts[i].gameObject.SetActive(false);
        }
    }

    public void UpdateCostItems(SOUpgradeBase upgrade)
    {
        SetCosts(upgrade.Costs);
    }
}
