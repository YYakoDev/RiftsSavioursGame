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
    private GameObject _weaponPrefabInstance;
    private WeaponBase _currentWeapon;
    //properties
    //public SOPlayerAttackStats AttackStats => _attackStats;
    public PlayerAttackEffects AtkEffects => _effects;
    public WeaponAiming WeaponAiming => _weaponAiming;
    public GameObject WeaponPrefab => _weaponPrefabInstance;
    public LayerMask EnemyLayer => _enemyLayer;
    public WeaponBase CurrentWeapon => _currentWeapon;
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
        _currentWeapon = _playerStats.WeaponBase;
        if(_weaponPrefab == null || _currentWeapon == null || _weaponAiming == null)
        {
            Debug.LogError("A Reference is not assigned to the weapon manager");
            return;
        }
        _weaponAiming.Initialize(_currentWeapon, _enemyLayer);

        if(_weaponPrefabInstance == null)
        {
            _weaponPrefabInstance = Instantiate(_weaponPrefab);
            _weaponPrefabInstance.name = _currentWeapon.WeaponName;

            _weaponPrefabInstance.transform.SetParent(_weaponAiming.transform);
            _weaponPrefabInstance.transform.localPosition = _currentWeapon.SpawnPosition;
            
            var rotation = _weaponPrefabInstance.transform.localRotation.eulerAngles;
            rotation.z = _currentWeapon.SpawnRotation;
            _weaponPrefabInstance.transform.localRotation = Quaternion.Euler(rotation);

            _weaponPrefabInstance.GetComponent<WeaponPrefab>().SetWeaponBase(_currentWeapon);
        }
        _currentWeapon.Initialize(this, _weaponPrefabInstance.transform);
    }

    void ApplyNewAttackStats()
    {
        _currentWeapon.EvaluateStats(_playerAttackStats);
    }

    private void OnDestroy() {
        _playerAttackStats.onStatsChange -= ApplyNewAttackStats;
    }

}
