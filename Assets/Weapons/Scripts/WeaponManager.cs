using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField]private PlayerManager _playerManager;
    [SerializeField]private PlayerAttackEffects _effects;
    private SOPlayerStats _playerStats;
    private SOPlayerAttackStats _playerAttackStats;
    [SerializeField]private GameObject _weaponPrefab;
    [SerializeField]private WeaponAiming _weaponAiming;
    [SerializeField]private LayerMask _enemyLayer;
    private WeaponBase _currentWeapon;
    GameObject _weaponPrefabInstance;
    WeaponPrefab _weaponLogicInstance;
    private KeyInput _switchKey;
    private int _weaponIndex, _maxWeaponAmount = 3;
    private int WeaponIndex 
    {
        get => _weaponIndex;
        set
        {
            _weaponIndex = (value >= _maxWeaponAmount) ? 0 : value;
        }
    }
    public event Action<WeaponBase> OnWeaponChange;

    public WeaponAiming AimingLogic => _weaponAiming;
    public PlayerAttackEffects AtkEffects => _effects;
    public LayerMask EnemyLayer => _enemyLayer;

    private void Awake()
    {
        if (_playerManager == null) _playerManager = GetComponentInParent<PlayerManager>();
        if(_weaponAiming == null) _weaponAiming = GetComponentInChildren<WeaponAiming>();
        gameObject.CheckComponent<PlayerAttackEffects>(ref _effects);
        _playerStats = _playerManager.Stats;
        _playerAttackStats = _playerStats.AttackStats;
        _playerAttackStats.onStatsChange += ApplyNewAttackStats;
    }

    void Start()
    {
        PlayerManager.onCharacterChange += SetWeaponFromNewCharacter;
        if(_weaponPrefab == null || _weaponAiming == null)
        {
            Debug.LogError("A Reference is not assigned to the weapon manager");
            return;
        }
        CreatePrefab();
        _weaponAiming.Initialize(_enemyLayer);
        SetWeapon(_playerStats.Weapons[0]);
        _switchKey = YYInputManager.GetKey(KeyInputTypes.SwitchWeapon);
        _switchKey.OnKeyPressed += SwitchWeapon;
    }


    void CreatePrefab()
    {
        _weaponPrefabInstance = Instantiate(_weaponPrefab, _weaponAiming.transform);
        _weaponLogicInstance = _weaponPrefabInstance.GetComponent<WeaponPrefab>();
        foreach(var weapon in _playerStats.Weapons)
        {
            if(weapon == null) continue;
            weapon.Initialize(this, _weaponPrefabInstance.transform);
            weapon.SetWeaponActive(false);
        }
        
    }

    void SetWeapon(WeaponBase weapon)
    {
        if(weapon == null || _currentWeapon == weapon) return;
        _currentWeapon?.SetWeaponActive(false);
        _currentWeapon = weapon;
        Transform instanceTransform = _weaponPrefabInstance.transform;
        instanceTransform.localPosition = weapon.SpawnPosition;
        var rotation = instanceTransform.localRotation.eulerAngles;
        rotation.z = weapon.SpawnRotation;
        instanceTransform.localRotation = Quaternion.Euler(rotation);
        _weaponLogicInstance.SetWeaponBase(weapon);
        _weaponAiming.SwitchCurrentWeapon(weapon);
        OnWeaponChange?.Invoke(weapon);
        _currentWeapon.SetWeaponActive(true);
    }

    void SwitchWeapon()
    {
        WeaponIndex++;
        SetWeapon(_playerStats.Weapons[WeaponIndex]);
    }

    void SetWeaponFromNewCharacter()
    {
        foreach(var weapon in _playerStats.Weapons)
        {
            if(weapon == null || weapon == _currentWeapon) continue;
            weapon.Initialize(this, _weaponPrefabInstance.transform);
            weapon.SetWeaponActive(false);
        }
        WeaponIndex = 0;    
        SetWeapon(_playerStats.Weapons[WeaponIndex]);
        _playerStats.Weapons[WeaponIndex].SetWeaponActive(true);
    }

    void ApplyNewAttackStats()
    {
        foreach(var weapon in _playerStats.Weapons)
        {
            if(weapon == null) continue;
            weapon.EvaluateStats(_playerAttackStats);
        }
    }

    private void OnDestroy() {
        PlayerManager.onCharacterChange -= SetWeaponFromNewCharacter;
        _playerAttackStats.onStatsChange -= ApplyNewAttackStats;
        _switchKey.OnKeyPressed -= SwitchWeapon;
        foreach(var weapon in _playerStats.Weapons) weapon?.UnsubscribeInput();
    }

}

