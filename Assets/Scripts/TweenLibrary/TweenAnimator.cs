using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenAnimator : MonoBehaviour
{

    [HideInInspector]public bool EnableAnimator = false;
    TweenStateFactory _stateFactory;
    TweenAnimationBase _currentAnimation;
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


    public void MoveTo(RectTransform rectTransform, Vector3 endPosition, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        if(_stateFactory == null) _stateFactory = new(this);
        TweenMoveTo moveToAnim = _stateFactory.GetMoveToAnimation();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);    

        moveToAnim.Initialize(rectTransform, endPosition, duration, curve, loop, onComplete);
        _currentAnimation = moveToAnim;
    }

    public void TweenImageOpacity(Image image, float endValue, float duration, CurveTypes curveType = CurveTypes.Linear, bool loop = false, Action onComplete = null)
    {
        if(_stateFactory == null) _stateFactory = new(this);
        TweenImageOpacity imgOpacityAnim = _stateFactory.GetTweenImageOpacity();
        AnimationCurve curve = TweenCurveLibrary.GetCurve(curveType);

        imgOpacityAnim.Initialize(image, endValue, duration, curve, loop, onComplete);
        _currentAnimation = imgOpacityAnim;
    }
}
