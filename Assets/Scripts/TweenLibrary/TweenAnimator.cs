using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimator : MonoBehaviour
{

    public bool EnableAnimator = false;
    TweenStateFactory _stateFactory;
    TweenAnimationBase _currentAnimation;

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


    public void MoveTo(RectTransform rectTransform, Vector3 endPosition, float duration, Action onComplete = null, bool loop = false)
    {
        if(_stateFactory == null) _stateFactory = new(this);
        TweenMoveTo moveToAnim = _stateFactory.GetMoveToAnimation();
        moveToAnim.Initialize(rectTransform, endPosition, duration, onComplete, loop);
        _currentAnimation = moveToAnim;
    }
}
