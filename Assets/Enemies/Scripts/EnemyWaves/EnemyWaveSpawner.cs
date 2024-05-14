using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWavePooler))]
public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField]World _currentWorld;
    [SerializeField]Transform _playerTransform;
    [SerializeField] float _innerRadius, _spawnRadius;
    EnemyWavePooler _pool;
    [SerializeField]GameObject _portalFx;
    [SerializeField] float _portalFxDuration = 0.3f;
    NotMonoObjectPool _portalPool;
    //current wave stats
    public SOEnemyWave _currentWave => _currentWorld.CurrentWave; //THIS IS PUBLIC ONLY FOR DEBUG PURPOSES(made for the DebugTestCurrentWave class) remove this later
    private float _spawnCooldown => _currentWave.EnemySpawnCooldown;
    private float _nextSpawnTime = 0f;
    private Vector3 _selectedSpawnpoint;

    bool _stopped;
    Timer _stopSpawningTimer, _spawnTimer;
    //private float _nextSpawnLocationCooldown = 0f;
    SOEnemy[] _enemiesInfo => _currentWave.Enemies;

    //also you will need to pass all the stats from that wave to the enemies to spawn

    private void Awake()
    {
        gameObject.CheckComponent<EnemyWavePooler>(ref _pool);
        _pool.SetCurrentWorld(_currentWorld);
        _portalPool = new(60, _portalFx, this.transform, true);
        _stopSpawningTimer = new(0.1f);
        _stopSpawningTimer.Stop();
        _nextSpawnTime = Time.time + 3f;
        _spawnTimer = new(_portalFxDuration);
        _spawnTimer.Stop();
        _spawnTimer.onEnd += DoSpawning;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(_playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        GameStateManager.OnStateSwitch += CheckGameState;
        _stopSpawningTimer.onEnd += ResumeSpawning;
        _stopSpawningTimer.Start();
        SelectSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        _stopSpawningTimer.UpdateTime();
        _spawnTimer.UpdateTime();
        if(_stopped) return;
        if (_nextSpawnTime < Time.time)
        {
            _nextSpawnTime = _spawnCooldown + 0.15f + Time.time;
            SelectSpawnPosition();
            SpawnPortalFx(_selectedSpawnpoint);
            _spawnTimer.Start();
        }
    }

    void DoSpawning()
    {
        int amountOfEnemiesToSpawn = Random.Range(1, _currentWave.EnemiesToSpawn + 1);
        for (int i = 0; i < amountOfEnemiesToSpawn; i++)
        {
            CreateEnemy();
        }

    }

    void CreateEnemy()
    {
        var data = _enemiesInfo[Random.Range(0, _enemiesInfo.Length)];
        var enemy = _pool.GetPooledObject();
        if(enemy.Value == null) return;
        
        enemy.Value.Initialize(data, _playerTransform);

        enemy.Key.transform.SetParent(null); //aca setearlo al parent del objeto "Units" adentro de environment
        enemy.Key.transform.position = _selectedSpawnpoint + (Vector3)(Vector2.one * Random.Range(-1f, 1f));
        enemy.Key.SetActive(true);
    }

    void StopSpawning(float time)
    {
        _stopSpawningTimer.ChangeTime(time + 0.5f);
        _stopSpawningTimer.Start();
        _stopped = true;
    }

    void ResumeSpawning()
    {
        _stopped = false;
    }

    void CheckGameState(GameStateBase state)
    {
        if(state.GetType() == typeof(RestState))
        {
            StopSpawning(_currentWorld.RestInterval);
        }
    }

    void SelectSpawnPosition()
    {
        //do the bag spawning like tetris
        Vector2 radius = Random.insideUnitCircle * _spawnRadius;
        radius.x = Mathf.Clamp(radius.x, _innerRadius, _spawnRadius) * Mathf.Sign(radius.x);
        radius.y = Mathf.Clamp(radius.y, _innerRadius, _spawnRadius) * Mathf.Sign(radius.y);
        _selectedSpawnpoint = _playerTransform.position + (Vector3)radius;

    }
    void SpawnPortalFx(Vector3 position)
    {
        GameObject portal = _portalPool.GetObjectFromPool();
        portal.transform.position = position;
        portal.SetActive(true);
    }

    private void OnDestroy()
    {
        GameStateManager.OnStateSwitch -= CheckGameState;
        _spawnTimer.onEnd -= DoSpawning;
        _stopSpawningTimer.onEnd -= ResumeSpawning;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_playerTransform.position, _innerRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_playerTransform.position, _spawnRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_selectedSpawnpoint, 1f);
    }
}
