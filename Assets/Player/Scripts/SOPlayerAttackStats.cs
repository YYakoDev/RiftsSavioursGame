using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerAttackStats")]
public class SOPlayerAttackStats : PlayerStatsBase
{
    public event Action onStatsChange;

    [SerializeField]private float _damageMultiplier = 1, _baseDamageAddition = 0, _attackRange = 1f, _attackCooldown = 1f, _attackSpeed = 1f;
    [SerializeField, Range(0f , 2.25f)]private float _attackKnockback = 1;
    [SerializeField]private float _projectilesCount = 1f, _projectilesSpeed = 1f, _summonDamage = 5, _summonSpeed = 1.5f;
    [SerializeField]private float _criticalChance = 1f, _criticalDamageMultiplier = 1f;


    //properties
    public float DamageMultiplier {get => _damageMultiplier; set {_damageMultiplier = value; onStatsChange?.Invoke();}}
    public float BaseDamageAddition {get => (_baseDamageAddition); set {_baseDamageAddition = value; onStatsChange?.Invoke();}}
    public float AttackRange {get => _attackRange; set {_attackRange = Mathf.Clamp(value, 0f, 2f); onStatsChange?.Invoke();}} // the range cap should be weapon dependent
    public float AttackCooldown {get => _attackCooldown; set {_attackCooldown = value; onStatsChange?.Invoke();}}
    public float AttackKnockback {get => _attackKnockback; set {_attackKnockback = value; onStatsChange?.Invoke();}}
    public float AttackSpeed {get => _attackSpeed; set {_attackSpeed = value; onStatsChange?.Invoke();}}
    public float ProjectilesCount {get => (_projectilesCount); set {_projectilesCount = value; onStatsChange?.Invoke();}}
    public float ProjectilesSpeed {get => _projectilesSpeed; set {_projectilesSpeed = value; onStatsChange?.Invoke();}}
    public float SummonDamage {get => (_summonDamage); set {_summonDamage = value; onStatsChange?.Invoke();}}
    public float SummonSpeed {get => _summonSpeed; set {_summonSpeed = value; onStatsChange?.Invoke();}}
    public float CriticalChance {get => _criticalChance; set {_criticalChance = value; onStatsChange?.Invoke();}}
    public float CriticalDamageMultiplier {get => _criticalDamageMultiplier; set {_criticalDamageMultiplier = value; onStatsChange?.Invoke();}}

    public void Initialize(SOPlayerAttackStats stats)
    {
        _damageMultiplier = stats._damageMultiplier;
        _baseDamageAddition = stats._baseDamageAddition;
        _attackRange = stats._attackRange;
        _attackCooldown = stats._attackCooldown;
        _attackKnockback = stats._attackKnockback;
        _projectilesCount = stats._projectilesCount;
        _projectilesSpeed = stats._projectilesSpeed;
        _summonDamage = stats._summonDamage;
        _summonSpeed = stats._summonSpeed;
        _attackSpeed = stats._attackSpeed;
        _criticalChance = stats._criticalChance;
        _criticalDamageMultiplier = stats._criticalDamageMultiplier;
    }
}
