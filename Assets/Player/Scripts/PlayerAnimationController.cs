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
    //float _lockedTill;
    //bool _action;



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
        /*_action = false;


        void LockState( float time)
        {
            _lockedTill = Time.time + time;
        }*/
    }


}
