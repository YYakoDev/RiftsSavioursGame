using System;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField]Transform[] _targets = new Transform[1];
    int _currentTargetIndex = 0;
    public event Action onTargetSwitch;
    public Transform Target => _targets[_currentTargetIndex];
    Timer _targetSwitchDuration;

    private void Awake() {
        if(_targets[0] == null) _targets[0] = transform;
        _targetSwitchDuration = new(1f);
        _targetSwitchDuration.onEnd += ReturnCamera;
        _targetSwitchDuration.Stop();

    }

    public int AddTarget(Transform target)
    {
        int newSize = _targets.Length + 1;
        int newTargetIndex = newSize - 1;
        Array.Resize<Transform>(ref _targets, newSize);
        _targets[newTargetIndex] = target;
        return newTargetIndex;
        
    }

    private void Update() {
        _targetSwitchDuration.UpdateTime();
    }

    public void SwitchTarget(int index)
    {
        if(index >= _targets.Length) index = _targets.Length - 1;
        if(index < 0) index = 0;
        
        _currentTargetIndex = index;
        onTargetSwitch?.Invoke();
    }

    public void SwitchTarget(int index, float duration)
    {
        if(index >= _targets.Length) index = _targets.Length - 1;
        if(index < 0) index = 0;
        
        _currentTargetIndex = index;
        onTargetSwitch?.Invoke();
        _targetSwitchDuration.ChangeTime(duration);
        _targetSwitchDuration.Start();
    }

    void ReturnCamera()
    {
        SwitchTarget(0);
    }

    private void OnDestroy() {
        _targetSwitchDuration.onEnd -= ReturnCamera;
    }





    
}
