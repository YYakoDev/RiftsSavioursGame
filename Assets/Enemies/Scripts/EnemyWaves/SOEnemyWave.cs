using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemyWave")]
public class SOEnemyWave : ScriptableObject
{
    //[SerializeField]private EnemyBrain[] enemyPrefabs;
    [SerializeField]EnemySignature[] _enemiesSignatures;
    [SerializeField, Range(0.1f,3f)]private float _enemySpawnCooldown = 1f; // seconds
    [SerializeField, Range(0, 15)]private int _enemiesToSpawn = 1;
    [SerializeField] private float _waveDuration = 30f;


    [Header("Enemy Stats To Apply")]
    [SerializeField]private float _enemiesSpeed = 1f;

    //Properties
    public EnemySignature[] EnemiesSignatures => _enemiesSignatures;
    public float EnemySpawnCooldown => _enemySpawnCooldown;
    public int EnemiesToSpawn => _enemiesToSpawn;
    public float WaveDuration => _waveDuration;

    //Enemy Stats To Apply
    public float EnemiesSpeed => _enemiesSpeed;
}
