using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TweenAnimator : MonoBehaviour
{
    public enum TimeUsage
    {
        ScaledTime,
        UnscaledTime
    }
    [HideInInspector]public bool EnableAnimator = false;
    TweenStateFactory _stateFactory;
    TweenAnimationBase _currentAnimation;
    Queue<TweenAnimationBase> _animationQueue = new();
    bool _isAnimationPlaying = false;
    [SerializeField]TimeUsage _timeUsage = TimeUsage.ScaledTime;

    //public accessor

    public TimeUsage timeUsage => _timeUsage;

    private void Awake()
    {
        if(_stateFactory == null) _stateFactory = new(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!EnableAnimator) return;
        _currentAnimation.Play();
    }


    public void Clear()
    {
        SetAnimatorState(false);
        _animationQueue.Clear();
    }

    public void MoveTo
    (RectTransform rectTransform, Vector3 endPosition, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        CheckFactory();
        TweenMoveTo moveToAnim = _stateFactory.GetMoveToAnimation();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);    

        moveToAnim.Initialize(rectTransform, endPosition, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(moveToAnim);
    }

    /// <summary>
    /// Change opacity of an UnityEngine Image type, opacity value goes from 0 to 255.
    /// </summary>
    public void TweenImageOpacity
    (RectTransform rect, float endValue, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        CheckFactory();
        TweenImageOpacity imgOpacityAnim = _stateFactory.GetTweenImageOpacity();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);

        imgOpacityAnim.Initialize(rect, endValue, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(imgOpacityAnim);
    }


    public void Scale
    (RectTransform rectTransform, Vector3 endSize, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        CheckFactory();
        TweenScale scaleAnim = _stateFactory.GetScaleAnimation();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);    

        scaleAnim.Initialize(rectTransform, endSize, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(scaleAnim);
    }

    /// <summary>
    /// Change opacity of a TMPRO Text, opacity value goes from 0 to 255.
    /// </summary>
    public void TweenTextOpacity
    (TextMeshProUGUI text, float opacityEndValue, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        CheckFactory();
        TweenTextOpacity txtOpacityAnim = _stateFactory.GetTextOpacityAnimation();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);   

        txtOpacityAnim.Initialize(text, opacityEndValue, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(txtOpacityAnim); 
    }

    public void TweenTransformMoveTo
    (Transform transform, Vector3 endPosition, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        CheckFactory();
        TweenTransformMoveTo anim = _stateFactory.GetTwTransformMoveTo();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);   

        anim.Initialize(transform, endPosition, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(anim); 
    }

    public void TweenImageColor
    (RectTransform rect, Color endValue, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        CheckFactory();
        TweenImageColor anim = _stateFactory.GetTwImgColor();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);   

        anim.Initialize(rect, endValue, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(anim); 
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
        }else if(_currentAnimation != null && _currentAnimation.Loop)
        {
            SetAnimatorState(true);
        }
        else
        {
            SetAnimatorState(false);
        }
    }

    void SetAnimatorState(bool state)
    {
        _isAnimationPlaying = state;
        EnableAnimator = state;
    }

    void CheckFactory()
    {
        if(_stateFactory == null) _stateFactory = new(this);
    }

    public void ChangeTimeScalingUsage(TimeUsage usage)
    {
        _timeUsage = usage;
    }
}
