using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/DashData")]
public class SODashData : ScriptableObject
{
    bool _initialized = false;
    [SerializeField] AnimatorOverrideController _dashAnimation;
    [SerializeField] float _motionBlur = 0.4f, _cameraShake = 0.4f, _dashDuration = 0.3f, _dashForceMultiplier = 0.9f;
    [SerializeField] bool _doBlinkFX = true, _abovePlayer = true;
    [SerializeField] AudioClip _dashSFX;

    //properties
    public AnimatorOverrideController DashAnimator => _dashAnimation;
    public float DashDuration => _dashDuration;
    public float ForceMultiplier => _dashForceMultiplier;
    public bool AbovePlayer => _abovePlayer;
    public bool DoBlinkFX => _doBlinkFX;
    public AudioClip DashSfx => _dashSFX;

    public void Initialize()
    {
        _initialized = true;
    }
    public virtual void PlayFX()
    {
        if(!_initialized) return;

        PostProcessingManager.SetMotionBlur(_motionBlur);
        CameraEffects.Shake(2f * _cameraShake);
        
    }

    public virtual void StopFX()
    {
        if(!_initialized) return;
        PostProcessingManager.SetMotionBlur(0f);
    }
}
