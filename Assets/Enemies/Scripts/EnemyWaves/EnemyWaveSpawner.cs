using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyWavePooler))]
public class EnemyWaveSpawner : MonoBehaviour
{

    [SerializeField] WaveSystem _waveSys;
    [SerializeField] DifficultyScaler _difficultyScaler;
    [SerializeField] Transform _playerTransform, _spawnPoint;
    [SerializeField] float _innerRadius, _spawnRadius;
    int _spawnedEnemies, _killedEnemies, _globalEnemyKills, _maxEnemiesToKill;
    EnemyWavePooler _pool;
    [SerializeField]GameObject _portalFx;
    [SerializeField] float _portalFxDuration = 0.3f;
    NotMonoObjectPool _portalPool;
    private SOEnemyWave _currentWave;
    private float _nextSpawnTime = 0f;
    private Vector3 _selectedSpawnpoint;

    bool _stopped;
    Timer _spawnTimer;
    //private float _nextSpawnLocationCooldown = 0f;
    //SOEnemy[] _enemiesInfo => _currentWave.Enemies;
    public int PlayerKills => _killedEnemies;
    public int MaxEnemiesToKill => _maxEnemiesToKill;
    public int GlobalEnemyKills => _globalEnemyKills;
    private int SpawnedEnemies
    {
        get => _spawnedEnemies;
        set
        {
            _spawnedEnemies = value;
            if(_spawnedEnemies >= _currentWave.MaxEnemiesToSpawn) _stopped = true;
        }
    }

    private void Awake()
    {
        gameObject.CheckComponent<EnemyWavePooler>(ref _pool);
        _portalPool = new(60, _portalFx, this.transform, true);
        _nextSpawnTime = Time.time + 1f;
        _spawnTimer = new(_portalFxDuration);
        _spawnTimer.Stop();
        _spawnTimer.onEnd += DoSpawning;
        _killedEnemies = 0;
        _maxEnemiesToKill = 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _waveSys.OnWaveChange += NextWaveCheck;
        EnemyBrain.OnEnemyDeath += AddEnemyKill;
        if(_playerTransform == null) _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(_spawnPoint == null) _spawnPoint = _playerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_waveSys.Enabled) return;
        _spawnTimer.UpdateTime();
        if(_stopped) return;
        if (_nextSpawnTime < Time.time)
        {
            _nextSpawnTime = _difficultyScaler.CurrentStats.SpawnCooldown + _currentWave.EnemySpawnCooldown + Time.time;
            if(_nextSpawnTime <= 0) _nextSpawnTime = Time.time + 0.055f;
            _spawnTimer.Start();
        }
    }

    void AddEnemyKill()
    {
        _globalEnemyKills++;
        _killedEnemies++;
        if(_killedEnemies >= _maxEnemiesToKill)
        {
            //WAVE FINISHED!
            SpawnedEnemies = 0;
            _killedEnemies = 0;
            _waveSys.AdvanceWave();
        }
    }

    void DoSpawning()
    {
        int amountOfEnemiesToSpawn = Random.Range(1, _difficultyScaler.CurrentStats.EnemiesToSpawn + _currentWave.EnemiesToSpawn + 1);
        for (int i = 0; i < amountOfEnemiesToSpawn; i++)
        {
            CreateEnemy();
        }

    }

    public void CreateEnemy()
    {
        if(_stopped) return;
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
        SpawnedEnemies++;
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



    public void ResumeSpawning()
    {
        //if(_killedEnemies < _maxEnemiesToKill) return;
        
        _stopped = false;
    }

    void NextWaveCheck(SOEnemyWave newWave)
    {
        Debug.Log("Spawning new wave");
        _currentWave = newWave;
        _maxEnemiesToKill = _currentWave.MaxEnemiesToSpawn;
        ResumeSpawning();
    }

    void SelectSpawnPosition()
    {
        //Do a nono circle around the player
        //player position + half inneradius + randomunitinside spawn radius. change the inner radius x,y and sign depending on the ramdom insideunitcircle
        Vector2 radius = Random.insideUnitCircle * _spawnRadius;
        var xSign = System.Math.Sign(radius.x);
        var ySign = System.Math.Sign(radius.y);
        var dirFromPlayer = (Vector3)radius - _playerTransform.position;


        //_selectedSpawnpoint = _spawnPoint.position + (Vector3)radius;
        _selectedSpawnpoint = (Vector3)radius + dirFromPlayer.normalized * _innerRadius;
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
        _waveSys.OnWaveChange -= NextWaveCheck;
        EnemyBrain.OnEnemyDeath -= AddEnemyKill;
        _spawnTimer.onEnd -= DoSpawning;
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
