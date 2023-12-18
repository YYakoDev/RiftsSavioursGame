using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFXPrefab : MonoBehaviour
{
    Timer _timer;
    [SerializeField, Range(-1f, 10f)] float _duration = 0.25f;

    private void Awake() {
        if(_duration <= 0) this.enabled = false;
        _timer = new(_duration);
        _timer.onEnd += DeactivateItself;
    }

    private void OnEnable()
    {
        _timer.Start();
    }
    void Update()
    {
        _timer.UpdateTime();
    }
    void DeactivateItself()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        _timer.onEnd -= DeactivateItself;
    }
}
