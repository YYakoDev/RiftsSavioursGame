using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class SOPlayerStats : PlayerStatsBase
{   

    //

    // IF YOU ADD ANY STAT TO THIS CLASS DO NOT SAVE BEFORE UPDATING THE TEXTS FROM THE UPGRADES, OTHERWISE SOME UPGRADES WILL LOSE THE REFERENCE TO THE STATS

    //

    public event Action onStatsChange;

    [Header("Health Stats")]
    [SerializeField]private int _maxHealth = 100;
    [SerializeField]private int _currentHealth;

    [Header("Level Stats")]
    [SerializeField]private int _level = 1;
    [SerializeField]private int _currentXP = 0;
    [SerializeField]private int _xpToNextLevel = 100;

    [Header("Movement Stats")]
    [SerializeField]private float _speed;
    [SerializeField, Range(0.15f,1f)]private float _slowdownMultiplier = 0.5f;
    [SerializeField] private float _dashSpeed = 10f, _dashCooldown = 1f;
    [SerializeField, Range(0f, 1f)] float _dashInvulnerabilityTime = 0.1f;

    [Header("PickUp Stats")]
    [SerializeField]private float _pickUpRange = 2f;

    [Header("Collecting Stats")]
    [SerializeField]float _collectingRange = 2f;
    [SerializeField]int _collectingDamage = 1;
    [Range(0,5), SerializeField]float _interactCooldown = 0.8f;
    [SerializeField, Range(1, 100)]int _maxResourceInteractions = 1; // a cap to the amount of resource you can interact with

    [Header("Weapon & Attack Stats")]
    [SerializeField]private WeaponBase[] _weapons = new WeaponBase[3];
    [SerializeField]private SOPlayerAttackStats _attackStats;

    [Header("Defense Stats")]
    [SerializeField, Range(0, 100)] private int _stunResistance = 0;
    [SerializeField, Range(0, 100)] private int _knockbackResistance = 10;
    [SerializeField, Range(0, 100)] private int _damageResistance = 0;

    [Header("Luck Stats")]
    [SerializeField] private int _faith = 1;
    [SerializeField] private float _harvestMultiplier;

    //properties

    // HEALTH MANAGER
    public int MaxHealth {get => _maxHealth; set {_maxHealth = value; onStatsChange?.Invoke();}}
    public int CurrentHealth {get => _currentHealth; set {_currentHealth = value; onStatsChange?.Invoke();}}

    // MOVEMENT STATS
    public float Speed {get => _speed; set {_speed = value; onStatsChange?.Invoke();}}
    public float SlowdownMultiplier {get => _slowdownMultiplier; set {_slowdownMultiplier = value; onStatsChange?.Invoke();}}
    public float DashSpeed { get => _dashSpeed; set { _dashSpeed = value; onStatsChange?.Invoke(); } }
    public float DashCooldown { get => _dashCooldown; set { _dashCooldown = value; onStatsChange?.Invoke(); } }
    public float DashInvulnerabilityTime { get => _dashInvulnerabilityTime; set { _dashInvulnerabilityTime = value; onStatsChange?.Invoke(); } }

    // PICKUP STATS
    public float PickUpRange {get => _pickUpRange; set {_pickUpRange = value; onStatsChange?.Invoke();}}


    // COLLECTING STATS
    public float CollectingRange {get => _collectingRange; set {_collectingRange = value; onStatsChange?.Invoke();}}
    public int CollectingDamage {get => _collectingDamage; set {_collectingDamage = value; onStatsChange?.Invoke();}}
    public float InteractCooldown {get => _interactCooldown; set {_interactCooldown = value; onStatsChange?.Invoke();}}
    public int MaxResourceInteractions {get => _maxResourceInteractions; set {_maxResourceInteractions = value; onStatsChange?.Invoke();}}

    // LEVEL STATS
    public int Level {get => _level; set {_level = value; onStatsChange?.Invoke();}}
    public int CurrentXP {get => _currentXP; set {_currentXP = value; onStatsChange?.Invoke();}}
    public int XPToNextLevel {get => _xpToNextLevel; set {_xpToNextLevel = value; onStatsChange?.Invoke();}}

    // WEAPON STATS
    public WeaponBase[] Weapons {get => _weapons;}
    public SOPlayerAttackStats AttackStats {get => _attackStats;}

    // DEFENSE STATS
    public int StunResistance { get => _stunResistance; set { _stunResistance = value; onStatsChange?.Invoke(); } }
    public int KnockbackResistance { get => _knockbackResistance; set { _knockbackResistance = value; onStatsChange?.Invoke(); } }
    public int DamageResistance { get => _damageResistance; set { _damageResistance = value; onStatsChange?.Invoke(); } }


    // LUCK STATAS
    public int Faith { get => _faith;  set { _faith = value; onStatsChange?.Invoke(); } }
    public float HarvestMultiplier { get => _harvestMultiplier;  set { _harvestMultiplier = value; onStatsChange?.Invoke(); } }

    public void Initialize(SOCharacterData data)
    {
        SOPlayerStats stats = data.Stats;
        _maxHealth = stats.MaxHealth;
        _currentHealth = _maxHealth;

        _level = 1;
        _currentXP = 0;
        _xpToNextLevel = stats.XPToNextLevel;

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
        //onStatsChange?.Invoke();
    }

    private void OnValidate() {
        if(_weapons.Length > 2)Array.Resize<WeaponBase>(ref _weapons, 3);
    }
}
