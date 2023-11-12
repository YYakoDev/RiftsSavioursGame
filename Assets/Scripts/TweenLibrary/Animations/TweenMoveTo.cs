using System;
using UnityEngine;

public class TweenMoveTo : TweenAnimationBase
{
    RectTransform _rectTransform;
    Vector3 _startPosition;
    Vector3 _destination;
    AnimationCurve _curve;


    public TweenMoveTo(TweenAnimator animator) : base(animator)
    {
    }


    public void Initialize(RectTransform rectTransform, Vector3 endPosition, float duration, AnimationCurve curve, bool loop, Action onComplete)
    {
        _rectTransform = rectTransform;
        _startPosition = rectTransform.localPosition;
        _destination = endPosition;
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
        _rectTransform.localPosition = Vector3.Lerp(_startPosition, _destination, _curve.Evaluate(_percent));

        
        AnimationEnd();
    }

    protected override void AnimationEnd()
    {
        if(_elapsedTime >= _totalDuration && _loop)
        {
            Vector3 oldStartPos = _startPosition;
            _startPosition = _destination;
            _destination = oldStartPos;
        }
        base.AnimationEnd();
    }
}
