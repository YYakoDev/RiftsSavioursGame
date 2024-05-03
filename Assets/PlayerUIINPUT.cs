using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIINPUT : MonoBehaviour
{
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] PlayerMovement _movementScript;
    WeaponBase _currentWeapon;
    UISkill _dashSkill, _attackSkill;


    private void Awake() {
        _weaponManager.OnWeaponChange += SetWeapon;
    }

    private IEnumerator Start() {
        yield return null;
        yield return null;
        _dashSkill = _movementScript.UIInputSkill;
        _attackSkill = _weaponManager.UIInputSkill;
        _movementScript.onDash += PlayDashInput;
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

    void PlayAttackInput()
    {
        _attackSkill?.Interact();
    }

    void PlayDashInput()
    {
        _dashSkill?.Interact();
    }

    private void OnDestroy() {
        _weaponManager.OnWeaponChange -= SetWeapon;
        _movementScript.onDash -= PlayDashInput;
    }

}
