using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradesMenu : MonoBehaviour
{
    [SerializeField]private RectTransform _upgradesMenu;
    [SerializeField]private RectTransform _upgradesContainer;
    [SerializeField]private UpgradeMenuAnimations _animations;
    [SerializeField]private UpgradeItemPrefab _upgradeItemPrefab;
    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList; // from here you should grab an x number of upgrades and show them everytime you open the menu
    [SerializeField]SOPlayerInventory _playerInventory;
    const int selectionCount = 3;
    int _choicesAmount = 1;
    bool _isMenuActive = false;
    private SOUpgradeBase[] _selectedUpgrades;
    private UpgradeItemPrefab[] _instantiatedItems;

    [SerializeField]bool _activeMenuOnStart = false;

    [Header("VFX & SFX")]
    [SerializeField]AudioSource _audio;
    [SerializeField]AudioClip _openingSound, _closingSound;



    //cursor stuff
    bool _previousCursorState;
    CursorLockMode _previousCursorLockMode;

    private void Start() {
        if(_playerInventory == null || _possibleUpgradesList == null)
        {
            Debug.LogError("A ScriptableObject reference is not set on the UpgradesMenu");
            DeactivateUpgradeMenu();
            this.enabled = false;
        }
        PlayerLevelManager.onLevelUp += ActivateUpgradeMenu;
        _selectedUpgrades = new SOUpgradeBase[selectionCount];
        CreateUpgradeItems();
        if(_activeMenuOnStart)
        {
            //CreateUpgradeItems();
            ActivateUpgradeMenu();
        }
    }

    public void ActivateUpgradeMenu()
    {
        if(_isMenuActive)
        {
            Debug.Log("UPGRADE MENU IS ALREADY ACTIVE, queueing another level up");
            //do the queue here mf
            return;
        }
        PickRandomUpgrades();
        CheckCreatedItems();
        _isMenuActive = true;
        _upgradesMenu.gameObject.SetActive(true);
        _animations.ClearAnimations();
        _animations.PlayAnimations();
        _audio.PlayWithVaryingPitch(_openingSound);
        //Cursor stuff
        _previousCursorState = Cursor.visible;
        _previousCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        TimeScaleManager.ForceTimeScale(0);
        
        //do the setup here and get the possible upgrades and show 3
        //every time the player gets an upgrade they are "popped" from the list
        //ALSO add to the button eventListener the CraftUpgrade method and pass as a parameter the actual upgrade that you generated
    }

    public void CraftUpgrade(SOUpgradeBase upgradeToCraft) //this method is being called by the upgrade button on the canvas
    {
        //deduce the cost of the upgrade from the players inventory if the player does not have enough materials then dont do that
        DeactivateUpgradeMenu();
        //apply the effect of the upgrade here
    }

    void PickRandomUpgrades()
    {
        List<UpgradeGroup> possibleUpgrades = new(_possibleUpgradesList.PossibleUpgrades);
        for (int i = 0; i < selectionCount; i++)
        {
            if(possibleUpgrades.Count == 0) break;
            int index = Random.Range(0, possibleUpgrades.Count);
            _selectedUpgrades[i] = possibleUpgrades[index].GetUpgrade();
            //remove the selected one from the list so the next iteration will get something different
            possibleUpgrades.Remove(possibleUpgrades[index]);
            if(_selectedUpgrades[i] == null) _selectedUpgrades[i] = possibleUpgrades[index].GetUpgrade();
        }
    }

    void CheckCreatedItems()
    {
        if(_instantiatedItems == null) CreateUpgradeItems();
        for (int i = 0; i < _instantiatedItems.Length; i++)
        {
            if (_selectedUpgrades[i] == null)
            {
                _instantiatedItems[i].gameObject.SetActive(false);
                continue;
            }
            _instantiatedItems[i].Initialize(_selectedUpgrades[i], CraftUpgrade);
        }
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
    }

    public void DeactivateUpgradeMenu()
    {
        _audio.PlayWithVaryingPitch(_closingSound);
        _animations.PlayCloseAnimations(Resume);
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;
        
    }
    
    void Resume()
    {
        TimeScaleManager.ForceTimeScale(1);
        _isMenuActive = false;
    }


    private void OnDestroy() {
        PlayerLevelManager.onLevelUp -= ActivateUpgradeMenu;
    }

}
