using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField] World _currentWorld;
    bool _enabled = false;
    SOEnemyWave _currentWave;
    public event Action<SOEnemyWave> OnWaveChange;
    private int _currentWaveIndex = -1;
    float _elapsedWaveTime = 0f;

    //properties

    public int WaveNumber => _currentWaveIndex;
    public float ElapsedWaveTime => _elapsedWaveTime;
    public bool Enabled => _enabled;

    private void Awake() {
        _enabled = false;
        _currentWaveIndex = -1;
    }

    private IEnumerator Start() {
        yield return null;
        //StartWaves();
    }

    public void StartWaves()
    {
        AdvanceWave();
        _enabled = true;
    }
    public void AdvanceWave()
    {
        _elapsedWaveTime = 0f;
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
        if(!_enabled) return;
        _elapsedWaveTime += Time.deltaTime;
    }

    public void ResumeWaves()
    {
        _enabled = true;
    }

    public void StopWaves()
    {
        _enabled = false;
    }



}
