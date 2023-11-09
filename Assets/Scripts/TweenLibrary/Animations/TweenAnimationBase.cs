using System;
public abstract class TweenAnimationBase
{
    protected TweenAnimator _animator;
    protected float _elapsedTime;
    protected float _totalDuration;
    protected bool _loop = false;
    protected Action _onComplete;

    public TweenAnimationBase(TweenAnimator animator)
    {
        _animator = animator;
    }

    public abstract void Play();

    protected virtual void AnimationEnd()
    {
        if(_elapsedTime >= _totalDuration)
        {
            _onComplete?.Invoke();
            if(_loop)
            {
                _elapsedTime = 0;
                _animator.EnableAnimator = true;
            }else
            {
                _animator.EnableAnimator = false;
                _elapsedTime = 0;
            }
        }
    }

}
