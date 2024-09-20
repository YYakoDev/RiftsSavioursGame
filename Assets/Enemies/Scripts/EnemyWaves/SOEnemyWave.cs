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
    [SerializeField] private float _graceTime = 5f;
    [SerializeField] bool _changeStatsOvertime = true, _spawnElite = false;
    [SerializeField] int _maxEnemiesToSpawn = 25;

    //Properties
    public SOEnemy[] Enemies => _enemies;
    public float EnemySpawnCooldown => _enemySpawnCooldown;
    public int EnemiesToSpawn => _enemiesToSpawn;
    public float GraceTime => _graceTime;
    public bool ChangeStatsOvertime => _changeStatsOvertime;
    public bool SpawnElite => _spawnElite;
    public int MaxEnemiesToSpawn => _maxEnemiesToSpawn;
}
