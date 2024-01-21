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
    private Dictionary<WeaponBase, WeaponPrefab> _instantiatedWeapons = new();
    private WeaponBase _currentWeapon;
    private GameObject _currentWeaponInstance;
    public event Action<WeaponBase> OnWeaponChange;
    //properties
    //public SOPlayerAttackStats AttackStats => _attackStats;
    public PlayerAttackEffects AtkEffects => _effects;
    //public WeaponAiming WeaponAiming => _weaponAiming;
    public LayerMask EnemyLayer => _enemyLayer;
    //public WeaponBase CurrentWeapon => _currentWeapon;
    //public GameObject CurrentWeaponInstance => _currentWeaponInstance;
    //public GameObject WeaponPrefab => _weaponPrefab;

    private void Awake()
    {
        if(_playerManager == null) _playerManager = GetComponentInParent<PlayerManager>();
        if(_weaponAiming == null) _weaponAiming = GetComponentInChildren<WeaponAiming>();
        gameObject.CheckComponent<PlayerAttackEffects>(ref _effects);
        _playerStats = _playerManager.Stats;
        _playerAttackStats = _playerStats.AttackStats;
        _playerAttackStats.onStatsChange += ApplyNewAttackStats;
    }

    void Start()
    {
        if(_weaponPrefab == null || _weaponAiming == null)
        {
            Debug.LogError("A Reference is not assigned to the weapon manager");
            return;
        }
        //_currentWeapon = _playerStats.WeaponBase;
        foreach(WeaponBase weapon in _playerStats.Weapons)
        {
            SpawnWeapon(weapon);
            Debug.Log("SpawningWeapon");
        }

        SetCurrentWeapon(_playerStats.Weapons[0]);
        YYInputManager.GetKey(KeyInputTypes.SwitchToWeapon1).OnKeyPressed += SelectWeapon1;
        YYInputManager.GetKey(KeyInputTypes.SwitchToWeapon2).OnKeyPressed += SelectWeapon2;
        YYInputManager.GetKey(KeyInputTypes.SwitchToWeapon3).OnKeyPressed += SelectWeapon3;
    }

    void SetCurrentWeapon(WeaponBase newWeapon)
    {
        if(_currentWeapon != null && _currentWeapon == newWeapon) return;
        //here we will deactivated the previous current weapon if it exist
        if(_currentWeapon != null)
        {
            _instantiatedWeapons[_currentWeapon].Animator.enabled = false;
            _currentWeapon.SetWeaponActive(false);
            _currentWeaponInstance.SetActive(false);
        }

        _currentWeapon = newWeapon;
        _currentWeaponInstance = _instantiatedWeapons[_currentWeapon].gameObject;
        var instanceScript = _instantiatedWeapons[_currentWeapon];
        instanceScript.Animator.enabled = false;
        instanceScript.Renderer.sprite = _currentWeapon.SpriteAndAnimationData.Sprite;
        instanceScript.Renderer.color = Color.white;
        instanceScript.Animator.enabled = true;
        _currentWeapon.SetWeaponActive(true);
        _currentWeaponInstance.SetActive(true);
        _weaponAiming.Initialize(_currentWeapon, _enemyLayer);
        OnWeaponChange?.Invoke(_currentWeapon);
    }

    void SpawnWeapon(WeaponBase weapon)
    {
        if(weapon == null) return;
        if(_instantiatedWeapons.ContainsKey(weapon)) return;
        GameObject instance = Instantiate(_weaponPrefab);
        instance.name = weapon.WeaponName;

        Transform instanceTransform = instance.transform;
        instanceTransform.SetParent(_weaponAiming.transform);
        instanceTransform.localPosition = weapon.SpawnPosition;
        
        var rotation = instanceTransform.localRotation.eulerAngles;
        rotation.z = weapon.SpawnRotation;
        instanceTransform.localRotation = Quaternion.Euler(rotation);

        var weaponPrefabScript = instance.GetComponent<WeaponPrefab>();

        instance.SetActive(false);
        weapon.Initialize(this, instanceTransform);
        weapon.SetWeaponActive(false);
        _instantiatedWeapons.Add(weapon, weaponPrefabScript);
        weaponPrefabScript?.SetWeaponBase(weapon);
        //Debug.Log($"Adding new weapon to dictionary:  {weapon.name} + {instance}");
    }

    void SelectWeapon1() => SwitchWeapon(0);
    void SelectWeapon2() => SwitchWeapon(1);
    void SelectWeapon3() => SwitchWeapon(2);

    void SwitchWeapon(int newIndex)
    {
        if(newIndex >= _instantiatedWeapons.Count) return;
        SetCurrentWeapon(_playerStats.Weapons[newIndex]);

    }

    int SetNewIndex(int amountToAdd)
    {
        int currentIndex = 0;
        for (int i = 0; i < _playerStats.Weapons.Length; i++)
        {
            var weapon = _playerStats.Weapons[i];
            if(_currentWeapon == weapon)
            {
                currentIndex = i;
                break;
            }
        }
        currentIndex += amountToAdd;
        return (currentIndex > _instantiatedWeapons.Count-1) ? 0 : currentIndex;
    }

    void ApplyNewAttackStats()
    {
        foreach(var weaponPairedInstance in _instantiatedWeapons)
        {
            weaponPairedInstance.Key.EvaluateStats(_playerAttackStats);
        }
    }

    private void OnDestroy() {
        _playerAttackStats.onStatsChange -= ApplyNewAttackStats;
        YYInputManager.GetKey(KeyInputTypes.SwitchToWeapon1).OnKeyPressed -= SelectWeapon1;
        YYInputManager.GetKey(KeyInputTypes.SwitchToWeapon2).OnKeyPressed -= SelectWeapon2;
        YYInputManager.GetKey(KeyInputTypes.SwitchToWeapon3).OnKeyPressed -= SelectWeapon3;
    }

}

