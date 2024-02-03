using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyWave")]
public class SOEnemyWave : ScriptableObject
{
    //[SerializeField]private EnemyBrain[] enemyPrefabs;
    [SerializeField]SOEnemy[] _enemies;
    [SerializeField, Range(0.05f, 5f)]private float _enemySpawnCooldown = 1f; // seconds
    [SerializeField, Range(0, 20)]private int _enemiesToSpawn = 1;
    [SerializeField] private float _waveDuration = 30f;
    [SerializeField] bool _changeStatsOvertime = true;

    //Properties
    public SOEnemy[] Enemies => _enemies;
    public float EnemySpawnCooldown => _enemySpawnCooldown;
    public int EnemiesToSpawn => _enemiesToSpawn;
    public float WaveDuration => _waveDuration;
    public bool ChangeStatsOvertime => _changeStatsOvertime;
}
