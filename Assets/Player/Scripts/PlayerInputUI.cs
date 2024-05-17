using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputUI : MonoBehaviour
{
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] WeaponAiming _aiming;
    [SerializeField] PlayerMovement _movementScript;
    [SerializeField] Sprite _dashUIIcon, _atkUIIcon, _aimIcon;
    UISkill _dashSkill, _attackSkill, _switchAimSkill;
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

    void SetInputOnUI(ref UISkill item, KeyInputTypes type, Sprite icon, float cooldown)
    {
        if(_uiSkillsManager == null) return;
        if(item == null)
        {
            item = _uiSkillsManager.SetInputSkill(type, icon, cooldown);
        }
        else item.UpdateCooldown(cooldown);
    }
    void SetInputOnUI(ref UISkill item, KeyInputTypes type, Sprite icon)
    {
        if(_uiSkillsManager == null) return;
        if(item == null)
        {
            item = _uiSkillsManager.SetInputSkill(type, icon);
        }
    }


    void SetDashInputOnUI()
    {
        SetInputOnUI(ref _dashSkill, KeyInputTypes.Dash, _dashUIIcon, _movementScript.DashCooldown);
    }
    void SetAtkInputOnUI()
    {
        SetInputOnUI(ref _attackSkill, KeyInputTypes.Attack, _atkUIIcon);
    }
    void SetSwitchAimOnUI()
    {
        SetInputOnUI(ref _switchAimSkill, KeyInputTypes.SwitchAim, _aimIcon);
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
