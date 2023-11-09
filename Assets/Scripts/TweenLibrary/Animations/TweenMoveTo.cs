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
        _totalDuration = duration;
        _curve = curve;
        _loop = loop;
        _onComplete = onComplete;
        _animator.EnableAnimator = true;
    }
    public override void Play()
    {
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _totalDuration;
        _rectTransform.localPosition = Vector3.Lerp(_startPosition, _destination, _curve.Evaluate(percent));
        //_rectTransform.localPosition = _startPosition;
        AnimationEnd();
    }

    protected override void AnimationEnd()
    {
        if(_elapsedTime >= _totalDuration)
        {
            if(!_loop) return;
            Vector3 oldStartPos = _startPosition;
            _startPosition = _destination;
            _destination = oldStartPos;
        }
        base.AnimationEnd();
    }
}
