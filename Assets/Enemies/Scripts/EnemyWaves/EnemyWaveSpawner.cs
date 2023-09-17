using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWavePooler))]
public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField]Transform[] _spawnPoints;
    [SerializeField]World _currentWorld;
    [SerializeField]Transform _playerTransform;
    EnemyWavePooler _pool;

    //current wave stats
    public SOEnemyWave _currentWave => _currentWorld.CurrentWave; //THIS IS PUBLIC ONLY FOR DEBUG PURPOSES(made for the DebugTestCurrentWave class) remove this later
    private float _spawnCooldown => _currentWave.EnemySpawnCooldown;
    private float _nextSpawnTime = 0f;
    private Transform _selectedSpawnpoint;
    private float _nextSpawnLocationCooldown = 0f;


    //also you will need to pass all the stats from that wave to the enemies to spawn
    
    private void Awake()
    {
        gameObject.CheckComponent<EnemyWavePooler>(ref _pool);
        _pool.SetCurrentWorld(_currentWorld);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(_playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _nextSpawnTime = _spawnCooldown;
        GameTimer.onWaveIntervalEnd += _currentWorld.AdvanceWave;
        SelectSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(_nextSpawnTime < Time.time)
        {
            _nextSpawnTime = Time.time + _spawnCooldown;
            int amountOfEnemiesToSpawn = Random.Range(2, _currentWave.EnemiesToSpawn);

            for(int i = 0; i<amountOfEnemiesToSpawn; i++)
            {
                StartCoroutine(Spawn());
            }
        }
        if(_nextSpawnLocationCooldown < Time.time)
        {
            SelectSpawnPosition();
        }
    }

    IEnumerator Spawn()
    {
        bool signatureLength = _currentWave.EnemiesSignatures.Length <= 0;
        bool prefabLength = _currentWorld.EnemyPrefabs.Length <= 0;
        if(signatureLength || prefabLength)
        {
            if(signatureLength)Debug.LogError($"<b> No enemy SIGNATURE found in the wave: {_currentWave.name} </b>");
            if(prefabLength)Debug.LogError($"<b> No enemy PREFABS found in the world: {_currentWorld.name} </b>");
            yield break;
        }
        yield return null;

        GameObject enemy = _pool.GetPooledObject();
        if(enemy == null) yield break;
        
        //i know all this comments are kinda pointless because the code speaks for itself
        //set the target in the avoidance data script of the enemy (if there is any)
        if(enemy.TryGetComponent<ITargetPositionProvider>(out ITargetPositionProvider targetData))
        {
            targetData.TargetTransform = _playerTransform;
        }
        //remove its parent and set the position to a random spawn point

        enemy.transform.position = _selectedSpawnpoint.position + (Vector3)(Vector2.one * Random.Range(-1f, 1f));
        enemy.transform.SetParent(null);
        enemy.SetActive(true);
        
    }

    void SelectSpawnPosition()
    {
        //do the bag spawning like tetris
        _nextSpawnLocationCooldown = Time.time + Random.value * 0.1f;
        _selectedSpawnpoint = GetSpawnPoint();

        Transform GetSpawnPoint()
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        }

    }

    private void OnDestroy()
    {
        GameTimer.onWaveIntervalEnd -= _currentWorld.AdvanceWave;
    }
}
