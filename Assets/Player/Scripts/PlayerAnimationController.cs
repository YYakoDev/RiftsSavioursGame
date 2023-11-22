using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField]EntryAnimationFX _introFX;
    PlayerIntroAnimation _introAnim;
    string _currentAnimation;
    /*float _lockedTill;
    float _attackDuration;
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

    public void PlayStated(string animationName)
    {
        //if(Time.time < _lockedTill) return;
        if(animationName == _currentAnimation)return;


        _currentAnimation = animationName;
        _animator.Play(animationName);


        /*void LockState( float time)
        {
            _lockedTill = Time.time + time;
        }*/
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
