using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField]private PlayerManager _playerManager;
    private SOPlayerStats _playerStats;
    [SerializeField]private GameObject _weaponSheathe;
    [SerializeField]private GameObject _weaponPrefab;
    [SerializeField]private WeaponParentAiming _weaponParent;
    [SerializeField]private LayerMask _enemyLayer;
    private GameObject _weaponPrefabInstance;
    private SOPlayerAttackStats _attackStats;
    private WeaponBase _currentWeapon;
    Timer _atkDurationTimer;
    //properties
    public SOPlayerAttackStats AttackStats => _attackStats;
    public LayerMask EnemyLayer => _enemyLayer;
    public GameObject WeaponPrefab => _weaponPrefab;

    private void Awake()
    {
        if(_playerManager == null) _playerManager = GetComponentInParent<PlayerManager>();
        if(_weaponParent == null) _weaponParent = GetComponentInChildren<WeaponParentAiming>();
        _playerStats = _playerManager.Stats;
        _atkDurationTimer = new(1, false);
        _atkDurationTimer.onTimerStart += HideWeaponSheathe;
        _atkDurationTimer.onReset += ShowSheathe;
    }

    IEnumerator Start()
    {
        _attackStats = _playerStats.AttackStats;
        _currentWeapon = _playerStats.WeaponBase;
        if(_weaponPrefab == null || _currentWeapon == null || _weaponParent == null)
        {
            Debug.LogError("A Reference is not assigned to the weapon manager");
            yield break;
        }
        _weaponParent.Initialize(_enemyLayer, _currentWeapon);

        if(_weaponPrefabInstance == null)
        {
            _weaponPrefabInstance = Instantiate(_weaponPrefab);
            _weaponPrefabInstance.name = _currentWeapon.WeaponName;

            _weaponPrefabInstance.transform.SetParent(_weaponParent.transform);
            _weaponPrefabInstance.transform.localPosition = _currentWeapon.SpawnPosition;
            
            var rotation = _weaponPrefabInstance.transform.localRotation.eulerAngles;
            rotation.z = _currentWeapon.SpawnRotation;
            _weaponPrefabInstance.transform.localRotation = Quaternion.Euler(rotation);

            _weaponPrefabInstance.GetComponent<WeaponPrefab>().SetWeaponBase(_currentWeapon);
        }
        _currentWeapon.Initialize(this, _weaponPrefabInstance.transform);

        _currentWeapon.onAttack += PlayAttackAnimation;
        yield return null;
        _atkDurationTimer.ChangeTime(_playerManager.AnimController.AtkDuration);
    }

    private void Update() {
        _atkDurationTimer.UpdateTime();
    }

    void PlayAttackAnimation()
    {
        _playerManager.AnimController.PlayStated(PlayerAnimationsNames.Attack);
        _atkDurationTimer.Start();
    }

    void HideWeaponSheathe()
    {
        _weaponSheathe.SetActive(false);
    }

    void ShowSheathe()
    {
        _weaponSheathe.SetActive(true);
    }
    private void OnDestroy() {
        _atkDurationTimer.onTimerStart -= HideWeaponSheathe;
        _atkDurationTimer.onReset -= ShowSheathe;
        _currentWeapon.onAttack -= PlayAttackAnimation;
    }



}
