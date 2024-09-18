using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWavePooler))]
public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField]World _currentWorld;
    [SerializeField] DifficultyScaler _difficultyScaler;
    [SerializeField]Transform _playerTransform, _spawnPoint;
    [SerializeField] float _innerRadius, _spawnRadius;
    int _spawnedEnemies, _killedEnemies, _globalEnemyKills, _maxEnemiesToKill;
    EnemyWavePooler _pool;
    [SerializeField]GameObject _portalFx;
    [SerializeField] float _portalFxDuration = 0.3f;
    NotMonoObjectPool _portalPool;
    //current wave stats
    //public SOEnemyWave _currentWave => _currentWorld.CurrentWave; //THIS IS PUBLIC ONLY FOR DEBUG PURPOSES(made for the DebugTestCurrentWave class) remove this later
    public SOEnemyWave _currentWave => _currentWorld.CurrentWave;
    private float _spawnCooldown => _currentWave.EnemySpawnCooldown; //
    private float _nextSpawnTime = 0f;
    private Vector3 _selectedSpawnpoint;

    bool _stopped;
    Timer _stopSpawningTimer, _spawnTimer;
    //private float _nextSpawnLocationCooldown = 0f;
    //SOEnemy[] _enemiesInfo => _currentWave.Enemies;

    public int GlobalEnemyKills => _globalEnemyKills;

    private void Awake()
    {
        gameObject.CheckComponent<EnemyWavePooler>(ref _pool);
        _pool.SetCurrentWorld(_currentWorld);
        _portalPool = new(60, _portalFx, this.transform, true);
        _stopSpawningTimer = new(0.1f);
        _stopSpawningTimer.Stop();
        _stopSpawningTimer.onEnd += ResumeSpawning;
        _nextSpawnTime = Time.time + 3f;
        _spawnTimer = new(_portalFxDuration);
        _spawnTimer.Stop();
        _spawnTimer.onEnd += DoSpawning;
        _killedEnemies = 1;
        _maxEnemiesToKill = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EnemyBrain.OnEnemyDeath += AddEnemyKill;
        if(_playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(_spawnPoint == null) _spawnPoint = _playerTransform;
        GameStateManager.OnStateSwitch += CheckGameState;
        ResumeSpawning();
        //_stopSpawningTimer.Start();
        //SelectSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        _stopSpawningTimer.UpdateTime();
        _spawnTimer.UpdateTime();
        if(_stopped) return;
        if (_nextSpawnTime < Time.time)
        {
            _nextSpawnTime = _difficultyScaler.CurrentStats.SpawnCooldown + _spawnCooldown + 0.1f + Time.time;
            if(_spawnedEnemies >= _currentWave.MaxEnemiesToSpawn)
            {
                _stopped = true;
            }
            _spawnTimer.Start();
        }
    }

    void AddEnemyKill()
    {
        _globalEnemyKills++;
        _killedEnemies++;
        if(_killedEnemies >= _maxEnemiesToKill)
        {
            _killedEnemies = 0;
            _spawnedEnemies = 0;
            //WAVE FINISHED!
        }
    }

    void DoSpawning()
    {
        int amountOfEnemiesToSpawn = Random.Range(1, _difficultyScaler.CurrentStats.EnemiesToSpawn + _currentWave.EnemiesToSpawn + 1);
        for (int i = 0; i < amountOfEnemiesToSpawn; i++)
        {
            CreateEnemy();
            _spawnedEnemies++;
        }

    }

    public void CreateEnemy()
    {
        SelectSpawnPosition();
        var enemies = _currentWave.Enemies;
        var data = enemies[Random.Range(0, enemies.Length)];
        var enemy = _pool.GetPooledObject();
        if(enemy.Value == null) return;
        
        enemy.Value.Initialize(data, _playerTransform);
        if(_currentWave.ChangeStatsOvertime) enemy.Value.Stats.AddDifficultyStats(_difficultyScaler.CurrentStats);

        enemy.Key.transform.SetParent(null); //aca setearlo al parent del objeto "Units" adentro de environment
        enemy.Key.transform.position = _selectedSpawnpoint + (Vector3)(Vector2.one * Random.Range(-1f, 1f));
        enemy.Key.SetActive(true);
    }
    public GameObject CreateEnemy(SOEnemy enemyData)
    {
        SelectSpawnPosition();
        var enemy = _pool.GetPooledObject();
        if(enemy.Value == null) return null;
        
        enemy.Value.Initialize(enemyData, _playerTransform);

        enemy.Key.transform.SetParent(null); //aca setearlo al parent del objeto "Units" adentro de environment
        enemy.Key.transform.position = _selectedSpawnpoint + (Vector3)(Vector2.one * Random.Range(-1f, 1f));
        enemy.Key.SetActive(true);
        return enemy.Key;
    }

    public void StopSpawning(float time)
    {
        _stopSpawningTimer.ChangeTime(time + 0.5f);
        _stopSpawningTimer.Start();
        _stopped = true;
    }
    public void StopSpawning()
    {
        _stopped = true;
    }


    public void ResumeSpawning()
    {
        if(_killedEnemies < _maxEnemiesToKill) return;
        _maxEnemiesToKill = _currentWave.MaxEnemiesToSpawn;
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
        //Do a nono circle around the player
        
        Vector2 radius = Random.insideUnitCircle * _spawnRadius;
        var xSign = Mathf.Sign(radius.x);
        var ySign = Mathf.Sign(radius.y);
        radius.x = Mathf.Clamp(radius.x, _innerRadius * xSign, _spawnRadius * xSign);
        radius.y = Mathf.Clamp(radius.y, _innerRadius * ySign, _spawnRadius * ySign);

        _selectedSpawnpoint = _spawnPoint.position + (Vector3)radius;
        SpawnPortalFx(_selectedSpawnpoint);
    }
    void SpawnPortalFx(Vector3 position)
    {
        GameObject portal = _portalPool.GetObjectFromPool();
        portal.transform.position = position;
        portal.SetActive(true);
    }

    private void OnDestroy()
    {
        EnemyBrain.OnEnemyDeath -= AddEnemyKill;
        GameStateManager.OnStateSwitch -= CheckGameState;
        _spawnTimer.onEnd -= DoSpawning;
        _stopSpawningTimer.onEnd -= ResumeSpawning;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_spawnPoint.position, _innerRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(_spawnPoint.position, _spawnRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_selectedSpawnpoint, 1f);
    }
}
