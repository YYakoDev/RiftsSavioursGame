using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesMenu : MonoBehaviour
{
    [SerializeField]private RectTransform _upgradesMenu;
    [SerializeField]private RectTransform _upgradesContainer;
    [SerializeField]private UpgradeItemPrefab _upgradeItemPrefab;
    [SerializeField]SOPossibleUpgradesList _possibleUpgradesList; // from here you should grab an x number of upgrades and show them everytime you open the menu
    [SerializeField]SOPlayerInventory _playerInventory;
    private UpgradeGroup[] _selectedUpgrades;


    [Header("VFX & SFX")]
    [SerializeField]AudioSource _audio;
    [SerializeField]AudioClip _openingSound;
    [SerializeField]AudioClip _closingSound;



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
        DeactivateUpgradeMenu();

        
    }

    public void ActivateUpgradeMenu()
    {
        _upgradesMenu.gameObject.SetActive(true);
        _audio.PlayWithVaryingPitch(_openingSound);

        //Cursor stuff
        _previousCursorState = Cursor.visible;
        _previousCursorLockMode = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        TimeScaleManager.ForceTimeScale(0);
        
        //do the setup here and get the possible upgrades and show 3
        //every time the player gets an upgrade they are "popped" from the list
        //ALSO add to the button eventListener the CraftUpgrade method an pass as a parameter
    }

    public void CraftUpgrade(SOUpgradeBase upgradeToCraft) //this method is being called by the upgrade button on the canvas
    {
        //deduce the cost of the upgrade from the players inventory if the player does not have enough materials then dont do that
        DeactivateUpgradeMenu();
        //apply the effect of the upgrade here
    }

    public void DeactivateUpgradeMenu()
    {
        _audio.PlayWithVaryingPitch(_closingSound);
        _upgradesMenu.gameObject.SetActive(false);
        Cursor.visible = _previousCursorState;
        Cursor.lockState = _previousCursorLockMode;
        TimeScaleManager.ForceTimeScale(1);
    }


    private void OnDestroy() {
        PlayerLevelManager.onLevelUp -= ActivateUpgradeMenu;
    }

}
