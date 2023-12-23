using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemPrefab : MonoBehaviour
{
    
    [SerializeField]private TextMeshProUGUI _upgradeName; 
    [SerializeField]private TextMeshProUGUI _upgradeDescription;
    [SerializeField]private Image _upgradeIcon;
    //this 3 fields above will inherit the properties of the upgrade in question such as the description, the name and the sprite
    [SerializeField]private Button _craftUpgradeButton;
    [SerializeField]private RectTransform _costsContainer;
    [SerializeField]RecipeItemPrefab _recipeItemPrefab;
    const int MaxCostItems = 3;
    private UpgradeCost[] _upgradeCosts;
    RecipeItemPrefab[] _instantiatedCosts;

    //properties
    public Button CraftUpgradeButton => _craftUpgradeButton;

    public void Initialize(SOUpgradeBase upgrade, Action<SOUpgradeBase> onClick)
    {
        _upgradeName.text = upgrade.name;
        _upgradeDescription.text = upgrade.EffectDescription;
        _upgradeIcon.sprite = upgrade.Sprite;
        _upgradeCosts = upgrade.Costs;
        _craftUpgradeButton.RemoveAllEvents();
        _craftUpgradeButton.AddEventListener<SOUpgradeBase>(onClick, upgrade);
        //_craftingMaterials = upgrade.CraftingMaterials;
        if(_upgradeCosts == null || _upgradeCosts.Length == 0)
        {
            Debug.Log($"{upgrade}  is FREE");
            foreach(var item in _instantiatedCosts) item?.gameObject.SetActive(false);
            //make it so that no cost is deduced // ALSO add a way to indicate that it is free
            return;
        }
        CheckCostItems();
    }

    void CheckCostItems()
    {
        if(_instantiatedCosts == null) CreateCostItems();
        for (int i = 0; i < _upgradeCosts.Length; i++)
        {
            var item = _instantiatedCosts[i];
            if(_upgradeCosts[i].CraftingMaterial == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            item.gameObject.SetActive(true);
            item.Initialize(_upgradeCosts[i]);
        }
    }

    void CreateCostItems()
    {
        _instantiatedCosts = new RecipeItemPrefab[MaxCostItems];
        for (int i = 0; i < MaxCostItems; i++)
        {
            _instantiatedCosts[i] = Instantiate(_recipeItemPrefab, _costsContainer);
            _instantiatedCosts[i].gameObject.SetActive(false);
        }
    }

        
    
}
