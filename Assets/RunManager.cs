using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static event Action OnRunDestroy, OnRunStart, OnRunEnd;

    public void StartRun()
    {
        OnRunStart?.Invoke();
    }

    public void EndRun()
    {
        OnRunEnd?.Invoke();
    }

    private void OnDestroy() {
        OnRunDestroy?.Invoke();
    }
}
