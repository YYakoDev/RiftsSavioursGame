using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "DoubleAttackMeleeWeapon")]
public class DoubleAttackMelee : MeleeWeapon
{
    [SerializeField] TestWeaponSlash _weaponSlashPrefab;
    [SerializeField] Vector3 _slashOffset = new Vector3(1.35f, 0.1f, 0f);
    TestWeaponSlash _slashInstance;
    Vector3 _slashScale;
    int _atkIndex = 0;
    float _slashYDirection;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _slashInstance = Instantiate(_weaponSlashPrefab);
        _slashScale = _slashInstance.transform.localScale;
        _slashInstance.gameObject.SetActive(false);
        _currentAnim = Animator.StringToHash("Attack");
        _atkIndex = 0;
        _randomizeSounds = false;
        SetSounds();
    }
    protected override void Attack(float cooldown)
    {
        base.Attack(cooldown);
        _attackSound = _weaponSounds[_atkIndex];
        SetAtkIndex();
        SpawnSlash();
    }

    void SpawnSlash()
    {
        _slashInstance.gameObject.SetActive(true);

        Vector2 dirToWeapon = _weaponPrefabTransform.position - _weaponManager.transform.position;
        float rangeSign = (dirToWeapon.x < 0) ? -1 : 1;
        _slashInstance.transform.position = _attackPoint + _slashOffset * rangeSign;
        
        Vector3 newScale = _slashInstance.transform.localScale;
        newScale.y = _slashYDirection * rangeSign;
        //newScale.x = _slashScale.x * rangeSign;
        _slashInstance.transform.localScale = newScale;

        float angle = Mathf.Atan2(dirToWeapon.y, dirToWeapon.x) * Mathf.Rad2Deg;
        var rotation = _slashInstance.transform.rotation.eulerAngles;
        rotation.z = angle;

        _slashInstance.transform.rotation = Quaternion.Euler(rotation);

        _slashInstance.SetAnimationSpeed(_attackSpeed);
        _slashInstance.Play();
    }

    void SetAtkIndex()
    {
        _atkIndex++;
        if(_atkIndex > 1) _atkIndex = 0;
        string AnimName = (_atkIndex > 0) ? "Attack2" : "Attack";
        _currentAnim = Animator.StringToHash(AnimName);
        _slashYDirection = (_atkIndex > 0) ? -1 : 1;
    }

    void SetSounds()
    {
        int? length = _weaponSounds?.Length;
        if(length == 0)
        {
            Debug.LogError("You need to set a sound for the weapon");
            return;
        }
        if(length != 2)
            Array.Resize<AudioClip>(ref _weaponSounds, 2);
        if(length < 2)
            _weaponSounds[1] = _weaponSounds[0];
        
        
    }
}
