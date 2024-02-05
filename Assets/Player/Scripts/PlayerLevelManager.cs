using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField]private SOPlayerStats _playerStats;
    //private int _currentLevel;
    public static event Action onLevelUp;

    public void SetPlayerStats(SOPlayerStats stats)
    {
        _playerStats = stats;
    }

    private IEnumerator Start() 
    {
        yield return null;
        this.enabled = (_playerStats != null);
        _playerStats.Level = 1;
        _playerStats.CurrentXP = 0;
        SetNextLevelXP();
        onLevelUp += SetNextLevelXP;
    }

    public void AddXP(int xp)
    {
        _playerStats.CurrentXP += xp;
        if(_playerStats.CurrentXP >= _playerStats.XPToNextLevel)
        {
            AddXP(-_playerStats.XPToNextLevel);
            LevelUp();
        }
    }

    public void LevelUp()
    {
        _playerStats.Level++;
        onLevelUp?.Invoke();
        var currentHealth = _playerStats.CurrentHealth;
        currentHealth += 5;
        currentHealth = Mathf.Clamp(currentHealth, 5, _playerStats.MaxHealth);
        _playerStats.CurrentHealth = currentHealth;
    }

    void SetNextLevelXP()
    {
        //do the nextXPToLevelUP FORMULA
        _playerStats.XPToNextLevel = 20 * _playerStats.Level + 5 * _playerStats.Level;
    }

    private void OnDestroy() {
        onLevelUp -= SetNextLevelXP;
    }
}
