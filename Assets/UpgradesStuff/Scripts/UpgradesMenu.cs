using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LevelUpSequence))]
public class UpgradesMenu : MonoBehaviour
{
    [SerializeField]private RectTransform _upgradesMenu;
    [SerializeField]private UpgradeUILayout _upgradesContainer;
    [SerializeField]private Button _continueButton;
    [SerializeField]private UpgradeMenuAnimations _animations;
    [SerializeField]private UpgradeItemPrefab _upgradeItemPrefab;
    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList; // from here you should grab an x number of upgrades and show them everytime you open the menu
    [SerializeField]SOPlayerInventory _playerInventory;
    LevelUpSequence _levelupSequence;
    const int selectionCount = 3;
    [SerializeField]int _choicesAmount = 3;
    [SerializeField,ReadOnly]int _craftingAttempts = 0;
    private SOUpgradeBase[] _selectedUpgrades;
    private UpgradeItemPrefab[] _instantiatedItems;

    [SerializeField]bool _activeMenuOnStart = false;
    public event Action OnMenuClose;


    [Header("VFX & SFX")]
    //[SerializeField]AudioSource _audio;
    //[SerializeField]AudioClip _openingSound, _closingSound;

    //cursor stuff
    bool _previousCursorState;
    CursorLockMode _previousCursorLockMode;


    private void Awake() {
        _levelupSequence = GetComponent<LevelUpSequence>();
    }

    private void Start() {
        if(_playerInventory == null || _possibleUpgradesList == null)
        {
            Debug.LogError("A ScriptableObject reference is not set on the UpgradesMenu");
            DeactivateUpgradeMenu();
            this.enabled = false;
        }
        //PlayerLevelManager.onLevelUp += ActivateUpgradeMenu;
        _selectedUpgrades = new SOUpgradeBase[selectionCount];
        CreateUpgradeItems();
        if(_activeMenuOnStart) PlayMenuAnimations();
        else _upgradesMenu.gameObject.SetActive(false);
    }

    public void DoLevelUpSequence()
    {
        YYInputManager.StopInput();
        _upgradesContainer.SetStopTimer(4f);
        _levelupSequence.Play(PlayMenuAnimations);
    }

    void PlayMenuAnimations()
    {
        _animations.SetElements(_instantiatedItems);
        _upgradesMenu.gameObject.SetActive(true);
        _animations.Play(ActivateUpgradeMenu);
    }

    void ActivateUpgradeMenu()
    {
        TimeScaleManager.ForceTimeScale(0);
        _upgradesContainer.ResumeInput();
        _craftingAttempts = 0;
        PickRandomUpgrades();
        CheckCreatedItems();

        //_animations.ClearAnimations();
        //_animations.PlayAnimations();
        //_audio.PlayWithVaryingPitch(_openingSound);
        
        //Cursor stuff
        _previousCursorState = Cursor.visible;
        _previousCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        
        //do the setup here and get the possible upgrades and show 3
        //every time the player gets an upgrade they are "popped" from the list
        //ALSO add to the button eventListener the CraftUpgrade method and pass as a parameter the actual upgrade that you generated
    }

    void PickRandomUpgrades()
    {
        List<UpgradeGroup> possibleUpgrades = new(_possibleUpgradesList.PossibleUpgrades);
        //foreach(UpgradeGroup group in possibleUpgrades) Debug.Log(group.name);
        //if possibleupgrades.count is equal to zero then you shouldnt open the menu at all or maybe add bundle items or health replenish upgrades
        if(possibleUpgrades.Count < selectionCount)
        {
            for (int i = selectionCount-1; i >= possibleUpgrades.Count; i--)
            {
                _selectedUpgrades[i] = null;
            }
        }
        for (int i = 0; i < selectionCount; i++)
        {
            if(possibleUpgrades.Count == 0) break;
            //if(i >= _possibleUpgradesList.PossibleUpgrades.Count) break;
            
            int index = Random.Range(0, possibleUpgrades.Count);
            _selectedUpgrades[i] = possibleUpgrades[index].GetUpgrade();
            //remove the selected one from the list so the next iteration will get something different
            possibleUpgrades.Remove(possibleUpgrades[index]);
        }
    }

