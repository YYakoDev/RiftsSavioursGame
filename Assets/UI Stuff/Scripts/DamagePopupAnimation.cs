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
    Vector3 _startingScale = Vector3.zero;
    Vector3 _startingPos = Vector3.zero;
    private void Awake() {
        _rect = GetComponent<RectTransform>();
        _text = _rect.GetComponent<TextMeshPro>();
        _animator = _rect.GetComponent<TweenAnimator>();
    }

    private void OnEnable() {
        _animator.Clear();
        _startingScale = _rect.localScale;
        _startingPos = _rect.position;
        PlayAnimations();
    }

    void PlayAnimations()
    {
        Vector3 startingScale = _rect.localScale;
        Vector3 startingPos = _rect.position;
        _animator.Scale(_rect, startingScale * 1.3f, 0.2f, CurveTypes.EaseInOut, onComplete: ScaleDown);
    }

    void ScaleDown()
    {
        _animator.Scale(_rect, _startingScale, 0.15f, CurveTypes.EaseInOut, onComplete: MoveUp);
    }

    void MoveUp()
    {
        _animator.MoveTo(_rect, _startingPos + Vector3.up * 2f, 0.4f, CurveTypes.EaseInOut, onComplete: Deactivate);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        PopupsManager.RecreatePool();
    }
}
