using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimator))]
public class TestTweenShake : MonoBehaviour
{
    TweenAnimator _animator;
    RectTransform _rect;
    [SerializeField]Vector3 _newSize;
    [SerializeField]float _animDuration;
    [SerializeField]bool _loop;
    private void Awake() {
        _animator = GetComponent<TweenAnimator>();
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        _animator.Scale(_rect, _newSize, _animDuration, CurveTypes.EaseInOut, _loop);
    }

   
}
