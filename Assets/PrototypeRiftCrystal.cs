using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRiftCrystal : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 3;
    [SerializeField] float _spawnRadius, _timeToRegenerate;
    [SerializeField] RunManager _runManager;
    [SerializeField] SpriteRenderer _fxRenderer;
    [SerializeField] EnemyWaveSpawner _enemySpawner;
    int _currentHealth;
    float _waveDuration = 13f;
    bool _doSpawning;
    Timer _regenTimer, _spawningTimer;


    [SerializeField] WhiteBlinkEffect _blinkFx;

    // audio stuff
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _hitSfx;

    private void Awake()
    {
        _regenTimer = new(_timeToRegenerate);
        _regenTimer.Stop();
        _regenTimer.onEnd += Regenerate;
        _spawningTimer = new(1f, true);
        _spawningTimer.Stop();
        _spawningTimer.onEnd += SpawnEnemy;
        _currentHealth = _maxHealth;
    }

    void Start()
    {
        _runManager.StartRun();
        ActivateCrystal();
    }

    // Update is called once per frame
    void Update()
    {
        _regenTimer.UpdateTime();
        if(_doSpawning)
        {
            //_waveDuration -= Time.deltaTime;
            _spawningTimer.UpdateTime();
            if(_waveDuration <= 0)
            {
                _doSpawning = false;
                _waveDuration = 13f;
                //_regenTimer.Start();
            }
        }
    }
    
    void ActivateCrystal()
    {
        _spawningTimer.Start();
        _doSpawning = true;
        _enemySpawner.ResumeSpawning();
        PlayEffects();
    }

    void SpawnEnemy()
    {

        _enemySpawner.CreateEnemy(transform.position + (Vector3)Random.insideUnitCircle * _spawnRadius);

    }

    void PlayEffects()
    {
        _fxRenderer.gameObject.SetActive(true);
    }
    void Regenerate()
    {
        /*_fxRenderer.gameObject.SetActive(false);
        _currentHealth = _maxHealth;
        _blinkFx.Play();
        _doSpawning = false;*/
        _currentHealth = _maxHealth;
        PlayEffects();
        _doSpawning = true;
    }

    public void TakeDamage(int damage)
    {
        if(_currentHealth <= 0) return;
        _audio.PlayOneShot(_hitSfx);
        _blinkFx.Play();
        _currentHealth -= 1;
        if(_currentHealth <= 0) Die();
    }
    public void Die()
    {
        if(!_runManager.RunStarted)
        {
            _enemySpawner.ResumeSpawning();
            _runManager.StartRun();
        }
        else
        {
        }
        //ActivateCrystal();
        _fxRenderer.gameObject.SetActive(false);
        //_currentHealth = _maxHealth;
        _blinkFx.Play();
        _doSpawning = false;
        _regenTimer.Start();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }

    private void OnDestroy() {
        _regenTimer.onEnd -= Regenerate;
        _spawningTimer.onEnd -= SpawnEnemy;
    }
}
