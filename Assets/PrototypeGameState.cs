using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PrototypeGameState : MonoBehaviour
{
    [SerializeField] FadeImage _fadeEffect;
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    [SerializeField] GameObject _overworldParent, _otherSideParent;
    [SerializeField] PlayerStatsManager _playerStatsManager;
    [SerializeField] int _wavesToFace = 3;
    [SerializeField] float _restTime = 30f, _spawnCooldown, _transitionDuration = 0.4f;
    [SerializeField] StoreMenu _store;
    float _nextSpawnTime, _transitionElapsedTime, _lensDistFXDefault;
    bool _resting;
    int _wavesPassed;
    Timer _restTimer, _storeOpeningTimer;

    private void Awake()
    {
        _wavesPassed = _wavesToFace + 1;
        _storeOpeningTimer = new(_transitionDuration + 0.5f);
        _storeOpeningTimer.Stop();
        _storeOpeningTimer.onEnd += OpenStore;
        _restTimer = new(_restTime);
        _restTimer.Stop();
        _restTimer.onEnd += StartWaves;
        if(!_waveSpawner.gameObject.activeInHierarchy) _waveSpawner.gameObject.SetActive(true);
        _waveSpawner.StopSpawning();
        GameStateManager.OnStateSwitch += WavePassed;
    }

    private void OpenStore()
    {
        //_store.OpenMenu();
    }

    private void Start() {
        _otherSideParent.SetActive(true);
        _overworldParent.SetActive(false);
        _waveSpawner.gameObject.SetActive(false);
        _lensDistFXDefault = PostProcessingManager.GetDefaultValue<LensDistortion>();
    }

    private void Update() {
        if(_transitionElapsedTime >= 0)
        {
            _transitionElapsedTime -= Time.deltaTime;
            var percent = _transitionElapsedTime / _transitionDuration;
            var fxIntensity = Mathf.Lerp(_lensDistFXDefault, 1f, percent);
            PostProcessingManager.SetLensDistortion(fxIntensity);
            return;
        }

        _storeOpeningTimer.UpdateTime();
        _restTimer.UpdateTime();
        if(_resting) return;
        if(_nextSpawnTime > Time.time) return;
        SpawnEnemies();
    }

    void WavePassed(GameStateBase state)
    {
        if(_resting) return;
        if(_wavesPassed >= _wavesToFace)
        {
            _resting = true;
            _restTimer.Start();
            _storeOpeningTimer.Start();
            KillEnemies();
            ChangeWorld();
            _wavesPassed = 0;
            return;
        }
        _wavesPassed++;
    }

    void KillEnemies()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyBrain>();
        foreach(EnemyBrain enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    void SpawnEnemies()
    {
        _nextSpawnTime = Time.time + _spawnCooldown;
        _waveSpawner.CreateEnemy();
    }

    void StartWaves()
    {
        //if(!_waveSpawner.gameObject.activeInHierarchy) _waveSpawner.gameObject.SetActive(true);
        _resting = false;
        _nextSpawnTime = Time.time + 2f;
        ChangeWorld();
    }

    void ChangeWorld()
    {
        var baseSpeed = _playerStatsManager.GetBaseStat(StatsTypes.Speed);
        var speedDecrease = -baseSpeed / 2.25f;
        var speedIncrease = baseSpeed * 0.1f;
        if(_resting)
        {
            if(_wavesPassed != _wavesToFace+1) _playerStatsManager.SetStat(StatsTypes.Speed, -speedDecrease + speedIncrease);
        }
        else
        {
            _playerStatsManager.SetStat(StatsTypes.Speed, speedDecrease - speedIncrease);
        }
        _transitionElapsedTime = _transitionDuration;
        CameraShake.Shake(2f, _transitionDuration + 0.2f);
        _fadeEffect.FadeIn(() => 
        {
            _otherSideParent.SetActive(!_otherSideParent.activeInHierarchy);
            _overworldParent.SetActive(!_overworldParent.activeInHierarchy);
            _fadeEffect.FadeOut();
        });
    }

    private void OnDestroy() {
        GameStateManager.OnStateSwitch -= WavePassed;
        _restTimer.onEnd -= StartWaves;
        _storeOpeningTimer.onEnd -= OpenStore;
    }
}
