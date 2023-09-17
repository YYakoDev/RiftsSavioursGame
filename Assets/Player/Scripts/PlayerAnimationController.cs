using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]Animator _animator;
    string _currentAnimation;
    
    //[SerializeField]float _collectingAnimTime = 0.4f;
    //public float CollectingTime => _collectingAnimTime;

    float _lockedTill;
    bool _action;

    void Awake()
    {
        gameObject.CheckComponent<Animator>(ref _animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayStated(string animationName)
    {
        if(Time.time < _lockedTill) return;
        if(animationName == _currentAnimation)return;
        /*if(_action)return;

        if(animationName == PlayerAnimationsNames.Mining || animationName == PlayerAnimationsNames.Chopping)
        {
            _action = true;
            LockState(_collectingAnimTime);
            _player.MovementScript.StopMovementForSeconds(_collectingAnimTime*0.85f);
        }
        if(animationName == PlayerAnimationsNames.Gathering)
        {
            _action = true;
            LockState(_collectingAnimTime/2f);
        }*/

        //Debug.Log(animationName +  " is currently playing");
        _currentAnimation = animationName;
        _animator.Play(animationName);
        _action = false;


        void LockState( float time)
        {
            _lockedTill = Time.time + time;
        }
    }


}
