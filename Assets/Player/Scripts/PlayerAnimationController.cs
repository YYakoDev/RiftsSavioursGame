using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField]EntryAnimationFX _introFX;
    PlayerIntroAnimation _introAnim;
    int _currentAnimation;
    float _lockedTill;
    /*float _attackDuration;
    //bool _action;

    public float AtkDuration => _attackDuration;*/


    void Awake()
    {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Animator>(ref _animator);
        _introAnim = new(_animator, _introFX.gameObject);
    }

    private void Start()
    {
        _introAnim.PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        _introAnim.UpdateLogic();
    }

    public void PlayStated(int animationHash)
    {
        if(Time.time < _lockedTill) return;
        if(animationHash == _currentAnimation)return;

        if(animationHash == PlayerAnimationsNames.Attack)
        {
            LockState(0.3f);
            //GET THE ANIM DURATION HERE!
        }
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

    /*void GetAttackDuration()
    {
        var clips = _animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            if(clip.name == PlayerAnimationsNames.Attack)
            {
                _attackDuration = clip.averageDuration;
                break;  
            } 
        }
    }*/


}
