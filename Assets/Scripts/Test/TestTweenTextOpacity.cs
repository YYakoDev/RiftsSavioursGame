using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TweenAnimator))]
public class TestTweenTextOpacity : MonoBehaviour
{
    TweenAnimator _animator;
    [SerializeField]TextMeshProUGUI _text;
    [SerializeField]float _opacityValue;
    [SerializeField]float _duration = 1;
    [SerializeField]bool _loop = false;
    
    private void Awake() {
        _animator = GetComponent<TweenAnimator>();
    }

    private void OnEnable() {
        _animator.TweenTextOpacity(_text, _opacityValue, _duration, CurveTypes.EaseInOut, _loop);
    }
}
