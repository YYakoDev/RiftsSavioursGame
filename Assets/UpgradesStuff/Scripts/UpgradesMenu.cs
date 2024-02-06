using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Random = UnityEngine.Random;

[RequireComponent(typeof(LevelUpSequence))]
public class UpgradesMenu : MonoBehaviour
{
    [SerializeField]private RectTransform _upgradesMenu;
    [SerializeField]private UpgradeUILayout _upgradesContainer;
    [SerializeField]private Button _continueButton, _rerollButton, _saveButton;
    [SerializeField]private UpgradeMenuAnimations _animations;
    [SerializeField]private UpgradeItemPrefab _upgradeItemPrefab;
    [SerializeField] private TextMeshProUGUI t_uiChoicesText;
    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList; // from here you should grab an x number of upgrades and show them everytime you open the menu
    [SerializeField]SOPlayerInventory _playerInventory;
    [SerializeField] SavedUpgradesInventory _savedUpgradesInventory;
    LevelUpSequence _levelupSequence;
    const int selectionCount = 3;
    int _choicesAmount = 1;
    [SerializeField,ReadOnly]int _craftingAttempts = 0;
    private SOUpgradeBase[] _selectedUpgrades;
    private UpgradeItemPrefab[] _instantiatedItems;

    [SerializeField]bool _activeMenuOnStart = false;
    public event Action OnMenuClose;
    EventSystem _eventSys;
    private SOUpgradeBase[] _savedUpgrades = new SOUpgradeBase[0];

    bool _isSavingUpgrades = false;


    public int CraftingAttempts
    {
        get => _craftingAttempts;
        set
        {
            _craftingAttempts = value;
            SetChoicesAmountOnUI();
            CheckRerollButton();
            if(value >= _choicesAmount) DeactivateUpgradeMenu();
        }
    }


    [Header("VFX & SFX")]
    [SerializeField]AudioSource _audio;
    [SerializeField]AudioClip _closingSound, _rerollSFX, _saveSFX, _craftSfx;

    //cursor stuff
    bool _previousCursorState;
    CursorLockMode _previousCursorLockMode;

    //time stuff
    bool _previousTimeScaleState;


    private void Awake() {
        _audio = GetComponent<AudioSource>();
        _levelupSequence = GetComponent<LevelUpSequence>();
        _choicesAmount = 1;
    }

    private void Start() {
        if(_playerInventory == null || _possibleUpgradesList == null)
        {
            Debug.LogError("A ScriptableObject reference is not set on the UpgradesMenu");
            DeactivateUpgradeMenu();
            this.enabled = false;
        }
        PlayerLevelManager.onLevelUp += AddMoreChoices;
        _selectedUpgrades = new SOUpgradeBase[selectionCount];
        CreateUpgradeItems();
        if(_activeMenuOnStart) PlayMenuAnimations();
        else _upgradesMenu.gameObject.SetActive(false);
        _eventSys = EventSystem.current;
    }

    public void DoLevelUpSequence()
    {
        YYInputManager.StopInput();
        _upgradesContainer.SetStopTimer(3f);
        _previousTimeScaleState = TimeScaleManager.IsForced;
        TimeScaleManager.ForceTimeScale(0);
        _upgradesMenu.localScale = Vector3.one;
        _levelupSequence.Play(PlayMenuAnimations);
    }

    void PlayMenuAnimations()
    {
        CraftingAttempts = 0;
        _animations.SetElements(_instantiatedItems);
        _upgradesMenu.gameObject.SetActive(true);
        _animations.Play(ActivateUpgradeMenu);
    }

    void AddMoreChoices() => _choicesAmount += 2;

    void SetChoicesAmountOnUI() => t_uiChoicesText.text = Mathf.Abs(_choicesAmount - CraftingAttempts).ToString();


