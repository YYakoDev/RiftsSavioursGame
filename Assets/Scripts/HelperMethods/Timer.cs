using System;
using UnityEngine;

public class Timer
{
    private float _countdownTime;
    private float _timeRemaining;
    private readonly bool _resetOnZero;
    private bool _timerStopped;
    private readonly bool _useUnscaledTime;
    public event Action onStart;
    public event Action onEnd;

    public float TotalDuration => _countdownTime;
    public float CurrentTime => _timeRemaining;
    public bool TimerStopped => _timerStopped;

    public Timer(float countdownTime, bool resetOnZero = false, bool useUnscaledTime = false)
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

    void Restart()
    {
        _timeRemaining = _countdownTime;
        _timerStopped = false;
        onEnd?.Invoke();
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

    public void End()
    {
        _timerStopped = true;
        onEnd?.Invoke();
    }

    public void ClearOnStartEvent() => onStart = null;
    public void ClearOnEndEvent() => onEnd = null;



}
