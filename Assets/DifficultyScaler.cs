using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DifficultyScaler : MonoBehaviour
{
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] SOPlayerAttackStats _playerAttackStats;
    [SerializeField] World _currentWorld;
    [SerializeField] int _scalingThreshold = 5;
    [SerializeField] DifficultyStats _maxStats = new();
    DifficultyStats _currentStats = new();
    int _waveCount = 0;
    public event Action OnDifficultyIncrease;

    public DifficultyStats CurrentStats => _currentStats;

    private void Start() {
        _currentWorld.OnWaveChange += CheckWaveCount;
    }

    void CheckWaveCount(SOEnemyWave wave)
    {
        _waveCount++;
        if(_waveCount >= _scalingThreshold)
        {
            _waveCount = 0;
            _currentStats.Duration += 2f;
            _currentStats.EnemiesToSpawn += 1;
            _currentStats.PlayerDamage += 2;
            _currentStats.WavesRestTime += 1f;
            _currentStats.EnemyDamage += 1;
            _currentStats.EnemySpeed += 0.065f;
            _currentStats.PlayerSpeed += 0.1f;
            _currentStats.SpawnCooldown -= 0.13f;
            _currentStats.SpawnElite = (_currentWorld.WaveIndex % 10f == 0f);

            _playerStats.Speed += _currentStats.PlayerSpeed;
            _playerAttackStats.BaseDamageAddition += _currentStats.PlayerDamage;
            NotificationSystem.SendNotification(NotificationType.Bottom, "Difficulty Increased!", waitTime: 1.5f);
            OnDifficultyIncrease?.Invoke();
            //play a sound, shake the screen?, change the wave icon and do a little animation
        }
    }

    private void OnDestroy() {
        _currentWorld.OnWaveChange -= CheckWaveCount;
    }
}
