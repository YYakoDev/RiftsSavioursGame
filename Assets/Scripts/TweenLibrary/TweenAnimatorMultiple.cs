using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimatorMultiple : TweenAnimator
{
    TweenAnimationBase[] _animations = new TweenAnimationBase[0];

    protected override void SwitchCurrentAnimation(TweenAnimationBase animationBase)
    {
        base.EnableAnimator = false;
        int arrayLength = _animations.Length;
        System.Array.Resize<TweenAnimationBase>(ref _animations, arrayLength + 1);
        _animations[arrayLength] = animationBase;

    }

    private void Update() {
        foreach(TweenAnimationBase animation in _animations)
        {
            animation?.Play();
        }
    }

    public override void AnimationComplete()
    {
        CheckAnimationEndState();
    }

    void CheckAnimationEndState()
    {
        for (int i = 0; i < _animations.Length; i++)
        {
            var animation = _animations[i];
            if(animation == null) continue;
            if(animation.Loop) continue;
            if(animation.Percent >= 1f) _animations[i] = null;
        }
    }

    public override void Clear()
    {
        _animations = new TweenAnimationBase[0];
    }
}
