using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerAttackStats")]
public class SOPlayerAttackStats : PlayerStatsBase
{
    public event Action onStatsChange;

    [SerializeField]private float _damageMultiplier = 1;
    [SerializeField] private int _baseDamageAddition = 0;
    [SerializeField]private float _attackRange;
    [SerializeField]private float _attackCooldown;
    [SerializeField, Range(0f , 2.25f)]private float _attackKnockback;
    [SerializeField]private int _projectilesCount;
    [SerializeField]private float _projectilesSpeed;
    [SerializeField] private int _summonDamage = 5;
    [SerializeField] private float _summonSpeed = 1.5f;



    //properties
    public float DamageMultiplier {get => _damageMultiplier; set {_damageMultiplier = value; onStatsChange?.Invoke();}}
    public int BaseDamageAddition {get => _baseDamageAddition; set {_baseDamageAddition = value; onStatsChange?.Invoke();}}
    public float AttackRange {get => _attackRange; set {_attackRange = value; onStatsChange?.Invoke();}}
    public float AttackCooldown {get => _attackCooldown; set {_attackCooldown = value; onStatsChange?.Invoke();}}
    public float AttackKnockback {get => _attackKnockback; set {_attackKnockback = value; onStatsChange?.Invoke();}}
    public int ProjectilesCount {get => _projectilesCount; set {_projectilesCount = value; onStatsChange?.Invoke();}}
    public float ProjectilesSpeed {get => _projectilesSpeed; set {_projectilesSpeed = value; onStatsChange?.Invoke();}}
    public int SummonDamage {get => _summonDamage; set {_summonDamage = value; onStatsChange?.Invoke();}}
    public float SummonSpeed {get => _summonSpeed; set {_summonSpeed = value; onStatsChange?.Invoke();}}

    public void Initialize(SOPlayerAttackStats stats)
    {
        _damageMultiplier = stats._damageMultiplier;
        _baseDamageAddition = stats.BaseDamageAddition;
        _attackRange = stats._attackRange;
        _attackCooldown = stats._attackCooldown;
        _attackKnockback = stats._attackKnockback;
        _projectilesCount = stats._projectilesCount;
        _projectilesSpeed = stats._projectilesSpeed;
        _summonDamage = stats.SummonDamage;
        _summonSpeed = stats.SummonSpeed;
    }
}
