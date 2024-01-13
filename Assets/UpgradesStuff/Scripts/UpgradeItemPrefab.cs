using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemPrefab : MonoBehaviour
{
    [SerializeField]private RectTransform _itemsParent;
    [SerializeField]private TextMeshProUGUI _upgradeName;
    [SerializeField]private RectTransform _descriptionsParent;
    [SerializeField]private TextMeshProUGUI _upgradeDescriptionPrefab;
    [SerializeField]private Image _upgradeIcon;
    //this 3 fields above will inherit the properties of the upgrade in question such as the description, the name and the sprite
    [SerializeField]private Button _craftUpgradeButton;
    [SerializeField]private TextMeshProUGUI _btnText;
    [SerializeField]private RectTransform _costsContainer;
    [SerializeField]RecipeItemPrefab _recipeItemPrefab;
    SOPlayerInventory _inventory;
    const int MaxCostItems = 3;
    private UpgradeCost[] _upgradeCosts;
    RecipeItemPrefab[] _instantiatedCosts;
    TextMeshProUGUI[] _instantiatedDescriptions;

    //properties
    public RectTransform ItemsParent => _itemsParent;
    public TextMeshProUGUI Name => _upgradeName;
    //public TextMeshProUGUI Description => _upgradeDescription;
    public Image Icon => _upgradeIcon;
    public Button CraftBtn => _craftUpgradeButton;
    public TextMeshProUGUI ButtonText => _btnText;
    public RectTransform CostContainer => _costsContainer;

    public void Initialize(SOPlayerInventory inventory, SOUpgradeBase upgrade, Action<SOUpgradeBase, int> onClick, int uiItemIndex)
    {
        _inventory = inventory;
        _upgradeName.text = upgrade.Name;
        CheckDescriptionItems(upgrade.EffectDescriptions);
        _upgradeIcon.sprite = upgrade.Sprite;
        _upgradeCosts = upgrade.Costs;
        _craftUpgradeButton.RemoveAllEvents();
        _craftUpgradeButton.AddEventListener<SOUpgradeBase, int>(onClick, upgrade, uiItemIndex);
        SetButton(true);
        //_craftingMaterials = upgrade.CraftingMaterials;

        if(_upgradeCosts == null || _upgradeCosts.Length == 0)
        {
            Debug.Log($"{upgrade}  is FREE");
            if(_instantiatedCosts != null) foreach(var item in _instantiatedCosts) item?.gameObject?.SetActive(false);
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
            var upgradeCost = _upgradeCosts[i];
            var craftingMat = upgradeCost.CraftingMaterial;
            if(craftingMat == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            item.gameObject.SetActive(true);

            InitializeRecipeItem(item, upgradeCost);
        }
        if(_upgradeCosts.Length < _instantiatedCosts.Length)
        {
            for (int i = _instantiatedCosts.Length - 1; i >= _upgradeCosts.Length; i--)
            {
                _instantiatedCosts[i].gameObject.SetActive(false);
            }
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

    public void RecalculateCosts()
    {
        SetButton(true);
        for (int i = 0; i < _upgradeCosts.Length; i++)
        {
            var item = _instantiatedCosts[i];
            var upgradeCost = _upgradeCosts[i];
            InitializeRecipeItem(item, upgradeCost);

        }

    }

    void InitializeRecipeItem(RecipeItemPrefab item, UpgradeCost upgradeCost)
    {

        var craftingMat = upgradeCost.CraftingMaterial;
        var materialsInventory = _inventory.OwnedMaterials;
        int materialCount = (materialsInventory.ContainsKey(craftingMat)) ? materialsInventory[craftingMat] : 0;
        item.Initialize(upgradeCost, materialCount);
        if(upgradeCost.Cost > materialCount)
        {
            SetButton(false);
        }
    }

    void SetButton(bool state)
    {
        if(state)
        {
            _craftUpgradeButton.interactable = true;
            _btnText.fontStyle = FontStyles.Normal;
            _btnText.fontStyle = FontStyles.Bold;
        }else
        {
            _craftUpgradeButton.interactable = false;
            _btnText.fontStyle = FontStyles.Strikethrough;
        }
    }

    void CheckDescriptionItems(UpgradeDescription[] descriptions)
    {
        int count = descriptions.Length;
        if(_instantiatedDescriptions == null) CreateDescriptionItems();
        for (int i = 0; i < 5; i++)
        {
            var item = _instantiatedDescriptions[i];
            if(i >= count)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            var newDescription = descriptions[i];
            item.text = newDescription.Text;
            item.color = newDescription.Color;
            item.gameObject.SetActive(true);
        }
    }

    void CreateDescriptionItems()
    {
        _instantiatedDescriptions = new TextMeshProUGUI[5];
        for (int i = 0; i < 5; i++)
        {
            _instantiatedDescriptions[i] = Instantiate(_upgradeDescriptionPrefab, _descriptionsParent);
            _instantiatedDescriptions[i].gameObject.SetActive(false);
        }
    }


        
    
}
