using System;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimator : MonoBehaviour
{

    public bool EnableAnimator = false;
    TweenAnimationBase _currentAnimation;

    private void Awake() {

    }

    // Update is called once per frame
    void Update()
    {
        if(!EnableAnimator) return;
        _currentAnimation.Play();
    }


    public void MoveTo(RectTransform rectTransform, Vector3 endPosition, float duration, Action onComplete = null, bool loop = false)
    {
        TweenMoveTo moveAnim = new(this);
        moveAnim.Initialize(rectTransform, endPosition, duration, onComplete, loop);
        _currentAnimation = moveAnim;
    }
}
