using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    //[SerializeField]TextMeshProUGUI _timerText;
    float _currentRiftTime;
    public static event Action onRiftTimerEnd;
    float _waveIntervalTime;
    public static event Action onWaveIntervalEnd;
    float _restInterval;
    public static event Action<float> OnRestStart;
    [SerializeField]World _currentWorld;

    public float CurrentRiftTime => _currentRiftTime;
    public float CurrentWaveTime => _waveIntervalTime;
    // Start is called before the first frame update
    void Start()
    {
        _currentRiftTime = World.RiftDurationInSeconds;
        _waveIntervalTime = _currentWorld.WavesInterval;
        _restInterval = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentRiftTime <= 0)
        {
            _currentRiftTime = 0;
            //also advance the world into a new scene or fire an event?
            onRiftTimerEnd?.Invoke();
        }
        UpdateTimer();
        UpdateWaveInterval();
        UpdateRestInterval();
    }

    void UpdateTimer()
    {
        _currentRiftTime -= 1 * Time.deltaTime;
    }

    void UpdateWaveInterval()
    {
        if(_restInterval > 0) return;
        _waveIntervalTime -= Time.deltaTime;
        if(_waveIntervalTime <= 0)
        {
            //_waveIntervalTime = _currentWorld.WavesInterval;
            _restInterval = _currentWorld.RestInterval;
            Debug.Log("Starting Rest");
            OnRestStart?.Invoke(_restInterval);
            onWaveIntervalEnd?.Invoke();
        }
    }

    void UpdateRestInterval()
    {
        if(_waveIntervalTime > 0) return;
        _restInterval -= Time.deltaTime;
        if(_restInterval <= 0)
        {
            _restInterval = 0;
            _waveIntervalTime = _currentWorld.WavesInterval;
        }
    }


}
