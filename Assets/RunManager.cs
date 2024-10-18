using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    bool _runStarted = false;
    public static event Action OnRunDestroy, OnRunStart, OnRunEnd;

    public bool RunStarted => _runStarted;

    public void StartRun()
    {
        _runStarted = true;
        OnRunStart?.Invoke();
    }

    public void EndRun()
    {
        OnRunEnd?.Invoke();
        _runStarted = false;
    }

    private void OnDestroy() {
        OnRunDestroy?.Invoke();
    }
}
