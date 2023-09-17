using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyWave")]
public class SOEnemyWave : ScriptableObject
{
    //[SerializeField]private EnemyBrain[] enemyPrefabs;
    [SerializeField]EnemySignature[] _enemiesSignatures;
    [SerializeField, Range(0.2f,2f)]private float _enemySpawnCooldown = 1f; // seconds
    [SerializeField, Range(0,10)]private int enemiesToSpawn = 1;

    
    [Header("Enemy Stats To Apply")]
    [SerializeField]private float _enemiesSpeed = 1f;

    //Properties
    public EnemySignature[] EnemiesSignatures => _enemiesSignatures;
    public float EnemySpawnCooldown => _enemySpawnCooldown;
    public int EnemiesToSpawn => enemiesToSpawn;

    //Enemy Stats To Apply
    public float EnemiesSpeed => _enemiesSpeed;
}
