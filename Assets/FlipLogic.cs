using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLogic
{
    Transform _transform;
    Vector3 _currentScale, _endScale;
    AnimationCurve _curve;
    float _lockFlipTime, _elapsedTime = 0;
    float _flipDuration;
    bool _isFlipped, _flipCharacter, _flipX, _flipY;
    public bool IsFlipped => _isFlipped;
    public FlipLogic(Transform transformToFlip, bool flipX, bool flipY, float flipDuration = 0.25f)
    {
        _transform = transformToFlip;
        _flipX = flipX;
        _flipY = flipY;
        _flipDuration = flipDuration;
        _curve = TweenCurveLibrary.GetCurve(CurveTypes.EaseInOut);
    }


    public void UpdateLogic()
    {
        if(_flipCharacter)
        {
            _elapsedTime += Time.deltaTime;
            var percent = _elapsedTime / _flipDuration;
            var newScale = Vector3.Lerp(_currentScale, _endScale, _curve.Evaluate(percent));
            _transform.localScale = newScale;
            if(percent >= 1.01f)
            {
                _flipCharacter = false;
                _elapsedTime = 0;
                _isFlipped = !_isFlipped;
            }
        }
    }

    public void FlipCheck(float xDirection, float lockFlipTime = 0f)
    {
        if(_lockFlipTime > Time.time)return;

        if(xDirection < 0 && !_isFlipped)
        {
            Flip();

        }else if(xDirection > 0 && _isFlipped)
        {
            Flip();
        }

        void Flip()
        {
            _currentScale = _transform.localScale;
            _lockFlipTime = Time.time + lockFlipTime + _flipDuration + 0.075f;
            _flipCharacter = true;
            _elapsedTime = 0;
            int flipXDir = (_flipX) ? -1 : 1;
            int flipYDir = (_flipY) ? -1 : 1;
            _endScale = _currentScale;
            _endScale.x *= flipXDir;
            _endScale.y *= flipYDir;
        }
    }
}
