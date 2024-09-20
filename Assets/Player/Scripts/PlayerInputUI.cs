using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputUI : MonoBehaviour
{
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] WeaponAiming _aiming;
    [SerializeField] PlayerMovement _movementScript;
    [SerializeField] Sprite _dashUIIcon, _atkUIIcon, _aimIcon;
    UISkill _dashSkill, _attackSkill, _switchAimSkill, _switchWeaponSkill;
    [SerializeField]InputActionReference _atkInput, _switchInput, _dashInput, _switchAimInput;
    WeaponBase _currentWeapon, _previousWeapon;

    private void Awake() {
        _weaponManager.OnWeaponChange += SetWeapon;
    }

    private IEnumerator Start() {
        yield return null;
        yield return null;
        SetAtkInputOnUI();
        SetSwitchWeaponOnUI();
        SetDashInputOnUI();
        SetSwitchAimOnUI();
        _movementScript.onDash += PlayDashInput;
        _aiming.OnAimingChange += PlayAimInput;
        _switchInput.action.performed += PlaySwitchWeaponInput;
        SwitchAttackIcons();
    }

    void SetWeapon(WeaponBase weapon)
    {
        if(_currentWeapon != null)
        {
            _currentWeapon.onAttack -= PlayAttackInput;
            _previousWeapon = _currentWeapon;
        }
        _currentWeapon = weapon;
        _currentWeapon.onAttack += PlayAttackInput;
        SwitchAttackIcons();
    }

    void SetInputOnUI(ref UISkill item, InputActionReference inputType, Sprite icon, float cooldown)
    {
        if(_uiSkillsManager == null) return;
        if(item == null)
        {
            item = _uiSkillsManager.SetInputSkill(inputType, icon, cooldown);
        }
        else item.UpdateCooldown(cooldown);
    }
    void SetInputOnUI(ref UISkill item, InputActionReference inputType, Sprite icon)
    {
        if(_uiSkillsManager == null) return;
        if(item == null)
        {
            item = _uiSkillsManager.SetInputSkill(inputType, icon);
        }
    }

    void SetSwitchWeaponOnUI()
    {
        SetInputOnUI(ref _switchWeaponSkill, _switchInput, _atkUIIcon);
    }

    void SetDashInputOnUI()
    {
        SetInputOnUI(ref _dashSkill, _dashInput, _dashUIIcon, _movementScript.DashCooldown);
    }
    void SetAtkInputOnUI()
    {
        SetInputOnUI(ref _attackSkill, _atkInput, _atkUIIcon);
    }
    void SetSwitchAimOnUI()
    {
        SetInputOnUI(ref _switchAimSkill, _switchAimInput, _aimIcon);
    }

    void PlayAttackInput()
    {
        //_attackSkill?.UpdateCooldown(_currentWeapon.GetWeaponCooldown());
        _attackSkill?.Interact();
    }
    void PlaySwitchWeaponInput(InputAction.CallbackContext obj)
    {
        _switchWeaponSkill?.Interact();
    }

    void SwitchAttackIcons()
    {
        if(_currentWeapon == null)
        {
            _attackSkill?.UpdateSkillIcon(_atkUIIcon);
        }else
        {
            WeaponBase otherWeapon = null;
            for (int i = 0; i < _playerStats.Weapons.Length; i++)
            {
                var weapon = _playerStats.Weapons[i];
                if(weapon != null && weapon != _currentWeapon)
                {
                    otherWeapon = weapon;
                    break;
                }
            }
            if(otherWeapon == null)
            {
                _attackSkill?.UpdateSkillIcon(_currentWeapon.SpriteAndAnimationData.Sprite);   
                _switchWeaponSkill?.UpdateSkillIcon(_atkUIIcon);
            }else
            {
                _attackSkill?.UpdateSkillIcon(_currentWeapon.SpriteAndAnimationData.Sprite);   
                _switchWeaponSkill?.UpdateSkillIcon(otherWeapon.SpriteAndAnimationData.Sprite);
            }
        }
    }

    void PlayDashInput()
    {
        _dashSkill?.UpdateCooldown(_movementScript.DashCooldown);
        _dashSkill?.Interact();
    }

    void PlayAimInput(bool state)
    {
        _switchAimSkill?.Interact();
    }
    private void OnDestroy() {
        _weaponManager.OnWeaponChange -= SetWeapon;
        _switchInput.action.performed -= PlaySwitchWeaponInput;
        _movementScript.onDash -= PlayDashInput;
        _aiming.OnAimingChange -= PlayAimInput;
    }

}
