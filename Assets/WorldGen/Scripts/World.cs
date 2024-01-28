using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObjects/World")]
public class World : ScriptableObject
{
    [SerializeField]private string _name;
    public const float RiftDuration = 900f; // 15mins
    float _restInterval = 10f;
    [SerializeField]private SOEnemyWave[] _waves;
    [SerializeField]private EnemyBrain[] _enemyPrefabs;
    [SerializeField]private ChunkTileMap[] _chunks = new ChunkTileMap[0];

    private SOEnemyWave _currentWave;
    private int _currentWaveIndex = 0;

    

    //properties
    public static float RiftDurationInSeconds => RiftDuration;
    public SOEnemyWave CurrentWave => _currentWave;
    public float WavesInterval => _currentWave.WaveDuration;
    public float RestInterval => _restInterval;
    public EnemyBrain[] EnemyPrefabs => _enemyPrefabs;
    public ChunkTileMap[] Chunks => _chunks;

    public void Initialize(World world)
    {
        _name = world._name;
        _waves = world._waves.Clone() as SOEnemyWave[];
        _chunks = world._chunks.Clone() as ChunkTileMap[];
        _enemyPrefabs = world._enemyPrefabs.Clone() as EnemyBrain[];
        _currentWaveIndex = 0;
        if(_waves.Length <= _currentWaveIndex)
        {
            Debug.LogError($"<b> No waves found in the World: {_name} </b>");
            return;
        }
        _currentWave = _waves[_currentWaveIndex];

    }

    public void AdvanceWave()
    {
        _currentWaveIndex++;
        if(_waves.Length <= _currentWaveIndex)
        {
            Debug.Log("<b>Ressetting Waves</b>");
            _currentWaveIndex = 0;
            _currentWave = _waves[_currentWaveIndex];
            return;
        }
        Debug.Log($"<b>Advancing to wave: {_waves[_currentWaveIndex].name} </b>");
        _currentWave = _waves[_currentWaveIndex];
        SetRestInterval(_currentWave.WaveDuration);
    }

    public void AddNewChunk(ChunkTileMap chunk)
    {
        if(_chunks.Contains(chunk))return;
        Array.Resize<ChunkTileMap>(ref _chunks, _chunks.Length + 1);
        _chunks[_chunks.Length - 1] = chunk;
    }

    void SetRestInterval(float waveDuration)
    {
        _restInterval = 1f + ((3f + 1f * _currentWave.EnemiesToSpawn) - 50f / waveDuration);
    }
}