    void CheckCreatedItems()
    {
        if(_instantiatedItems == null) CreateUpgradeItems();
        bool playerHasEnoughMaterials = false;
        for (int i = 0; i < _instantiatedItems.Length; i++)
        {
            _instantiatedItems[i].gameObject.SetActive(true);
            if (_selectedUpgrades[i] == null)
            {
                //Debug.Log($"Selected upgrade at index: '{i}' is null");
                _instantiatedItems[i].gameObject.SetActive(false);
                continue;
            }
            _instantiatedItems[i].Initialize(_playerInventory, _selectedUpgrades[i], CraftUpgrade, i);
            if (_instantiatedItems[i].CraftBtn.interactable) playerHasEnoughMaterials = true;
        }
        if(!playerHasEnoughMaterials) ChangeFocusToContinueButton();
    }
    public void CraftUpgrade(SOUpgradeBase upgradeToCraft, int uiItemIndex) //this method is being called by the upgrade button on the canvas
    {
        _craftingAttempts++;
        //_audio.PlayWithVaryingPitch(_closingSound);
        foreach (UpgradeCost requirement in upgradeToCraft.Costs)
        {
            if (requirement.Cost <= 0) continue;
            _playerInventory.RemoveMaterial(requirement.CraftingMaterial, requirement.Cost);
        }
        _playerInventory.AddUpgrade(upgradeToCraft);
        RecalculateCosts();
        if (_craftingAttempts >= _choicesAmount) DeactivateUpgradeMenu();
        else
        {
            //remove selected upgrade and generate another one
            RepickUpgradeItem(upgradeToCraft, uiItemIndex);
        }
    }
    void RepickUpgradeItem(SOUpgradeBase currentUpgrade, int index)
    {
        List<UpgradeGroup> list = new(_possibleUpgradesList.PossibleUpgrades);
        foreach (SOUpgradeBase upgrade in _selectedUpgrades)
        {
            if(upgrade == null || upgrade.GroupParent == null)
            {
                //Debug.Log("Other Upgrade or other Upgrade Parent is null");
                continue;
            }
            if(upgrade.GroupParent == currentUpgrade.GroupParent) continue;
            list.Remove(upgrade.GroupParent);
        }
        if(list.Count == 0)
        {
            //Debug.Log("List count is equal to 0");
            _instantiatedItems[index].gameObject.SetActive(false);
            return;
        }
        int randomIndex = Random.Range(0, list.Count);
        SOUpgradeBase randomUpgrade = list[randomIndex].GetUpgrade();
        if(randomUpgrade == null)
        {
            _instantiatedItems[index].gameObject.SetActive(false);
            return;
        }
        _selectedUpgrades[index] = randomUpgrade;
        _instantiatedItems[index].Initialize(_playerInventory, randomUpgrade, CraftUpgrade, index);
    }

    void RecalculateCosts()
    {
        if (_instantiatedItems == null) return;

        foreach (var item in _instantiatedItems)
        {
            item.RecalculateCosts();

        }
    }

    void ChangeFocusToContinueButton()
    {
        EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);
    }

    void CreateUpgradeItems()
    {
        Transform cachedTransform = _upgradesContainer.transform;
        _instantiatedItems = new UpgradeItemPrefab[selectionCount];
        for (int i = 0; i < selectionCount; i++)
        {
            _instantiatedItems[i] = Instantiate(_upgradeItemPrefab, cachedTransform);
            _instantiatedItems[i].gameObject.SetActive(true);
        }
        _upgradesContainer.SetElements(_instantiatedItems);
    }

    public void DeactivateUpgradeMenu()
    {
        //when closing you should check if you have another levelup in the queue so you can do the multiple level up
        //_audio.PlayWithVaryingPitch(_closingSound);
        //_animations.PlayCloseAnimations(Resume);
        _upgradesMenu.gameObject.SetActive(false);
        _levelupSequence.DisableVisuals();
        Resume();
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;
        OnMenuClose?.Invoke();
        YYInputManager.ResumeInput();
    }

    void Resume()
    {
        TimeScaleManager.ForceTimeScale(1);
    }


    private void OnDestroy()
    {
        //PlayerLevelManager.onLevelUp -= ActivateUpgradeMenu;
    }

}
