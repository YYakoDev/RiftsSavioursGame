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
    }

    void SetNextLevelXP()
    {
        //do the nextXPToLevelUP FORMULA
    }

    private void OnDestroy() {
        onLevelUp -= SetNextLevelXP;
    }
}
