using System;
using UnityEngine;

public class Timer
{
    float _countdownTime = 1f;
    float _timeRemaining;
    bool _resetOnZero;
    private bool _timerStopped = false;
    bool _useUnscaledTime = false;
    public event Action onStart;
    public event Action onEnd;

    public float CurrentTime => _timeRemaining;

    public Timer(float countdownTime, bool resetOnZero = true, bool useUnscaledTime = false)
    {
        _countdownTime = countdownTime;
        _timeRemaining = _countdownTime;
        _resetOnZero = resetOnZero;
        _useUnscaledTime = useUnscaledTime;
        _timerStopped = false;
    }


    public void UpdateTime()
    {
        if(_timerStopped)return;
        if(_timeRemaining > 0)
        { 
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
        _timeRemaining = _countdownTime;
        _timerStopped = false;
        onEnd?.Invoke();
    }

    public void ResetTime()
    {
        _timeRemaining = _countdownTime;
    }

    public void Start()
    {
        _timeRemaining = _countdownTime;
        onStart?.Invoke();    
        Resume();
    }
    public void Resume()
    {
        _timerStopped = false;
    }

    public void Stop()
    {
        _timerStopped = true;
    }
}