    void ActivateUpgradeMenu()
    {
        _upgradesContainer.ResumeInput();
        PickRandomUpgrades();
        CheckCreatedItems();
        //Cursor stuff
        _previousCursorState = Cursor.visible;
        _previousCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void PickRandomUpgrades()
    {
        List<UpgradeGroup> possibleUpgrades = new(_possibleUpgradesList.PossibleUpgrades);
        foreach(var upgrade in _savedUpgrades)
        {
            if(upgrade == null) continue;
            possibleUpgrades.Remove(upgrade.GroupParent);
        }
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
        if(!playerHasEnoughMaterials) Select(_continueButton.gameObject);
    }
    public void CraftUpgrade(SOUpgradeBase upgradeToCraft, int uiItemIndex) //this method is being called by the upgrade button on the canvas
    {
        CraftingAttempts++;
        //_audio.PlayWithVaryingPitch(_closingSound);
        foreach (UpgradeCost requirement in upgradeToCraft.Costs)
        {
            if (requirement.Cost <= 0) continue;
            _playerInventory.RemoveMaterial(requirement.CraftingMaterial, requirement.Cost);
        }
        _playerInventory.AddUpgrade(upgradeToCraft);
        RecalculateCosts();
        PlaySound(_craftSfx);
        if (CraftingAttempts >= _choicesAmount) DeactivateUpgradeMenu();
        else
        {
            //remove selected upgrade and generate another one
            RepickUpgradeItem(upgradeToCraft, uiItemIndex);
        }
    }
    void RepickUpgradeItem(SOUpgradeBase currentUpgrade, int index)
    {
        List<UpgradeGroup> list = new(_possibleUpgradesList.PossibleUpgrades);
        for (int i = 0; i < _selectedUpgrades.Length; i++)
        {
            var selectedUpgrade = _selectedUpgrades[i];
            var savedUpgrade = _savedUpgrades[i];
            if (selectedUpgrade == null || selectedUpgrade.GroupParent == null 
            || savedUpgrade == null || savedUpgrade.GroupParent == null)
            {
                //Debug.Log("Other Upgrade or other Upgrade Parent is null");
                continue;
            }
            if (selectedUpgrade.GroupParent == currentUpgrade.GroupParent) continue;
            list.Remove(selectedUpgrade.GroupParent);
            list.Remove(savedUpgrade.GroupParent);
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

    public void RerollUpgrades()
    {

        CraftingAttempts++;
        PickRandomUpgrades();
        PlaySound(_rerollSFX);
        CheckCreatedItems();
    }

    void CheckRerollButton()
    {
        _rerollButton.interactable = (_choicesAmount - CraftingAttempts) > 1;
    }

    public void OpenSavedUpgrades()
    {
        _savedUpgradesInventory.Initialize(_savedUpgrades, _playerInventory, CraftUpgrade, _instantiatedItems.Length);
        _savedUpgradesInventory.Open();
    }

    public void StartToSaveUpgrades()
    {
        _isSavingUpgrades = !_isSavingUpgrades;
        if(_isSavingUpgrades) SetSaveMode();
        else ReturnButtonsToNormal();
    }

    void SetSaveMode()
    {
        for (int i = 0; i < _instantiatedItems.Length; i++)
        {
            var item = _instantiatedItems[i];
            var upgrade = _selectedUpgrades[i];
            item.CraftBtn.interactable = true;
            item.ChangeButtonText("SAVE");
            item.ChangeButtonEvent(SaveUpgrade, upgrade, i);
        }
        Select(_instantiatedItems[0].gameObject);

    }

    void ReturnButtonsToNormal()
    {
        _isSavingUpgrades = false;
        for (int i = 0; i < _instantiatedItems.Length; i++)
        {
            var item = _instantiatedItems[i];
            var upgrade = _selectedUpgrades[i];
            item.ChangeButtonEvent(CraftUpgrade, upgrade, i);
            item.ChangeButtonText("CRAFT");
        }
        RecalculateCosts();
    }
    void SaveUpgrade(SOUpgradeBase upgrade, int index)
    {

        if(_savedUpgrades.Length != 3) Array.Resize<SOUpgradeBase>(ref _savedUpgrades, 3);
        bool allSlotsOcuppied = true;
        for (int i = 0; i < _savedUpgrades.Length; i++)
        {
            if(_savedUpgrades[i] == null)
            {
                allSlotsOcuppied = false;
                _savedUpgrades[i] = upgrade;
                break;
            }
        }
        if(allSlotsOcuppied)
        {
            ReturnButtonsToNormal();
            return;
        }
        _instantiatedItems[index].gameObject.SetActive(false);
        CraftingAttempts++;
        ReturnButtonsToNormal();
    }

    void RecalculateCosts()
    {
        if (_instantiatedItems == null) return;
        foreach (var item in _instantiatedItems)
        {
            item.RecalculateCosts();

        }
    }

    void Select(GameObject selectObj) => _upgradesContainer.SwitchFocus(selectObj);
    

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

    void PlaySound(AudioClip clip)
    {
        if(_audio == null) return;
        _audio.PlayWithVaryingPitch(clip);
    }

    public void DeactivateUpgradeMenu()
    {
        //when closing you should check if you have another levelup in the queue so you can do the multiple level up
        //_audio.PlayWithVaryingPitch(_closingSound);
        //_animations.PlayCloseAnimations(Resume);
        PlaySound(_closingSound);
        _animations.PlayCloseAnimation(Resume);
        _levelupSequence.DisableVisuals();
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;

        YYInputManager.ResumeInput();
    }

    void Resume()
    {
        float timeScale = (_previousTimeScaleState) ? 0f : 1f;
        _upgradesMenu.gameObject.SetActive(false);
        TimeScaleManager.SetTimeScale(1f);
        TimeScaleManager.ForceTimeScale(timeScale);
        OnMenuClose?.Invoke();
    }


    private void OnDestroy()
    {
        PlayerLevelManager.onLevelUp -= AddMoreChoices;
    }

}
