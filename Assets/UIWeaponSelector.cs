using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIWeaponSelector : MonoBehaviour
{
    bool _currentlySelecting = false;
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] PlayerInputController _inputController;
    [SerializeField] MenuController _menuController;
    [SerializeField] InputActionReference _uiEscape;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] UIWeaponSelectionItem _weaponToSwapItemUI;
    [SerializeField] UIWeaponSelectionItem[] _swappeableWeaponsUIItems;
    [SerializeField]GameObject _menuVisuals;
    WeaponSwapper _swapper;
    WeaponBase _weaponToSwap;

    private void Start() {
        _uiEscape.action.performed += CloseMenuWithInput;
        if(_menuVisuals.activeInHierarchy) OpenMenu(null, null);
    }

    public void OpenMenu(WeaponBase weaponToSwap, WeaponSwapper swapper)
    {
        if(_currentlySelecting) return;
        _currentlySelecting = true;
        _weaponToSwap = weaponToSwap;
        _swapper = swapper;
        _menuVisuals.SetActive(true);
        _menuController.SwitchCurrentMenu(_menuVisuals);
        PauseMenuManager.DisablePauseBehaviour(true);
        TimeScaleManager.ForceTimeScale(0f);
        _inputController.ChangeInputToUI();
        _text.SetText($"Select weapon to swap the {weaponToSwap?.WeaponName} to");
        _weaponToSwapItemUI.Init(weaponToSwap, null);
        for (int i = 0; i < _swappeableWeaponsUIItems.Length; i++)
        {
            if(i >= _playerStats.Weapons.Length) break;
            _swappeableWeaponsUIItems[i].Init(_playerStats.Weapons[i], SelectWeapon, i);
        }
    }

    public void SelectWeapon(int index)
    {
        Debug.Log("Selected:   " + _weaponToSwap.WeaponName + " at  " + index + "  index");
        var weaponToLeave = _playerStats.Weapons[index];
        if(!_weaponManager.SetWeaponAtIndex(index, _weaponToSwap))
        {
            CloseMenu();
            return;
        }
        _swapper.SetWeapon(weaponToLeave);
        _playerStats.Weapons[index] = _weaponToSwap;
        CloseMenu();
    }
    void CloseMenuWithInput(InputAction.CallbackContext obj)
    {
        CloseMenu();
    }

    public void CloseMenu()
    {
        _currentlySelecting = false;
        _menuVisuals.SetActive(false);
        _menuController.SwitchCurrentMenu(null);
        PauseMenuManager.DisablePauseBehaviour(false);
        TimeScaleManager.ForceTimeScale(1f);
        _inputController.ChangeInputToGameplay();
    }

    private void OnDestroy() {
        _uiEscape.action.performed -= CloseMenuWithInput;
    }
}
