using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PrototypeCraftingSystem : MonoBehaviour
{
    [SerializeField] GameObject _menuParent;
    [SerializeField] SOPlayerInventory _playerInventory;
    [SerializeField] SOUpgradeBase[] _knownRecipes;
    [SerializeField] PrototypeCraftingIngredient _craftingIngredientPrefab;
    [SerializeField] PrototypeCraftingRecipe _recipeButtonPrefab;
    [SerializeField] PrototypeResultPrefab _resultIconPrefab;
    [SerializeField] Transform _ingredientsParent, _knownRecipesParent;
    [SerializeField] Button _requestButton;
    [SerializeField] float _craftingCooldown = 1f;
    float _nextCraftingTime;
    bool _isMenuOpen, _timeScaleWasForced;
    PrototypeCraftingIngredient[] _ingredientsInstance;
    PrototypeResultPrefab _result;
    PrototypeCraftingRecipe[] _recipes;
    PrototypeCraftingRecipe _currentRecipe;
    EventSystem _eventSys;
    void Start()
    {
        _requestButton.AddEventListener(RequestUpgrade);
        _eventSys = EventSystem.current;
        //OpenMenu();
    }

    private void Update() {
        if(Input.GetButtonDown("Crafting"))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        if(_isMenuOpen) CloseMenu();
        else OpenMenu();
    }

    public void OpenMenu()
    {
        _isMenuOpen = true;

        _timeScaleWasForced = TimeScaleManager.IsForced;
        TimeScaleManager.ForceTimeScale(0f);
        YYInputManager.StopInput();
        _menuParent.SetActive(true);
        SetIngredients();
        SetRecipes();
    }

    public void CloseMenu()
    {
        if(_timeScaleWasForced) TimeScaleManager.SetTimeScale(1f);
        else TimeScaleManager.ForceTimeScale(1f);
        YYInputManager.ResumeInput();
        _menuParent.SetActive(false);
        _isMenuOpen = false;
    }

    void SetIngredients()
    {
        if(_ingredientsInstance == null) CreateIngredients();
    }

    void CreateIngredients()
    {
        _ingredientsInstance = new PrototypeCraftingIngredient[3];
        for (int i = 0; i < 3; i++)
        {
            _ingredientsInstance[i] = Instantiate(_craftingIngredientPrefab);
            _ingredientsInstance[i].Initialize(null, 0);
            _ingredientsInstance[i].transform.SetParent(_ingredientsParent, false);
        }
        _result = Instantiate(_resultIconPrefab);
        _result.InitializeResult(string.Empty, null);
        _result.transform.SetParent(_ingredientsParent, false);
    }

    void SetRecipes()
    {
        if(_recipes == null) CreateRecipes();
        _eventSys.SetSelectedGameObject(_recipes[0].gameObject);
    }

    void CreateRecipes()
    {
        _recipes = new PrototypeCraftingRecipe[_knownRecipes.Length];        
        for (int i = 0; i < _knownRecipes.Length; i++)
        {
            _recipes[i] = Instantiate(_recipeButtonPrefab);
            _recipes[i].InitializeRecipe(_knownRecipes[i], SelectRequestButton, FillRecipe, i);
            _recipes[i].transform.SetParent(_knownRecipesParent, false);
        }
    }

    void SelectRequestButton()
    {
        if(_requestButton.interactable)
            _eventSys.SetSelectedGameObject(_requestButton.gameObject);
    }

    void RequestUpgrade()
    {
        if(_currentRecipe == null) return;
        //if(!CheckAffordability(_currentRecipe.Upgrade)) return;
        Debug.Log("Crafting:   " + _currentRecipe.Upgrade + "Upgrade");
        foreach(var requirement in _currentRecipe.Upgrade.Costs)
        {
            _playerInventory.RemoveMaterial(requirement.CraftingMaterial, requirement.Cost);
        }
        _playerInventory.AddUpgrade(_currentRecipe.Upgrade);
        //CloseMenu();
    }

    bool CheckAffordability(SOUpgradeBase upgrade)
    {
        bool affordable = true;
        for (int i = 0; i < upgrade.Costs.Length; i++)
        {
            var requirement = upgrade.Costs[i];
            if(requirement.Cost > _playerInventory.GetMaterialOwnedCount(requirement.CraftingMaterial))
            {
                affordable = false;
                break;
            }
        }
        return affordable;
    }

    void FillRecipe(SOUpgradeBase upgrade, int recipeIndex)
    {
        for (int i = 0; i < _ingredientsInstance.Length; i++)
        {
            if(i >= upgrade.Costs.Length)
            {
                //_ingredientsInstance[i].gameObject.SetActive(false);
                _ingredientsInstance[i].Initialize(null, 0);
                continue;
            }
            //_ingredientsInstance[i].gameObject.SetActive(true);
            var craftingMaterial = upgrade.Costs[i].CraftingMaterial;
            var ownedAmount = _playerInventory.GetMaterialOwnedCount(craftingMaterial);
            _ingredientsInstance[i].Initialize(upgrade.Costs[i], ownedAmount);
        }
        
        _result.InitializeResult(upgrade.Name, upgrade.Sprite);
        _currentRecipe = _recipes[recipeIndex];
        _requestButton.interactable = (CheckAffordability(_currentRecipe.Upgrade));
    }

    private void OnDestroy() {
        _requestButton.RemoveAllEvents();
    }
}
