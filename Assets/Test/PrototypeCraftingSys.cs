using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeCraftingSys : MonoBehaviour
{   
    [SerializeField] Transform _recipeItemsContainer;
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] PrototypeCraftingElement _craftingRecipePrefab;
    [SerializeField] SOUpgradeBase[] _upgrades;
    PrototypeCraftingElement[] _instantiatedItems;

    private void Start() {
        _playerInventory.onInventoryChange += UpdateOwnedMaterials;
        SetUpgrades();
    }

    public void SetUpgrades()
    {
        CreateRecipes(_upgrades.Length);
        for (int i = 0; i < _upgrades.Length; i++)
        {
            _instantiatedItems[i].Initialize(_upgrades[i], _playerInventory);
            _instantiatedItems[i].gameObject.SetActive(true);
        }
    }

    void CreateRecipes(int number)
    {
        //
        int childCount = _recipeItemsContainer.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(_recipeItemsContainer.GetChild(i).gameObject);
        }
        _instantiatedItems = new PrototypeCraftingElement[number];
        for (int i = 0; i < number; i++)
        {
            _instantiatedItems[i] = Instantiate(_craftingRecipePrefab, _recipeItemsContainer);
            _instantiatedItems[i].gameObject.SetActive(false);
        }

    }

    void UpdateOwnedMaterials()
    {
        for (int i = 0; i < _instantiatedItems.Length; i++)
        {
            var upgrade = _upgrades[i];
            var item = _instantiatedItems[i];
            item.UpdateCostItems(upgrade);
        }
    }

    private void OnDestroy() {
        _playerInventory.onInventoryChange -= UpdateOwnedMaterials;
    }
}
