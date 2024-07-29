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
    Camera _mainCamera;
    Vector3 _lastPlayerPosition = Vector3.zero;

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
        //_otherSideParent.SetActive(true);
        //_overworldParent.SetActive(false);
        _waveSpawner.gameObject.SetActive(false);
        _lensDistFXDefault = PostProcessingManager.GetDefaultValue<LensDistortion>();
        _mainCamera = Camera.main;
    }

    private void Update() {
        if(_transitionElapsedTime >= 0) // la distorsion deberia ser un fade in y fade out! (de -1 a 1 y devuelta a 0 o valor default)
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

        //SpawnEnemies();
    }

    void WavePassed(GameStateBase state)
    {
        if(_resting) return;
        CheckEnemies();
        if(_wavesPassed >= _wavesToFace)
        {
            Rest();
            return;
        }
        _wavesPassed++;
    }

    void Rest()
    {
        _resting = true;
        _restTimer.Start();
        _storeOpeningTimer.Start();
        KillEnemies();
        ChangeWorld();
        _wavesPassed = 0;
    }

    void CheckEnemies()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyBrain>();
        bool allDead = true;
        foreach(EnemyBrain enemy in enemies)
        {
            if(enemy.gameObject.activeInHierarchy) allDead = false;
        }
        if(allDead) Rest();
    }

    public void KillEnemies()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyBrain>();
        foreach(EnemyBrain enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    void SpawnEnemies()
    {
        if(_nextSpawnTime > Time.time) return;
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

    public void ChangeWorld() //esto es un change state de manual. 
    {
        //SpeedChange();
        _transitionElapsedTime = _transitionDuration;
        CameraShake.Shake(2f, _transitionDuration + 0.2f);
        _fadeEffect.FadeIn(() => 
        {
            _otherSideParent.SetActive(!_otherSideParent.activeInHierarchy);
            _overworldParent.SetActive(!_overworldParent.activeInHierarchy);
            //SetPlayerPosition(); // this should be in the middle of the fade in and not when it finishes!
            _fadeEffect.FadeOut();
        });
    }

    void SpeedChange()
    {
        var baseSpeed = _playerStatsManager.GetBaseStat(StatsTypes.Speed);
        var speedDecrease = -baseSpeed / 12.5f;
        var speedIncrease = baseSpeed * 0.1f;
        if(_resting)
        {
            _playerStatsManager.SetStat(StatsTypes.Speed, -speedDecrease + speedIncrease);
        }
        else
        {
            _playerStatsManager.SetStat(StatsTypes.Speed, speedDecrease - speedIncrease);
        }
    }

    void SetPlayerPosition()
    {
        var currentPosition = _playerStatsManager.transform.position;
        if(_lastPlayerPosition != Vector3.zero) 
        {
            _playerStatsManager.transform.position = _lastPlayerPosition;
            if(_mainCamera != null) _mainCamera.transform.position = _lastPlayerPosition - (Vector3.forward * 10f);
        }
        _lastPlayerPosition = currentPosition;
    }

    private void OnDestroy() {
        GameStateManager.OnStateSwitch -= WavePassed;
        _restTimer.onEnd -= StartWaves;
        _storeOpeningTimer.onEnd -= OpenStore;
    }
}
