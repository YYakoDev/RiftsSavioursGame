using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class SOPlayerStats : PlayerStatsBase
{   
    public event Action onStatsChange;

    [Header("Health Stats")]
    [SerializeField]private int _maxHealth = 100;
    [SerializeField]private int _currentHealth;

    [Header("Movement Stats")]
    [SerializeField]private float _speed;
    [SerializeField, Range(0.15f,1f)]private float _slowdownMultiplier = 0.5f;
    [SerializeField] private float _dashSpeed = 10f, _dashCooldown = 1f;
    [SerializeField, Range(0f, 1f)] float _dashInvulnerabilityTime = 0.1f;

    [Header("PickUp Stats")]
    [SerializeField]private float _pickUpRange = 2f;

    [Header("Collecting Stats")]
    [SerializeField]float _collectingRange = 2f;
    [SerializeField]float _collectingDamage = 1;
    [Range(0,5), SerializeField]float _interactCooldown = 0.8f;
    [SerializeField, Range(1f, 100f)]float _maxResourceInteractions = 1; // a cap to the amount of resource you can interact with

    [Header("Weapon & Attack Stats")]
    [SerializeField]private WeaponBase[] _weapons = new WeaponBase[3];
    [SerializeField]private SOPlayerAttackStats _attackStats;

    [Header("Defense Stats")]
    [SerializeField, Range(0, 90)] private float _stunResistance = 1;
    [SerializeField, Range(0, 90)] private float _knockbackResistance = 10;
    [SerializeField, Range(0, 90)] private float _damageResistance = 1;
    [SerializeField, Range(0, 300)] private float _buffBooster = 1;
    [SerializeField, Range(0, 90)] private float _debuffResistance = 1;

    [Header("Luck Stats")]
    [SerializeField] private float _faith = 1;
    [SerializeField] private float _harvestMultiplier;

    //properties

    // HEALTH MANAGER
    public int MaxHealth 
    {   
        get => _maxHealth; 
        set 
        {
            var healthDiff = _maxHealth - _currentHealth;
            _maxHealth = value; 
            if(healthDiff > 0)
            {
                int result = value - healthDiff;
                if(result <= 0) _currentHealth = 1;
                else _currentHealth = result;

            }else if(_currentHealth > _maxHealth) 
                _currentHealth = _maxHealth;
            onStatsChange?.Invoke(); 
        }
    }
    public int CurrentHealth {get => _currentHealth; set {_currentHealth = value; onStatsChange?.Invoke();}}

    // MOVEMENT STATS
    public float Speed {get => _speed; set {_speed = Mathf.Clamp(value, 0.1f, 8f); onStatsChange?.Invoke();}}
    public float SlowdownMultiplier {get => _slowdownMultiplier; set {_slowdownMultiplier = Mathf.Clamp(value, 0.1f, 1f); onStatsChange?.Invoke();}}
    public float DashSpeed { get => _dashSpeed; set { _dashSpeed = value; onStatsChange?.Invoke(); } }
    public float DashCooldown { get => _dashCooldown; set { _dashCooldown = Mathf.Clamp(value, 0.075f,100f); onStatsChange?.Invoke(); } }
    public float DashInvulnerabilityTime { get => _dashInvulnerabilityTime; set { _dashInvulnerabilityTime = Mathf.Clamp(value, 0, 1.5f); onStatsChange?.Invoke(); } }

    // PICKUP STATS
    public float PickUpRange {get => _pickUpRange; set {_pickUpRange = value; onStatsChange?.Invoke();}}


    // COLLECTING STATS
    public float CollectingRange {get => _collectingRange; set {_collectingRange = value; onStatsChange?.Invoke();}}
    public float CollectingDamage {get => _collectingDamage; set {_collectingDamage = value; onStatsChange?.Invoke();}}
    public float InteractCooldown {get => _interactCooldown; set {_interactCooldown = Mathf.Clamp(value, 0.05f, 5f); onStatsChange?.Invoke();}}
    public float MaxResourceInteractions {get => _maxResourceInteractions; set {_maxResourceInteractions = Mathf.Clamp(value, 1, 90); onStatsChange?.Invoke();}}


    // WEAPON STATS
    public WeaponBase[] Weapons {get => _weapons;}
    public SOPlayerAttackStats AttackStats {get => _attackStats;}

    // DEFENSE STATS
    public float StunResistance { get => (_stunResistance); set { _stunResistance = Mathf.Clamp(value, 0, 90); onStatsChange?.Invoke(); } }
    public float KnockbackResistance { get => (_knockbackResistance); set { _knockbackResistance = Mathf.Clamp(value, 0, 90); onStatsChange?.Invoke(); } }
    public float DamageResistance { get => (_damageResistance); set { _damageResistance = Mathf.Clamp(value, 0, 90); onStatsChange?.Invoke(); } }
    public float BuffBooster { get => (_buffBooster); set { _buffBooster = Mathf.Clamp(value, 0, 300); onStatsChange?.Invoke(); } }
    public float DebuffResistance { get => (_debuffResistance); set { _debuffResistance = Mathf.Clamp(value, 0, 90); onStatsChange?.Invoke(); } }



    // LUCK STATS
    public float Faith { get => (_faith);  set { _faith = value; onStatsChange?.Invoke(); } }
    public float HarvestMultiplier { get => _harvestMultiplier;  set { _harvestMultiplier = value; onStatsChange?.Invoke(); } }

    public void Initialize(SOCharacterData data)
    {
        SOPlayerStats stats = data.Stats;
        _maxHealth = stats.MaxHealth;
        _currentHealth = _maxHealth;
        _speed = stats.Speed;
        _slowdownMultiplier = stats.SlowdownMultiplier;
        _dashSpeed = stats.DashSpeed;
        _dashCooldown = stats.DashCooldown;
        _dashInvulnerabilityTime = stats.DashInvulnerabilityTime;
        _pickUpRange = stats.PickUpRange;

        _collectingRange = stats.CollectingRange;
        _collectingDamage = stats.CollectingDamage;
        _interactCooldown = stats.InteractCooldown;
        _maxResourceInteractions = stats.MaxResourceInteractions;

        Array.Copy(stats.Weapons, _weapons, _weapons.Length);
        _attackStats.Initialize(stats.AttackStats);

        _stunResistance = stats.StunResistance;
        _knockbackResistance = stats.KnockbackResistance;
        _damageResistance = stats.DamageResistance;

        _faith = stats.Faith;
        _harvestMultiplier = stats._harvestMultiplier;
        
    }

    private void OnValidate() {
        if(_weapons.Length > 2)Array.Resize<WeaponBase>(ref _weapons, 2);
    }
}
