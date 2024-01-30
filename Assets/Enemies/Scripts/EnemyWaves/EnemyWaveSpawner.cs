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
            int amountOfEnemiesToSpawn = Random.Range(1, _currentWave.EnemiesToSpawn);
            for (int i = 0; i < amountOfEnemiesToSpawn; i++)
            {
                StartCoroutine(Spawn());
            }

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
        var list = enemy.GetComponents<ITargetPositionProvider>();
        if(list != null) foreach(ITargetPositionProvider targetData in list) targetData.TargetTransform = _playerTransform;
        
        //remove its parent and set the position to a random spawn point

        enemy.transform.position = _selectedSpawnpoint + (Vector3)(Vector2.one * Random.Range(-0.8f, 0.8f));
        enemy.transform.SetParent(null); //aca setearlo al parent del objeto "Units" adentro de environment
        enemy.SetActive(true);
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
