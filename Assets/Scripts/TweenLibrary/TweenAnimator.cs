using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenAnimator : MonoBehaviour
{

    [HideInInspector]public bool EnableAnimator = false;
    TweenStateFactory _stateFactory;
    TweenAnimationBase _currentAnimation;
    Queue<TweenAnimationBase> _animationQueue = new();
    bool _isAnimationPlaying = false;

    private void Awake()
    {
        if(_stateFactory == null) _stateFactory = new(this);
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!EnableAnimator) return;
        _currentAnimation.Play();
    }


    public void MoveTo
    (RectTransform rectTransform, Vector3 endPosition, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        if(_stateFactory == null) _stateFactory = new(this);
        TweenMoveTo moveToAnim = _stateFactory.GetMoveToAnimation();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);    

        moveToAnim.Initialize(rectTransform, endPosition, duration, curve, loop, onComplete + UnlockAnimator);
        SwitchCurrentAnimation(moveToAnim);
    }

    public void TweenImageOpacity
    (RectTransform rect, float endValue, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        if(_stateFactory == null) _stateFactory = new(this);
        TweenImageOpacity imgOpacityAnim = _stateFactory.GetTweenImageOpacity();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);

        imgOpacityAnim.Initialize(rect, endValue, duration, curve, loop, onComplete + UnlockAnimator);
        SwitchCurrentAnimation(imgOpacityAnim);
    }


    public void Scale
    (RectTransform rectTransform, Vector3 endSize, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        if(_stateFactory == null) _stateFactory = new(this);
        TweenScale scaleAnim = _stateFactory.GetScaleAnimation();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);    

        scaleAnim.Initialize(rectTransform, endSize, duration, curve, loop, onComplete + UnlockAnimator);
        SwitchCurrentAnimation(scaleAnim);
    }

    void SwitchCurrentAnimation(TweenAnimationBase animationBase)
    {
        if(_isAnimationPlaying)
        {
            _animationQueue.Enqueue(animationBase);
        }else
        {
            _currentAnimation = animationBase;
            SetAnimatorState(true);
        }

    }

    public void UnlockAnimator()
    {
        if(_animationQueue.Count > 0)
        {
            _currentAnimation = _animationQueue.Dequeue();
            SetAnimatorState(true);
        }else if(_currentAnimation.Loop)
        {
            SetAnimatorState(true);
        }else
        {
            SetAnimatorState(false);
        }
    }

    void SetAnimatorState(bool state)
    {
        _isAnimationPlaying = state;
        EnableAnimator = state;
    }
}