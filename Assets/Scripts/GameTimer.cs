using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    //[SerializeField]TextMeshProUGUI _timerText;
    [SerializeField]Image _timerImage;
    float _currentRiftTime;
    public static event Action onRiftTimerEnd;
    float _waveIntervalTime;
    public static event Action onWaveIntervalEnd;
    [SerializeField]World _currentWorld;
    // Start is called before the first frame update
    void Start()
    {
        _currentRiftTime = _currentWorld.RiftDurationInSeconds;
        _waveIntervalTime = _currentWorld.WavesInterval;
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
    }

    void UpdateTimer()
    {
        _currentRiftTime -= 1 * Time.deltaTime;
        float fillAmount = _currentRiftTime / _currentWorld.RiftDurationInSeconds;
        _timerImage.fillAmount = fillAmount;
    }

    void UpdateWaveInterval()
    {
        if(_waveIntervalTime <= 0)
        {
            _waveIntervalTime = _currentWorld.WavesInterval;
            onWaveIntervalEnd?.Invoke();
        }
        _waveIntervalTime -= 1 * Time.deltaTime;
    }


}
