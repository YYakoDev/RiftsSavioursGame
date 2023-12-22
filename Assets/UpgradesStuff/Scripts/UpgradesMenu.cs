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
    int _selectionCount = 3;
    private int[] _pickedIndexes;
    private SOUpgradeBase[] _selectedUpgrades;

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
        _pickedIndexes = new int[_selectionCount];
        _selectedUpgrades = new SOUpgradeBase[_selectionCount];
        if(_activeMenuOnStart)
        {
            ActivateUpgradeMenu();
        }
    }

    public void ActivateUpgradeMenu()
    {
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
        CheckSelectionAmount();
        List<UpgradeGroup> possibleUpgrades = new(_possibleUpgradesList.PossibleUpgrades);
        
        for (int i = 0; i < _selectionCount; i++)
        {
            _pickedIndexes[i] = Random.Range(0, _possibleUpgradesList.PossibleUpgrades.Length);
            _selectedUpgrades[i] = possibleUpgrades[_pickedIndexes[i]].GetNextUpgrade();
            //remove the selected one from the list so the next iteration will get something different
            possibleUpgrades.Remove(possibleUpgrades[i]);
        }
    }

    void CheckSelectionAmount()
    {
        if(_pickedIndexes.Length != _selectionCount) Array.Resize<int>(ref _pickedIndexes, _selectionCount);
        if(_selectedUpgrades.Length != _selectionCount) Array.Resize<SOUpgradeBase>(ref _selectedUpgrades, _selectionCount);
    }

    public void DeactivateUpgradeMenu()
    {
        _audio.PlayWithVaryingPitch(_closingSound);
        _animations.PlayCloseAnimations();
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;
        TimeScaleManager.ForceTimeScale(1);
    }


    private void OnDestroy() {
        PlayerLevelManager.onLevelUp -= ActivateUpgradeMenu;
    }

}
