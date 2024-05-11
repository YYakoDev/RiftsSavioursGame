using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemPrefab : MonoBehaviour, ISelectHandler
{
    [SerializeField] private RectTransform _itemsParent;
    [SerializeField] private TextMeshProUGUI _upgradeName;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private TextMeshProUGUI _upgradeDescriptionPrefab;
    [SerializeField] private Image _upgradeIcon;
    //this 3 fields above will inherit the properties of the upgrade in question such as the description, the name and the sprite
    [SerializeField] private Button _craftUpgradeButton;
    [SerializeField] private TextMeshProUGUI _btnText;
    [SerializeField] private RectTransform _costsContainer;
    [SerializeField] RecipeItemPrefab _recipeItemPrefab;
    SOPlayerInventory _inventory;
    const int MaxCostItems = 3;
    private UpgradeCost[] _upgradeCosts;
    RecipeItemPrefab[] _instantiatedCosts;

    //properties
    public RectTransform ItemsParent => _itemsParent;
    public TextMeshProUGUI Name => _upgradeName;
    public TextMeshProUGUI Description => _description;
    public Image Icon => _upgradeIcon;
    public Button CraftBtn => _craftUpgradeButton;
    public TextMeshProUGUI ButtonText => _btnText;
    public RectTransform CostContainer => _costsContainer;

    public void Initialize(SOPlayerInventory inventory, SOUpgradeBase upgrade, Action<SOUpgradeBase, int> onClick, int uiItemIndex)
    {
        _inventory = inventory;
        _upgradeName.text = upgrade.Name;
        _description.text = upgrade.Description;
        Debug.Log(upgrade.Description);
        _upgradeIcon.sprite = upgrade.Sprite;
        _upgradeCosts = upgrade.Costs;
        _craftUpgradeButton.RemoveAllEvents();
        _craftUpgradeButton.AddEventListener<SOUpgradeBase, int>(onClick, upgrade, uiItemIndex);
        SetButton(true);
        //_craftingMaterials = upgrade.CraftingMaterials;

        if (_upgradeCosts == null || _upgradeCosts.Length == 0)
        {
            Debug.Log($"{upgrade}  is FREE");
            if (_instantiatedCosts != null) foreach (var item in _instantiatedCosts) item?.gameObject?.SetActive(false);
            //make it so that no cost is deduced // ALSO add a way to indicate that it is free
            return;
        }
        CheckCostItems();

        SetTextMeshAutoSize(true);
        if(gameObject.activeInHierarchy)StartCoroutine(DisableAutoSize());
    }

    private void OnEnable()
    {
        SetTextMeshAutoSize(true);
        StartCoroutine(DisableAutoSize());
    }

    void CheckCostItems()
    {
        if (_instantiatedCosts == null) CreateCostItems();
        for (int i = 0; i < _upgradeCosts.Length; i++)
        {
            var item = _instantiatedCosts[i];
            var upgradeCost = _upgradeCosts[i];
            var craftingMat = upgradeCost.CraftingMaterial;
            if (craftingMat == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }
            item.gameObject.SetActive(true);

            InitializeRecipeItem(item, upgradeCost);
        }
        if (_upgradeCosts.Length < _instantiatedCosts.Length)
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
        if(_upgradeCosts == null || _upgradeCosts.Length <= 0) return;
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
        if (upgradeCost.Cost > materialCount)
        {
            SetButton(false);
        }
    }

    void SetButton(bool state)
    {
        if (state)
        {
            _craftUpgradeButton.interactable = true;
            _btnText.fontStyle = FontStyles.Normal;
            _btnText.fontStyle = FontStyles.Bold;
        }
        else
        {
            _craftUpgradeButton.interactable = false;
            _btnText.fontStyle = FontStyles.Normal;
            _btnText.fontStyle = FontStyles.Strikethrough;
        }
    }


    public void ChangeButtonEvent(Action<SOUpgradeBase, int> onClick, SOUpgradeBase upgrade, int index)
    {
        _craftUpgradeButton.RemoveAllEvents();
        _craftUpgradeButton.AddEventListener<SOUpgradeBase, int>(onClick, upgrade, index);
    }

    public void ChangeButtonText(string text)
    {
        _btnText.fontStyle = FontStyles.Normal;
        _btnText.text = text;
    }


    IEnumerator DisableAutoSize()
    {
        yield return null;
        yield return null;
        SetTextMeshAutoSize(false);
    }
    void SetTextMeshAutoSize(bool state)
    {
        _btnText.enableAutoSizing = state;
        _upgradeName.enableAutoSizing = state;
        _description.enableAutoSizing = state;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(SelectButton());
    }

    IEnumerator SelectButton()
    {
        yield return null;
        yield return null;
        yield return null;
        EventSystem.current.SetSelectedGameObject(_craftUpgradeButton.gameObject);
    }

    private void OnDestroy() {
        _craftUpgradeButton.RemoveAllEvents();
    }
}
