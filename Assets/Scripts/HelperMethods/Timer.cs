using System;
using UnityEngine;

public class Timer
{
    float _countdownTime = 1f;
    float _timeRemaining;
    bool _resetOnZero;
    private bool _timerStopped = false;
    bool _useUnscaledTime = false;
    public event Action onTimerStart;
    public event Action onReset;

    public Timer(float countdownTime, bool resetOnZero = true, bool useUnscaledTime = false)
    {
        _countdownTime = countdownTime;
        _timeRemaining = _countdownTime;
        _resetOnZero = resetOnZero;
        _useUnscaledTime = useUnscaledTime;
    }


    public void UpdateTime()
    {
        if(_timerStopped)return;
        if(_timeRemaining > 0)
        {
            onTimerStart?.Invoke();     
            if(_useUnscaledTime) _timeRemaining -= Time.unscaledDeltaTime;
            else _timeRemaining -= Time.deltaTime;
        }else
        {
            Restart();    
            if(!_resetOnZero) _timerStopped = true;
        }
    }
    public void ChangeTime(float newTime)
    {
        _countdownTime = newTime;
    }

    public void Restart()
    {
        _timerStopped = false;
        _timeRemaining = _countdownTime;
        onReset?.Invoke();
    }

    public void SetActive(bool state)
    {
        _timerStopped = !state;
        _timeRemaining = _countdownTime;
    }
}
