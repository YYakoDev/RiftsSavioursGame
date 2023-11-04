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

    public void SwitchTarget(int index)
    {
        if(index >= _targets.Length) index = _targets.Length - 1;
        _currentTargetIndex = index;
        onTargetSwitch?.Invoke();
    }




    
}
