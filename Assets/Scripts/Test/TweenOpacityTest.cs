using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenOpacityTest : MonoBehaviour
{
    TweenAnimator _animator;
    
    [SerializeField]RectTransform _img;
    [SerializeField]float _fadeInTime = 0.3f;
    private void Awake() {
        _animator = GetComponent<TweenAnimator>();
    }

    private void OnEnable() {
        _animator.TweenImageOpacity(_img, 175, _fadeInTime, CurveTypes.EaseInOut);
    }
}
