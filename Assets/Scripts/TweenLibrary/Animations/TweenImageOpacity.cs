using System;
using UnityEngine;
using UnityEngine.UI;

public class TweenImageOpacity : TweenAnimationBase
{
    Image _image;
    Color _newColor;
    float _startingValue;
    float _endValue;
    AnimationCurve _curve;
    public TweenImageOpacity(TweenAnimator animator) : base(animator)
    {
    }
    public void Initialize(Image image, float endValue, float duration, AnimationCurve curve, bool loop = false, Action onComplete = null)
    {
        _image = image;
        _newColor = image.color;
        _startingValue = _newColor.a;
        _endValue = endValue;
        _totalDuration = duration;
        _curve = curve;
        _loop = loop;
        _onComplete = onComplete;
    }

    public override void Play()
    {
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _totalDuration;
        _newColor.a = Mathf.Lerp(_startingValue, _endValue, _curve.Evaluate(percent));
        _image.color = _newColor;

        AnimationEnd();
    }

    protected override void AnimationEnd()
    {
        base.AnimationEnd();
        if(_elapsedTime >= _totalDuration && _loop)
        {
            var oldStartValue = _startingValue;
            _startingValue = _endValue;
            _endValue = oldStartValue;
        }
    }
}
