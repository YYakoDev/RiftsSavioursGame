using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashFXPrefab : MonoBehaviour
{
    PlayerManager _player;
    PlayerHealthManager _healthManager;
    SpriteRenderer _renderer;
    Animator _animator;
    SODashData _dashData;
    float _animDuration = 1f;
    AudioSource _audio;
    AudioClip _dashSFX;
    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void SetDashData(PlayerManager player, PlayerHealthManager healthManager, AudioSource audio)
    {
        _player = player;
        _audio = audio;
        _healthManager = healthManager;
        _dashSFX = _player.DashData.DashSfx;
        _dashData = _player.DashData;
        _dashData.Initialize(_player, _healthManager, _audio);
        _animator.runtimeAnimatorController = _player.DashData.DashAnimation;
        _animDuration = _player.DashData.DashAnimation["DashAnimation"].averageDuration;
        var speed = _animDuration / _player.MovementScript.DashDuration;
        _animator.speed = speed;
        _player.MovementScript.onDash += Play;
    }

    public void UpdateDashDuration(float dashDuration)
    {
        var speed = _animDuration / dashDuration;
        _animator.speed = speed;
    }

    public void Play()
    {
        _renderer.enabled = true;
        _animator.enabled = true;
        YYExtensions.i.PlayAnimationWithEvent(_animator, "Animation", Stop);
        DoVisualEffects();
    }

    void DoVisualEffects()
    {
        _dashData.PlayFX();
    }

    void Stop()
    {
        _animator.enabled = false;
        _renderer.enabled = false;
        _dashData.StopFX();
    }

    private void OnDestroy() {
        _player.MovementScript.onDash -= Play;
    }
}
