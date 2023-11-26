using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TweenAnimator))]
public class DamagePopupAnimation : MonoBehaviour
{
    TweenAnimator _animator;
    RectTransform _rect;
    TextMeshPro _text;
    private void Awake() {
        _animator = GetComponent<TweenAnimator>();
        _rect = GetComponent<RectTransform>();
        _text = GetComponent<TextMeshPro>();
    }

    private void OnEnable() {
        PlayAnimations();
    }

    void PlayAnimations()
    {
        Vector3 startingScale = _rect.localScale;
        Vector3 startingPos = _rect.localPosition;
        _animator.Scale(_rect, startingScale * 1.3f, 0.2f, CurveTypes.EaseInOut, 
        onComplete:() => 
        {
            _animator.Scale(_rect, startingScale, 0.15f, CurveTypes.EaseInOut, 
            onComplete:() => 
            {
                _animator.MoveTo(_rect, startingPos + Vector3.up * 2f, 0.4f, CurveTypes.EaseInOut, onComplete:() => 
                {
                    gameObject.SetActive(false);
                });
            });
        });
    }
}
