using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerStats")]
public class SOPlayerStats : ScriptableObject
{   
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
    [Tooltip("The time it takes the player to reach the desired velocity")] [SerializeField, Range(0f, 2f)] private float _accelerationTime = 0.5f;
    [SerializeField, Range(0.15f,1f)]private float _slowdownMultiplier = 0.5f;

    [Header("PickUp Stats")]
    [SerializeField]private float _pickUpRange = 2f;

    [Header("Collecting Stats")]
    [SerializeField]float _collectingRange = 2f;
    [SerializeField]int _collectingDamage = 1;
    [Range(0,5), SerializeField]float _interactCooldown = 0.8f;
    [SerializeField, Range(1, 100)]int _maxResourceInteractions = 1; // a cap to the amount of resource you can interact with

    [Header("Weapon & Attack Stats")]
    [SerializeField]private WeaponBase _weaponBase;
    [SerializeField]private SOPlayerAttackStats _attackStats;

    
    //properties
    
    // HEALTH MANAGER
    public int MaxHealth {get => _maxHealth; set {_maxHealth = value; onStatsChange?.Invoke();}}
    public int CurrentHealth {get => _currentHealth; set {_currentHealth = value; onStatsChange?.Invoke();}}

    // MOVEMENT STATS
    public float Speed {get => _speed; set {_speed = value; onStatsChange?.Invoke();}}
    public float AccelerationTime { get => _accelerationTime;  set { _accelerationTime = value; onStatsChange?.Invoke(); } }
    public float SlowdownMultiplier {get => _slowdownMultiplier; set {_slowdownMultiplier = value; onStatsChange?.Invoke();}}

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
    public WeaponBase WeaponBase {get => _weaponBase;}
    public SOPlayerAttackStats AttackStats {get => _attackStats;}



    public void Initialize(SOPlayerStats stats)
    {
        _maxHealth = stats.MaxHealth;
        _currentHealth = stats.CurrentHealth;

        _level = 1;
        _currentXP = 0;
        _xpToNextLevel = stats.XPToNextLevel;

        _speed = stats.Speed;
        _slowdownMultiplier = stats.SlowdownMultiplier;

        _pickUpRange = stats.PickUpRange;

        _collectingRange = stats.CollectingRange;
        _collectingDamage = stats.CollectingDamage;
        _interactCooldown = stats.InteractCooldown;
        _maxResourceInteractions = stats.MaxResourceInteractions;

        _weaponBase = stats.WeaponBase;
        _attackStats = stats.AttackStats;
    }

}
