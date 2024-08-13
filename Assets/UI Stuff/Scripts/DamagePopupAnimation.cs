using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TweenAnimatorMultiple))]
public class DamagePopupAnimation : MonoBehaviour
{
    TweenAnimatorMultiple _animator;
    RectTransform _rect;
    TextMeshPro _text;
    Vector3 _startingScale = Vector3.zero;
    Vector3 _startingPos = Vector3.zero;
    private void Awake() {
        _rect = GetComponent<RectTransform>();
        _text = _rect.GetComponent<TextMeshPro>();
        _animator = _rect.GetComponent<TweenAnimatorMultiple>();
    }

    private void OnEnable() {
        _animator.Clear();
        if(_startingScale == Vector3.zero)_startingScale = _rect.localScale;
        _startingPos = _rect.position;
        PlayAnimations();
    }

    void PlayAnimations()
    {
        _rect.localScale = _startingScale;
        _animator.Scale(_rect, _startingScale * 1.35f, 0.4f, CurveTypes.EaseInBounce, onComplete: ScaleDown);
        _animator.MoveTo(_rect, _startingPos + Vector3.up / 2f, 0.55f, CurveTypes.EaseInBounce, onComplete: Deactivate);
    }

    void ScaleDown()
    {
        _animator.Scale(_rect, Vector3.zero, 0.25f, CurveTypes.EaseOutCirc);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        PopupsManager.RecreatePool();
    }
}
