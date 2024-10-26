using System;
using UnityEngine;

public class TweenMoveTo : TweenAnimationBase
{
    RectTransform _rectTransform;
    Vector3 _startPosition, _endPosition;
    AnimationCurve _curve;
    TweenDestination _startDestination, _endDestination;

    bool _flip = false;

    public TweenMoveTo(TweenAnimator animator) : base(animator)
    {
    }


    public void Initialize(RectTransform rectTransform, TweenDestination endDestination, float duration, AnimationCurve curve, bool loop, Action onComplete)
    {
        _rectTransform = rectTransform;
        _startPosition = rectTransform.localPosition;
        _startDestination = _animator.GetDestination(_startPosition);
        _endDestination = endDestination;
        _endPosition = endDestination.GetEndPosition();
        _totalDuration = (duration == 0) ?  0.0001f : duration ;
        _elapsedTime = 0;
        _curve = curve;
        _loop = loop;
        _onComplete = onComplete;
        _animator.EnableAnimator = true;
    }
    public override void Play()
    {
        base.Play();
        _rectTransform.localPosition = Vector3.Lerp(_startPosition, _endPosition, _curve.Evaluate(_percent));

        
        AnimationEnd();
    }

    protected override void AnimationEnd()
    {
        if(_elapsedTime >= _totalDuration && _loop)
        {
            _flip = !_flip;
            if(_flip)
            {
                _startPosition = _endDestination.GetEndPosition();
                _endPosition = _startDestination.GetEndPosition();
            }else
            {
                _startPosition = _startDestination.GetEndPosition();
                _endPosition = _endDestination.GetEndPosition();
            }
            
        }
        base.AnimationEnd();
    }
}
