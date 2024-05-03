using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/DashData")]
public class SODashData : ScriptableObject
{
    bool _initialized = false;
    private PlayerManager _player;
    private PlayerHealthManager _healthManager;
    private AudioSource _audio;
    [SerializeField] AnimatorOverrideController _dashAnimation;
    [SerializeField] float _motionBlur = 0.4f, _cameraShake = 0.4f;
    [SerializeField] AudioClip _dashSFX;

    //properties
    public AnimatorOverrideController DashAnimation => _dashAnimation;
    public AudioClip DashSfx => _dashSFX;

    public void Initialize(PlayerManager player, PlayerHealthManager healthManager, AudioSource audio)
    {
        _player = player;
        _audio = audio;
        _healthManager = healthManager;
        _initialized = true;
    }
    public virtual void PlayFX()
    {
        if(!_initialized) return;
        _healthManager.BlinkFX.Play();
        PostProcessingManager.SetMotionBlur(_motionBlur);
        CameraShake.Shake(_cameraShake * _player.MovementScript.DashForce);
        _audio.PlayOneShot(_dashSFX);
    }

    public virtual void StopFX()
    {
        if(!_initialized) return;
        PostProcessingManager.SetMotionBlur(0f);
        _healthManager.BlinkFX.Stop();
    }
}
