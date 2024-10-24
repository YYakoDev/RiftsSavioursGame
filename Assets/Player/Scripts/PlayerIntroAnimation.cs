using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntroAnimation
{
    Transform _player;
    Animator _animator;
    GameObject _fxPrefab;
    GameObject _fxInstance;
    EntryAnimationFX _fxScript;
    float _animDuration = 0.67f;
    bool _animationEnded = false;
    Timer _animTimer;

    public PlayerIntroAnimation(Animator animator, GameObject fxPrefab)
    {
        _animator = animator;
        _player = animator.transform;
        _fxPrefab = fxPrefab;
        SetAnimDuration();
        
        _animTimer = new(_animDuration + 0.1f, false, true);
        _animTimer.Stop();
        _animTimer.onEnd += ResumeTime;
        //PlayAnimation();
    }

    public void PlayAnimation()
    {
        StopTime();
        _animTimer.Start();
        _animator.Play(PlayerAnimationsNames.Landing);
        SpawnFX();
    }

    public void UpdateLogic()
    {
        if(_animationEnded)return;
        _animTimer.UpdateTime();
    }

    void SpawnFX()
    {
        Vector3 playerPos = _player.position + Vector3.up;
        if(_fxInstance == null)
        {
            _fxInstance = GameObject.Instantiate(_fxPrefab, playerPos, Quaternion.identity);
            _fxScript = _fxInstance.GetComponent<EntryAnimationFX>();
        }
        _fxInstance.transform.position = playerPos;
        _fxScript.PlayFXAnimation();
    }

    void SetAnimDuration()
    {
        var clips = _animator.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips)
        {
            int hash = Animator.StringToHash(clip.name);
            if(hash == PlayerAnimationsNames.Landing)
            {
                _animDuration = clip.averageDuration;
                break;
            }
        }
    }

    void StopTime()
    {
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        TimeScaleManager.SetTimeScale(0f);
    }

    void ResumeTime()
    {
        TimeScaleManager.ForceTimeScale(1f);
        _animationEnded = true;
        _animator.updateMode = AnimatorUpdateMode.Normal;
        _animator.StopPlayback();
    }
}
