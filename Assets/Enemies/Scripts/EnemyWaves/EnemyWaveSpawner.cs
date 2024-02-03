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
    NotMonoObjectPool _portalPool;
    //current wave stats
    public SOEnemyWave _currentWave => _currentWorld.CurrentWave; //THIS IS PUBLIC ONLY FOR DEBUG PURPOSES(made for the DebugTestCurrentWave class) remove this later
    private float _spawnCooldown => _currentWave.EnemySpawnCooldown;
    private float _nextSpawnTime = 0f;
    private Vector3 _selectedSpawnpoint;

    bool _stopped;
    Timer _stopSpawningTimer;
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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if(_playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _nextSpawnTime = _spawnCooldown;

        GameStateManager.OnRestStart += StopSpawning;
        _stopSpawningTimer.onEnd += ResumeSpawning;

        SelectSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        _stopSpawningTimer.UpdateTime();
        if(_stopped) return;
        if (_nextSpawnTime < Time.time)
        {
            _nextSpawnTime = _spawnCooldown + Time.time;
            SelectSpawnPosition();
            SpawnPortalFx(_selectedSpawnpoint);
            int amountOfEnemiesToSpawn = Random.Range(1, _currentWave.EnemiesToSpawn + 1);
            for (int i = 0; i < amountOfEnemiesToSpawn; i++)
            {
                StartCoroutine(Spawn());
            }

        }
    }

    IEnumerator Spawn()
    {
        yield return null;

        var data = _enemiesInfo[Random.Range(0, _enemiesInfo.Length)];
        var enemy = _pool.GetPooledObject();
        if(enemy.Value == null) yield break;
        
        var targetProvider = enemy.Key.GetComponent<ITargetPositionProvider>();
        targetProvider.TargetTransform = _playerTransform;
        enemy.Value.Initialize(data);

        enemy.Key.transform.SetParent(null); //aca setearlo al parent del objeto "Units" adentro de environment
        enemy.Key.transform.position = _selectedSpawnpoint + (Vector3)(Vector2.one * Random.Range(-0.8f, 0.8f));
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
        GameStateManager.OnRestStart -= StopSpawning;
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
