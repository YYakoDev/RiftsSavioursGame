using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField]EntryAnimationFX _introFX;
    int _currentAnimation;
    float _lockedTime;
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
        if(Time.time < _lockedTime) return;
        if(animationHash == _currentAnimation)return;

        LockAnimator(duration);
        _currentAnimation = animationHash;
        _animator.Play(animationHash);
    }

    public void LockAnimator()
    {
        _lockedTime = 500f;
    }
    public void UnlockAnimator()
    {
        _lockedTime = 0f;
    }
    public void LockAnimator(float time)
    {
        _lockedTime = Time.time + time;
    }

    public void ChangeAnimator(AnimatorOverrideController animator)
    {
        _animator.runtimeAnimatorController = animator;
    }

}
