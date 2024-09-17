using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class Notification : MonoBehaviour
{
    Transform _cachedTransform, _target, _canvas;
    TweenAnimatorMultiple _animator;
    RectTransform _rect;
    Vector3 _offset, _initialPosition, _endPosition;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _icon;
    LayoutElement _iconElement;
    [SerializeField] CurveTypes _scaleUpCurveType, _scaleDownCurveType, _moveCurveType;
    [SerializeField] float _animDuration = 0.6f, _offsetScaler = 1f, _widthOffset = 125;
    float _elapsedMoveTime;
    AnimationCurve _moveCurve;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _rect = GetComponent<RectTransform>();
        _cachedTransform = transform;
        _iconElement = _icon.GetComponent<LayoutElement>();
    }

    private void Start() {
        _canvas = transform.parent.parent;
        _moveCurve = TweenCurveLibrary.GetCurve(_moveCurveType);
    }

    private void OnEnable() {
        
        _cachedTransform.localScale = Vector3.zero;
    }

    private void Update() {
        if(_target == null) return;
        _elapsedMoveTime += Time.deltaTime;
        var percent = _elapsedMoveTime / (_animDuration / 2f);
        _cachedTransform.position = _target.position + Vector3.Lerp(Vector3.zero, _offset, _moveCurve.Evaluate(percent));
    }

    public void Set(string text, Sprite icon, Transform targetTransform, NotificationType type, float offsetScaler)
    {
        _text.SetText(text);
        if(icon != null)
        {
            _iconElement.ignoreLayout = false;
            _icon.sprite = icon;
            var size = _rect.sizeDelta;
            size.x = _icon.rectTransform.sizeDelta.x + _text.rectTransform.sizeDelta.x + _widthOffset;
            _rect.sizeDelta = size;
        }
        else
        {
            _iconElement.ignoreLayout = true;
            var size = _rect.sizeDelta;
            size.x = _text.rectTransform.sizeDelta.x + _widthOffset;
            _rect.sizeDelta = size;
        }
        _target = targetTransform;
        var offset = type switch
        {   
            NotificationType.Top => Vector3.up,
            NotificationType.Left => Vector3.left,
            NotificationType.Right => Vector3.right,
            NotificationType.Bottom => Vector3.down,
            _ => Vector3.up
        };
        _elapsedMoveTime = 0f;
        if(_canvas == null) _canvas = transform.parent.parent;

        var sizeCalculation = (Vector3)((_rect.sizeDelta / 2f) * _canvas.localScale.x);
        sizeCalculation.x *= Math.Sign(offset.x);
        sizeCalculation.y *= Math.Sign(offset.y);
        _offset = offset + sizeCalculation;
        _offset *= _offsetScaler * offsetScaler;
        _initialPosition = _target.position;
        _endPosition = _initialPosition + _offset;
        PlayAnimation();
    }

    void PlayAnimation()
    {
        _animator.Scale(_rect, Vector3.one, _animDuration, _scaleUpCurveType, onComplete:() => 
        {
            _animator.Scale(_rect, Vector3.one, _animDuration / 2f, CurveTypes.Linear,onComplete: () => 
            {
                _animator.Scale(_rect, Vector3.zero, _animDuration / 4f, _scaleDownCurveType, onComplete: () =>
                {
                    gameObject.SetActive(false);//
                });
            });
        });
    }
}

