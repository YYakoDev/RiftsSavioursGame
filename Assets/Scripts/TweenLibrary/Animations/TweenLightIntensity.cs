using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
public class TweenLightIntensity : TweenAnimationBase
{
    Light2D _lightObj;
    AnimationCurve _curve;
    
    float _startValue, _endValue;

    public TweenLightIntensity(TweenAnimator animator) : base(animator)
    {
    }


    public void Initialize(Light2D lightObj, float endValue, float duration, AnimationCurve curve, bool loop, Action onComplete)
    {
        _lightObj = lightObj;
        _startValue = _lightObj.intensity;
        _endValue = endValue;
        _totalDuration = duration;
        _curve = curve;
        _loop = loop;
        _onComplete = onComplete;
    }

    public override void Play()
    {
        base.Play();
        _lightObj.intensity = Mathf.Lerp(_startValue, _endValue, _curve.Evaluate(_percent));
        AnimationEnd();
    }
    protected override void AnimationEnd()
    {
        if(_elapsedTime >= _totalDuration && _loop)
        {
            float oldStartValue = _startValue;
            _startValue = _endValue;
            _endValue = oldStartValue;
        }
        base.AnimationEnd();
    }


}
