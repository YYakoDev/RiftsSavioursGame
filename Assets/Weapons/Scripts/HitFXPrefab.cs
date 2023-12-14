using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFXPrefab : MonoBehaviour
{
    Timer _timer;
    Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();

        _timer = new(0.25f, false, false);
        if(_animator != null)
        {
            float animDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
            _timer.ChangeTime(animDuration);
        }
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
