using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenStateFactory
{
    TweenAnimator _animator;

    private TweenMoveTo _moveTo;
    private TweenImageOpacity _twImgOpacity;

    public TweenStateFactory(TweenAnimator animator)
    {
        _animator = animator;
    }
    
    public TweenMoveTo GetMoveToAnimation()
    {
        /*if(_moveTo == null)
        {
            _moveTo = new(_animator);
        }*/
        return new(_animator);
    }
    public TweenImageOpacity GetTweenImageOpacity()
    {
        /*if(_twImgOpacity == null)
        {
            _twImgOpacity = new(_animator);
        }*/
        return new(_animator);
    }

    public TweenScale GetScaleAnimation()
    {
        return new(_animator);
    }

}
