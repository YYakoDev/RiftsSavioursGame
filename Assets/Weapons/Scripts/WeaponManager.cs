using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] InputActionReference _switchKey, _attackKey;
    [SerializeField] float _holdTimeThreshold = 0.225f;
    private bool _allowSwitch = true, _holdingKey;
    private float _switchCooldown = 0.2f, _keyHoldingTime;
    private int _weaponIndex = 0, _maxWeaponAmount = 2;
    private int WeaponIndex 
    {
        get => _weaponIndex;
        set
        {
            _weaponIndex = (value >= _maxWeaponAmount) ? 0 : value;
        }
    }
    public event Action<WeaponBase> OnWeaponChange;
    public Transform PrefabInstance => _weaponPrefabInstance.transform;
    public WeaponPrefab PrefabLogicInstance => _weaponLogicInstance;
    public InputActionReference AttackInputRef => _attackKey;
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
        _switchKey.action.started += CheckSwitchInput;
        _switchKey.action.canceled += StopSwitchInput;
    }

    private void Update() {
        if(_switchCooldown > 0)
        {
            _switchCooldown -= Time.deltaTime;
            if(_switchCooldown <= 0) EnableSwitch();
        }
        if(_holdingKey) _keyHoldingTime += Time.deltaTime;
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
        if(!weapon.Initialized) weapon.Initialize(this, _weaponPrefabInstance.transform);
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

   /* public bool SetWeapon(int index)
    {
        var weapon = _playerStats.Weapons[index];
        if(weapon == null || weapon == _currentWeapon || GetSwitchWeapon(index) == weapon) return false;
        WeaponIndex = index;
        SetWeapon(weapon);
        return true;
    }*/

    public bool SetWeaponAtIndex(int index, WeaponBase weaponToSwitch)
    {
        var weaponEquipped = _playerStats.Weapons[index];
        if(weaponEquipped != null && weaponEquipped == weaponToSwitch || GetSwitchWeapon(index) == weaponToSwitch)
        {
            Debug.Log("You already have that weapon equipped");
            return false;
        }
        WeaponIndex = index;
        SetWeapon(weaponToSwitch);
        return true;

    }

    void CheckSwitchInput(InputAction.CallbackContext obj)
    {
        _holdingKey = true;
    }
    void StopSwitchInput(InputAction.CallbackContext obj)
    {
        _holdingKey = false;
        if(_keyHoldingTime >= _holdTimeThreshold) SwitchWeapon();
        else QuickSwitch();
        _keyHoldingTime = 0f;
    }

    void SwitchWeapon()
    {
        if(!_allowSwitch) return;
        if(_attackKey.action.IsPressed())
        {
            NotificationSystem.SendNotification(NotificationType.Left, "Can't Switch", animDurationScaler: 0.65f);
            return;
        }
        WeaponIndex++;
        SetWeapon(_playerStats.Weapons[WeaponIndex]);
        SetSwitchCooldown(_currentWeapon.AtkDuration);
    }

    void QuickSwitch()
    {
        if(!_allowSwitch) return;
        SetSwitchCooldown(0.05f);
        var switchWeapon = GetSwitchWeapon();
        if(switchWeapon == null) return;
        var currentInfo = _currentWeapon.GetSwitchInfo();
        var nextWeaponInfo = switchWeapon?.GetSwitchInfo();
        if(currentInfo == null || nextWeaponInfo == null)
        {
            //NotificationSystem.SendNotification(NotificationType.Left, "Can't do Quick Switch", animDurationScaler: 0.65f);
            return;
        }
        SetWeapon(switchWeapon);
        _currentWeapon.QuickSwitch(currentInfo);
        WeaponIndex++;
        NotificationSystem.SendNotification(NotificationType.Right, "Quick Switch!", _currentWeapon.SpriteAndAnimationData.Sprite, 0.875f);
        _effects.BlinkWeapon(0.15f);
        SetSwitchCooldown(_currentWeapon.AtkDuration - (Time.deltaTime * 5f));
        PostProcessingManager.SetMotionBlur(0.35f);
        YYExtensions.i.ExecuteEventAfterTime(_currentWeapon.AtkDuration / 2f, () =>
        {
            PostProcessingManager.SetMotionBlur(0f);
        });
        //GameFreezer.FreezeGame(0.015f);
    }

    public WeaponBase GetSwitchWeapon()
    {
        WeaponIndex++;
        var weapon = _playerStats.Weapons[WeaponIndex];
        WeaponIndex--;
        return weapon;
    }
    public WeaponBase GetSwitchWeapon(int index)
    {
        if(index < _playerStats.Weapons.Length - 1)
        {
            var weapon = _playerStats.Weapons[index + 1];
            return weapon;
        }else
        {
            var weapon = _playerStats.Weapons[0];
            return weapon;
        }
    }

    public void SetSwitchCooldown(float cd)
    {
        StopSwitch();
        _switchCooldown = cd;
    }
    public void StopSwitch() => _allowSwitch = false;
    public void EnableSwitch() => _allowSwitch = true;

    void SetWeaponFromNewCharacter()
    {
        Debug.Log("Setting weapon from new chara");
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
        _switchKey.action.started -= CheckSwitchInput;
        _switchKey.action.canceled -= StopSwitchInput;
        foreach(var weapon in _playerStats.Weapons) weapon?.UnsubscribeInput();
    }

}

