using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField]EntryAnimationFX _introFX;
    int _currentAnimation;
    float _lockedTill;
    private PlayerIntroAnimation _introAnim;

    /*float _attackDuration;
    //bool _action;

    public float AtkDuration => _attackDuration;*/

    void Awake()
    {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Animator>(ref _animator);
        _introAnim = new(_animator, _introFX.gameObject);
    }

    private void Start() {
        _introAnim.PlayAnimation();
    }

    private void Update() {
        _introAnim.UpdateLogic();
    }

    public void PlayStated(int animationHash, float duration = -0.05f)
    {
        if(Time.time < _lockedTill) return;
        if(animationHash == _currentAnimation)return;

        LockState(duration);
        _currentAnimation = animationHash;
        _animator.Play(animationHash);

        void LockState( float time)
        {
            _lockedTill = Time.time + time;
        }
    }

    public void ChangeAnimator(AnimatorOverrideController animator)
    {
        _animator.runtimeAnimatorController = animator;
    }

}
