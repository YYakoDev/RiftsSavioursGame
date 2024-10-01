using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DifficultyScaler : MonoBehaviour
{
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] SOPlayerAttackStats _playerAttackStats;
    [SerializeField] WaveSystem _waveSys;
    [SerializeField] int _scalingThreshold = 5;
    [SerializeField] DifficultyStats _maxStats = new();
    DifficultyStats _currentStats = new();
    int _waveCount = 0;
    public event Action OnDifficultyIncrease;

    public DifficultyStats CurrentStats => _currentStats;

    private void Start() {
        OnDifficultyIncrease?.Invoke();
        _waveSys.OnWaveChange += CheckWaveCount;
    }

    void CheckWaveCount(SOEnemyWave wave)
    {
        _waveCount++;
        if(_waveCount >= _scalingThreshold)
        {
            _waveCount = 0;
            if(_currentStats.GraceDuration <= _maxStats.GraceDuration)_currentStats.GraceDuration += 1f;
            _currentStats.EnemiesToSpawn += 3;
            if(_currentStats.PlayerDamage <= _maxStats.PlayerDamage) _currentStats.PlayerDamage += 1;
            if(_currentStats.WavesRestTime <= _maxStats.WavesRestTime)_currentStats.WavesRestTime += 0.5f;
            _currentStats.EnemyDamage += 2;
            if(_currentStats.EnemySpeed <= _maxStats.EnemySpeed)_currentStats.EnemySpeed += 0.3f;
            if(_currentStats.PlayerSpeed <= _maxStats.PlayerSpeed)_currentStats.PlayerSpeed += 0.095f;
            _currentStats.SpawnCooldown -= 0.1f;
            _currentStats.SpawnElite = (_waveSys.WaveNumber % 10f == 0f);
            _currentStats.MaxEnemiesToSpawn += 3;
            _currentStats.EnemyHealth += 3;

            _playerStats.Speed += _currentStats.PlayerSpeed;
            _playerAttackStats.BaseDamageAddition += _currentStats.PlayerDamage;
            NotificationSystem.SendNotification(NotificationType.Bottom, "Difficulty Increased!", waitTime: 1.5f);
            OnDifficultyIncrease?.Invoke();
            //play a sound, shake the screen?, change the wave icon and do a little animation
        }
    }

    private void OnDestroy() {
        _waveSys.OnWaveChange -= CheckWaveCount;
    }
}
