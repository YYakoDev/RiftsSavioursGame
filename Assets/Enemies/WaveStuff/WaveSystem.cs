using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] World _currentWorld;
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    [SerializeField] DifficultyScaler _difficultyScaler;
    bool _enabled = false;
    SOEnemyWave _currentWave;
    public event Action<SOEnemyWave> OnWaveChange;
    public event Action OnWaveEnd;
    private int _currentWaveIndex = -1, _currentKillCount = 0;
    float _elapsedRestTime = 0f;

    //properties

    public SOEnemyWave CurrentWave => _currentWave;
    public int WaveNumber => _currentWaveIndex;
    //public float ElapsedWaveTime => _elapsedWaveTime;
    public bool Enabled => _enabled;

    private void Awake() {
        _enabled = false;
        _currentWaveIndex = -1;
    }

    private void Start() 
    {
        EnemyBrain.OnEnemyDeath += CheckEnemyKills;    
        //StartWaves();
    }

    public void StartWaves()
    {
        AdvanceWave();
        _enabled = true;
    }
    public void AdvanceWave()
    {
        _currentWaveIndex++;
        if(_currentWorld.Waves.Length <= _currentWaveIndex)
        {
            Debug.Log("<b>Ressetting Waves</b>");
            _currentWaveIndex = 0;
        }
        _currentWave = _currentWorld.Waves[_currentWaveIndex];
        OnWaveChange?.Invoke(_currentWave);

        //Debug.Log($"<b>Advancing to wave: {_waves[_currentWaveIndex].name} </b>");
    }

    private void Update() 
    {
        if(_elapsedRestTime > 0f)
        {
            _elapsedRestTime -= Time.deltaTime;
            if(_elapsedRestTime <= 0)
            {
                _enabled = true;
            }
        }
        
        if(!_enabled) return;
    }

    void CheckEnemyKills()
    {
        _currentKillCount++;
        if(_currentKillCount >= _waveSpawner.MaxEnemiesToKill)
        {
            Debug.Log("Wave ended");
            OnWaveEnd?.Invoke();
            _currentKillCount = 0;
        }
    }


    public void ResumeWaves()
    {
        //_enabled = true;
        _elapsedRestTime = _currentWorld.RestInterval + _difficultyScaler.CurrentStats.WavesRestTime;
    }

    public void StopWaves()
    {
        _enabled = false;
    }

    private void OnDestroy() {
        EnemyBrain.OnEnemyDeath -= CheckEnemyKills;
    }

}
