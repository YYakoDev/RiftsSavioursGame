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
    [SerializeField]float _restInterval = 3f;
    [SerializeField]private SOEnemyWave[] _waves;
    [SerializeField]private ChunkTileMap[] _chunks = new ChunkTileMap[0];

    public SOEnemyWave[] Waves => _waves;
    public float RestInterval => _restInterval;
    public ChunkTileMap[] Chunks => _chunks;

    public void Initialize(World world)
    {
        _name = world._name;
        _waves = world._waves.Clone() as SOEnemyWave[];
        _chunks = world._chunks.Clone() as ChunkTileMap[];
        if(_waves.Length <= 0)
        {
            Debug.LogError($"<b> No waves found in the World: {_name} </b>");
            return;
        }
    }


    public void AddNewChunk(ChunkTileMap chunk)
    {
        if(_chunks.Contains(chunk))return;
        Array.Resize<ChunkTileMap>(ref _chunks, _chunks.Length + 1);
        _chunks[_chunks.Length - 1] = chunk;
    }

}
