using System;
using UnityEngine;

public class TweenMoveTo : TweenAnimationBase
{
    RectTransform _rectTransform;
    Vector3 _startPosition;
    Vector3 _destination;
    float _elapsedTime;
    float _totalDuration;
    event Action _onComplete;

    public TweenMoveTo(TweenAnimator animator) : base(animator)
    {
    }


    public void Initialize(RectTransform rectTransform, Vector3 endPosition, float duration, Action onComplete = null, bool loop = false)
    {
        _rectTransform = rectTransform;
        _startPosition = rectTransform.localPosition;
        _destination = endPosition;
        _totalDuration = duration;
        _onComplete = onComplete;
        _loop = loop;
        _animator.EnableAnimator = true;
    }
    public override void Play()
    {
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _totalDuration;
        _rectTransform.localPosition = Vector3.Lerp(_startPosition, _destination, percent);
        //_rectTransform.localPosition = _startPosition;

        if(_elapsedTime >= _totalDuration)
        {
            _onComplete?.Invoke();
            if(_loop)
            {
                Vector3 oldStartPos = _startPosition;
                _startPosition = _destination;
                _destination = oldStartPos;
                _elapsedTime = 0;
            _animator.EnableAnimator = true;
            }else
            {
                _animator.EnableAnimator = false;
                _elapsedTime = 0;
            }
        }
    }
}
