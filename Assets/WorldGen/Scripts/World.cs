using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ScriptableObjects/World")]
public class World : ScriptableObject
{
    [SerializeField]private string _name;
    public const float RiftDuration = 325f; // 5mins, 25 seconds
    //public const float RiftDuration = 5f; // just for debugging
    [SerializeField]private float _wavesInterval = 30f; // seconds
    [SerializeField]private SOEnemyWave[] _waves;
    [SerializeField]private EnemyBrain[] _enemyPrefabs;
    [SerializeField]private List<Tilemap> _chunks = new List<Tilemap>();
    [SerializeField]private CraftingMaterial[] _worldCraftingMaterials;

    private SOEnemyWave _currentWave;
    private int _currentWaveIndex = 0;

    

    //properties
    public float RiftDurationInSeconds => RiftDuration;
    public float WavesInterval => _wavesInterval;
    public SOEnemyWave CurrentWave => _currentWave;
    public EnemyBrain[] EnemyPrefabs => _enemyPrefabs;
    public List<Tilemap> Chunks => _chunks;
    public CraftingMaterial[] CurrentCraftingMaterials => _worldCraftingMaterials;

    public void Initialize(World world)
    {
        _name = world._name;
        _wavesInterval = world._wavesInterval;
        _waves = world._waves;
        _chunks = world._chunks;
        _enemyPrefabs = world._enemyPrefabs;
        _currentWaveIndex = 0;
        if(_waves.Length <= _currentWaveIndex)
        {
            Debug.LogError($"<b> No waves found in the World: {_name} </b>");
            return;
        }
        _currentWave = _waves[_currentWaveIndex];
        _worldCraftingMaterials = world._worldCraftingMaterials;

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
    }
}
