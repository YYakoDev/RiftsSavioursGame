using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TweenAnimationBase
{
    protected TweenAnimator _animator;
    protected bool _loop = false;

    public TweenAnimationBase(TweenAnimator animator)
    {
        _animator = animator;
    }

    public abstract void Play();

}
