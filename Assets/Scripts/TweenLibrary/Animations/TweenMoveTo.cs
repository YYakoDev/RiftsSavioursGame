using System;
using UnityEditor;
using UnityEngine;

public class TweenMoveTo : TweenAnimationBase
{
    RectTransform _rectTransform;
    Vector3 _startPosition, _endPosition;
    AnimationCurve _curve;
    TweenDestination _startDestination, _endDestination;

    private Vector2 _anchorOffset;
    
    bool _flip = false;

    public TweenMoveTo(TweenAnimator animator) : base(animator)
    {
    }


    public void Initialize(RectTransform rectTransform, TweenDestination endDestination, float duration, AnimationCurve curve, bool loop, Action onComplete)
    {
        _rectTransform = rectTransform;
        _startPosition = rectTransform.localPosition;
        _startDestination = _animator.GetDestination(_startPosition);
        SetAnchorOffset();
        _endDestination = endDestination;
        _endPosition = endDestination.GetEndPosition() + (Vector3)_anchorOffset;
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
            SetAnchorOffset();
            _flip = !_flip;
            if(_flip)
            {
                _startPosition = _endDestination.GetEndPosition() + (Vector3)_anchorOffset;
                _endPosition = _startDestination.GetEndPosition();
            }else
            {
                _startPosition = _startDestination.GetEndPosition();
                _endPosition = _endDestination.GetEndPosition() + (Vector3)_anchorOffset;
            }
            
        }
        base.AnimationEnd();
    }

    protected virtual void SetAnchorOffset()
    {
        // + new Vector3(1920f * 0.5f - 206f, 1080f * -0.5f + 300f);
        _anchorOffset = Vector2.zero;
        RectTransform rectParent = _rectTransform;
        var rootCanvas = _rectTransform.root.GetComponent<RectTransform>();
        GetRectParent();
        int iterations = 0;
        while (rectParent != null)
        {
            if(iterations >= 5) break;
            iterations++;
            if (rectParent == rootCanvas)
            {
                break; // when reaching the canvas stopping the loop
            }
            var offsetDirection = GetDirectionFromAnchor(rectParent.anchorMax);
            if (offsetDirection == Directions.EightDirections[4] && rectParent.anchorMin.Approximately(Vector3.zero)) // if anchor is top left but the min anchor is 0,0. that means is strecthed to the whole screen
            {
                continue;
            }

            if(offsetDirection == Vector2.zero) continue; //this means the anchor is the center so there shouldnt be an anchor offset
            var screenSize = _startDestination.GetCanvasSize();
            var anchoredPos = rectParent.anchoredPosition;
            var offset = -offsetDirection * (screenSize * 0.5f);
            anchoredPos = anchoredPos.Abs();
            anchoredPos.x *= Mathf.Sign(offsetDirection.x);
            anchoredPos.y *= Mathf.Sign(offsetDirection.y);
            _anchorOffset += offset + anchoredPos;

            GetRectParent();
        }

        return;
        
        void GetRectParent()
        {
            rectParent = rectParent?.parent?.GetComponent<RectTransform>();
        }

        Vector2 GetDirectionFromAnchor(Vector2 v)
        { 
            if (v.Approximately(_anchorDirections[0])) return Directions.EightDirections[7]; // bottom left : 0
            if (v.Approximately(_anchorDirections[1])) return Directions.EightDirections[1]; // bottom center : 1
            if (v.Approximately(_anchorDirections[2])) return Directions.EightDirections[6]; // bottom right : 2
            if (v.Approximately(_anchorDirections[3])) return Directions.EightDirections[3]; // center left : 3
            if (v.Approximately(_anchorDirections[4])) return Vector2.zero; // center center : 4
            if (v.Approximately(_anchorDirections[5])) return Directions.EightDirections[2]; // center right : 5
            if (v.Approximately(_anchorDirections[6])) return Directions.EightDirections[5]; // top left : 6 
            if (v.Approximately(_anchorDirections[7])) return Directions.EightDirections[0]; // top center : 7
            if (v.Approximately(_anchorDirections[8])) return Directions.EightDirections[4]; // top right : 8
            return Vector2.zero;
        }
    }

    private readonly Vector2[] _anchorDirections = new[]
    {
        new Vector2(0f, 0f), // bottom left : 0
        new Vector2(0.5f, 0f), // bottom center : 1
        new Vector2(1f, 0f), // bottom right : 2
        new Vector2(0f, 0.5f), // center left : 3
        new Vector2(0.5f,0.5f), // center center : 4
        new Vector2(1f, 0.5f), // center right : 5
        new Vector2(0f, 1f), // top left : 6 
        new Vector2(0.5f, 1f), // top center : 7
        new Vector2(1f, 1f), // top right : 8
        
        // you could also the stretched anchors
    };
}
