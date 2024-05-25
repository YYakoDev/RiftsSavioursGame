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
        _startingScale = _rect.localScale;
        _startingPos = _rect.position;
        PlayAnimations();
    }

    void PlayAnimations()
    {
        _rect.localScale = _startingScale;
        _animator.Scale(_rect, _startingScale * 1.3f, 0.2f, CurveTypes.EaseInOut, onComplete: ScaleDown);
        _animator.MoveTo(_rect, _startingPos + Vector3.up * 2f, 0.75f, CurveTypes.EaseInOut, onComplete: Deactivate);
    }

    void ScaleDown()
    {
        _animator.Scale(_rect, _startingScale, 0.35f, CurveTypes.EaseInOut);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        PopupsManager.RecreatePool();
    }
}
