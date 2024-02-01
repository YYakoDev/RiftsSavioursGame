using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class XPBarAnimations : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image _fillImg;
    [SerializeField] RectTransform _lvlNumber;
    RectTransform _fillRect;
    TweenAnimatorMultiple _animator;

    [Header("Animation Values")]
    [SerializeField] float _shakeDuration = 0.5f;
    [SerializeField] Vector3 _shakeOffset = Vector3.one;
    [SerializeField] int _maxShakes = 3;
    [SerializeField] float _flashDuration = 0.5f;
    [SerializeField] Color _flashColor;
    Color _startingFillColor;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _fillRect = _fillImg.rectTransform;
        _startingFillColor = _fillImg.color;
    }

    public void FlashBar()
    {
        _animator.TweenImageColor(_fillRect, _flashColor, _flashDuration, onComplete: () => 
        {
            _animator.TweenImageColor(_fillRect, _startingFillColor, _flashDuration / 2f);
        });
    }

    public void ShakeNumber()
    {
        _lvlNumber.localScale = Vector3.zero;
        _animator?.TweenScaleBouncy(_lvlNumber, Vector3.one, _shakeOffset, _shakeDuration, 0, _maxShakes);
    }




}
