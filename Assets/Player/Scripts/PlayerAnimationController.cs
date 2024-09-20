using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] PlayerManager _player;
    [SerializeField]Animator _animator;
    [SerializeField]EntryAnimationFX _introFX;
    int _currentAnimation;
    float _lockedTime;
    private PlayerIntroAnimation _introAnim;
    Dictionary<int, float> _durations;

    /*float _attackDuration;
    //bool _action;

    public float AtkDuration => _attackDuration;*/

    void Awake()
    {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Animator>(ref _animator);
        _introAnim = new(_animator, _introFX.gameObject);
    }

    private IEnumerator Start() {
        _durations = new()
        {
            {PlayerAnimationsNames.Iddle, GetAnimationDuration(PlayerAnimationsNames.Iddle)},
            {PlayerAnimationsNames.Run, GetAnimationDuration(PlayerAnimationsNames.Run)},
            {PlayerAnimationsNames.Attack, GetAnimationDuration(PlayerAnimationsNames.Attack)},
            {PlayerAnimationsNames.Landing, GetAnimationDuration(PlayerAnimationsNames.Landing)},
            {PlayerAnimationsNames.ForwardDash, GetAnimationDuration(PlayerAnimationsNames.ForwardDash)},
            {PlayerAnimationsNames.BackDash, GetAnimationDuration(PlayerAnimationsNames.BackDash)},
        };
        yield return null;
        //Debug.Log("Forward dash duration:  " + _durations[PlayerAnimationsNames.ForwardDash] + "\n Back Dash: " + _durations[PlayerAnimationsNames.BackDash]);
        _introAnim.PlayAnimation();
    }

    private void Update() {
        _introAnim.UpdateLogic();
    }

    public void PlayStated(int animationHash, float lockDuration = -0.05f, bool replay = false)
    {
        if(Time.time < _lockedTime) return;
        if(animationHash == _currentAnimation && !replay)return;
        LockAnimator(lockDuration);
        _currentAnimation = animationHash;
        if(replay) _animator.Play(animationHash, -1, 0f);
        else _animator.Play(animationHash);
    }
    public void PlayWithDuration(int animHash, bool replay = false)
    {
        float duration = 0f;
        _durations.TryGetValue(animHash, out duration);
        PlayStated(animHash, duration - Time.deltaTime, replay);
    }

    void LockAnimator(float time) => _lockedTime = Time.time + time;
    public void LockAnimator() => _lockedTime = Time.time + 500f;
    public void UnlockAnimator() => _lockedTime = 0f;
    

    public void ChangeAnimator(AnimatorOverrideController animator)
    {
        _animator.runtimeAnimatorController = animator;
    }

    float GetAnimationDuration(int animHash)
    {
        var originalClips = _player.CharacterData.Animator.runtimeAnimatorController.animationClips;
        var clips = _player.CharacterData.Animator.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            var clip = clips[i];
            var originalClip = originalClips[i];
            int hash = Animator.StringToHash(originalClip.name);
            if (hash == animHash)
            {
                return clip.averageDuration;
            }
        }
        Debug.Log("Didnt found anim duration");
        return 0.2f;
    }

}
