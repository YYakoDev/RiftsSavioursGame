using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputUI : MonoBehaviour
{
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] WeaponAiming _aiming;
    [SerializeField] PlayerMovement _movementScript;
    [SerializeField] Sprite _dashUIIcon, _atkUIIcon, _aimIcon;
    UISkill _dashSkill, _attackSkill, _switchAimSkill;
    [SerializeField]InputActionReference _atkInput, _dashInput, _switchAimInput;
    WeaponBase _currentWeapon;


    private void Awake() {
        _weaponManager.OnWeaponChange += SetWeapon;
    }

    private IEnumerator Start() {
        yield return null;
        yield return null;
        SetAtkInputOnUI();
        SetDashInputOnUI();
        SetSwitchAimOnUI();
        _movementScript.onDash += PlayDashInput;
        _aiming.OnAimingChange += PlayAimInput;
    }

    void SetWeapon(WeaponBase weapon)
    {
        if(_currentWeapon != null)
        {
            _currentWeapon.onAttack -= PlayAttackInput;
        }
        _currentWeapon = weapon;
        _currentWeapon.onAttack += PlayAttackInput;
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
        _movementScript.onDash -= PlayDashInput;
        _aiming.OnAimingChange -= PlayAimInput;
    }

}
