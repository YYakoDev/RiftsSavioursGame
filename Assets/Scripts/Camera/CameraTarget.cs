using System;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]Transform[] _targets = new Transform[1];
    int _currentTargetIndex = 0;
    public event Action onTargetSwitch;
    public Transform Target => _targets[_currentTargetIndex];

    private void Awake() {
        _targets[0] = transform;
    }

    public int AddTarget(Transform target)
    {
        int newSize = _targets.Length + 1;
        int newTargetIndex = newSize - 1;
        Array.Resize<Transform>(ref _targets, newSize);
        _targets[newTargetIndex] = target;
        return newTargetIndex;
        
    }

    public void SwitchTarget(int index)
    {
        if(index >= _targets.Length) index = _targets.Length - 1;
        if(index < 0) index = 0;
        
        _currentTargetIndex = index;
        onTargetSwitch?.Invoke();
    }




    
}
