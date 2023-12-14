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
    TweenAnimationBase _currentAnimation;
    Queue<TweenAnimationBase> _animationQueue = new();
    bool _isAnimationPlaying = false;
    [SerializeField]TimeUsage _timeUsage = TimeUsage.ScaledTime;

    //public accessor

    public TimeUsage timeUsage => _timeUsage;


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
        TweenMoveTo moveToAnim = new(this);
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
        TweenImageOpacity imgOpacityAnim = new(this);
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);

        imgOpacityAnim.Initialize(rect, endValue, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(imgOpacityAnim);
    }


    public void Scale
    (RectTransform rectTransform, Vector3 endSize, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        TweenScale scaleAnim = new(this);
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
        TweenTextOpacity txtOpacityAnim = new(this);
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);   

        txtOpacityAnim.Initialize(text, opacityEndValue, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(txtOpacityAnim); 
    }

    public void TweenTransformMoveTo
    (Transform transform, Vector3 endPosition, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        TweenTransformMoveTo anim = new(this);
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);   

        anim.Initialize(transform, endPosition, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(anim); 
    }

    public void TweenImageColor
    (RectTransform rect, Color endValue, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        TweenImageColor anim = new(this);
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);   

        anim.Initialize(rect, endValue, duration, curve, loop, onComplete);
        SwitchCurrentAnimation(anim); 
    }

    void SwitchCurrentAnimation(TweenAnimationBase animationBase)
    {
        Debug.Log("Enqueueing  " + animationBase);
        _animationQueue.Enqueue(animationBase);
        TryPlayNext();
    }

    void TryPlayNext()
    {
        if(_animationQueue.Count > 0 && !_isAnimationPlaying)
        {
            SetAnimatorState(true);
            Debug.Log("Dequeueing");
            _currentAnimation = _animationQueue.Dequeue();
        }
    }

    void SetAnimatorState(bool state)
    {
        EnableAnimator = state;
        _isAnimationPlaying = state;
    }

    public void AnimationComplete()
    {
        Debug.Log("Animation complete");
        _isAnimationPlaying = false;
        TryPlayNext();
    }


    public void ChangeTimeScalingUsage(TimeUsage usage)
    {
        _timeUsage = usage;
    }
}
