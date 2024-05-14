using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
   
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] SOPlayerAttackStats _playerAtkStats;

    public event Action<int, float> onStatModified;

    private void Awake() {
        
        if(_playerStats == null || _playerAtkStats == null) Debug.LogError("ASSIGN THE REFERENCES DUMBASS PlayerStatsManager: PlayerStats or PlayerAttackStats is null");
        
    }
    
    public float GetStat(StatsTypes statCode)
    {
        var code = (int)statCode;
        //return the stat based on the stattype
        float result = -1f;
        var stats = _playerStats;
        var atkStats = _playerAtkStats;
        switch(code)
        {
            case 0:
                result = stats.MaxHealth;
                break;
            case 1:
                result = stats.CurrentHealth;
                break;
            case 2:
                result = stats.Speed;
                break;
            case 3:
                result = stats.SlowdownMultiplier;
                break;
            case 4:
                result = stats.DashSpeed;
                break;
            case 5:
                result = stats.DashCooldown;
                break;
            case 6:
                result = stats.DashInvulnerabilityTime;
                break;
            case 7:
                result = stats.PickUpRange;
                break;
            case 8:
                result = stats.CollectingRange;
                break;
            case 9:
                result = stats.CollectingDamage;
                break;
            case 10:
                result = stats.InteractCooldown;
                break;
            case 11:
                result = stats.MaxResourceInteractions;
                break;
            case 12:
                result = stats.StunResistance;
                break;
            case 13:
                result = stats.KnockbackResistance;
                break;
            case 14:
                result = stats.DamageResistance;
                break;
            case 15:
                result = stats.BuffBooster;
                break;
            case 16:
                result = stats.DebuffResistance;
                break;
            case 17:
                result = stats.Faith;
                break;
            case 18:
                result = stats.HarvestMultiplier;
                break;
            case 19:
                result = atkStats.DamageMultiplier;
                break;
            case 20:
                result = atkStats.BaseDamageAddition;
                break;
            case 21:
                result = atkStats.AttackRange;
                break;
            case 22:
                result = atkStats.AttackCooldown;
                break;
            case 23:
                result = atkStats.AttackKnockback;
                break;
            case 24:
                result = atkStats.ProjectilesCount;
                break;
            case 25:
                result = atkStats.ProjectilesSpeed;
                break;
            case 26:    
                result = atkStats.SummonDamage;
                break;
            case 27:
                result = atkStats.SummonSpeed;
                break;
            case 28:
                result = atkStats.AttackSpeed;
                break;
        }
        return result;
    }

    public void SetStat(StatsTypes type, float increment)
    {
        var statType = (int)type;
        switch(statType)
        {
            case 0:
                _playerStats.MaxHealth += (int)increment;
                break;
            case 1:
                _playerStats.CurrentHealth += (int)increment;
                break;
            case 2:
                _playerStats.Speed += increment;
                break;
            case 3:
                _playerStats.SlowdownMultiplier += increment;
                break;
            case 4:
                _playerStats.DashSpeed += increment;
                break;
            case 5:
                _playerStats.DashCooldown -= increment;
                break;
            case 6:
                _playerStats.DashInvulnerabilityTime += increment;
                break;
            case 7:
                _playerStats.PickUpRange += increment;
                break;
            case 8:
                _playerStats.CollectingRange += increment;
                break;
            case 9:
                _playerStats.CollectingDamage += increment;
                break;
            case 10:
                _playerStats.InteractCooldown -= increment;
                break;
            case 11:
                _playerStats.MaxResourceInteractions += increment;
                break;
            case 12:
                _playerStats.StunResistance += increment;
                break;
            case 13:
                _playerStats.KnockbackResistance += increment;
                break;
            case 14:
                _playerStats.DamageResistance += increment;
                break;
            case 15:
                _playerStats.BuffBooster += increment;
                break;
            case 16:
                _playerStats.DebuffResistance += increment;
                break;
            case 17:
                _playerStats.Faith += increment;
                break;
            case 18:
                _playerStats.HarvestMultiplier += increment;
                break;
            case 19:
                _playerAtkStats.DamageMultiplier += increment;
                break;
            case 20:
                _playerAtkStats.BaseDamageAddition += (int)increment;
                break;
            case 21:
                _playerAtkStats.AttackRange += increment;
                break;
            case 22:
                _playerAtkStats.AttackCooldown += increment;
                break;
            case 23:
                _playerAtkStats.AttackKnockback += increment;
                break;
            case 24:
                _playerAtkStats.ProjectilesCount += (int)increment;
                break;
            case 25:
                _playerAtkStats.ProjectilesSpeed += increment;
                break;
            case 26:
                _playerAtkStats.SummonDamage += (int)increment;
                break;
            case 27:
                _playerAtkStats.SummonSpeed += increment;
                break;
            case 28:
                _playerAtkStats.AttackSpeed += increment;
                break;
        }

    }

}