using System;
using UnityEngine;

public class Timer
{
    float _countdownTime = 1f;
    float _timeRemaining;
    public event Action onTimerStart;
    public event Action onReset;

    private bool _timerStopped = false;
    public Timer(float countdownTime)
    {
        _countdownTime = countdownTime;
        _timeRemaining = _countdownTime;
    }


    public void TimerUpdate()
    {
        if(_timerStopped)return;
        if(_timeRemaining > 0)
        {
            onTimerStart?.Invoke();
            _timeRemaining -= Time.deltaTime;
        }else
        {
            _timeRemaining = _countdownTime;
            onReset?.Invoke();
        }
    }

    public void SetTimerActive(bool state)
    {
        _timerStopped = !state;
    }
}
