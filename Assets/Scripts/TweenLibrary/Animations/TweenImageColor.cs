using System;
using UnityEngine;
using UnityEngine.UI;

public class TweenImageColor : TweenAnimationBase
{
    Image _image;
    Color _startingValue;
    Color _endColor;
    AnimationCurve _curve;
    public TweenImageColor(TweenAnimator animator) : base(animator)
    {
    }
    public void Initialize(RectTransform rect, Color endValue, float duration, AnimationCurve curve, bool loop, Action onComplete)
    {
        _image = rect.GetComponent<Image>();
        if(_image == null)
        {
            Debug.LogError($"The rect {rect.name} has no image component to make the tweenimageColor animation");
            _animator.EnableAnimator = false;
            return;
        }
    
        _startingValue = _image.color;
        _endColor = endValue;
        _totalDuration = (duration == 0) ?  0.001f : duration ;
        _curve = curve;
        _loop = loop;
        _onComplete = onComplete;
        _animator.EnableAnimator = true;
    }

    public override void Play()
    {
        base.Play();
        _image.color = Color.Lerp(_startingValue, _endColor, _curve.Evaluate(_percent));
        AnimationEnd();
    }

    protected override void AnimationEnd()
    {
        if(_elapsedTime >= _totalDuration && _loop)
        {
            var oldStartValue = _startingValue;
            _startingValue = _endColor;
            _endColor = oldStartValue;
        }
        base.AnimationEnd();
    }
}
