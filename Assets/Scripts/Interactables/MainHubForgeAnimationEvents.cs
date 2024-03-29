using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubForgeAnimationEvents : MonoBehaviour
{
    [SerializeField] float _cameraShakeStrength = 2f;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _hammerFallSfx, _hammerEmergingSfx, _vulcanusEmergingSfx, _lavaDripSfx, _vulcanusJumpSfx, _vulcanusSpinningSfx, _vulcanusFallSfx;

    void PlaySfx(AudioClip clip)
    {
        if(_audio == null || clip == null) return;
        _audio.PlayWithVaryingPitch(clip);
    }
    public void PlayHammerFallSfx()
    {
        PlaySfx(_hammerFallSfx);
    }
    public void PlayHammerEmergingSfx()
    {
        PlaySfx(_hammerEmergingSfx);
    }
    public void PlayVulcanusEmergingSfx()
    {
        PlaySfx(_vulcanusEmergingSfx);
    }
    public void PlayLavaDripSfx()
    {
        PlaySfx(_lavaDripSfx);
    }
    public void PlayVulcanusJumpSfx()
    {
        PlaySfx(_vulcanusJumpSfx);
    }
    public void PlayVulcanusSpnningSfx()
    {
        PlaySfx(_vulcanusSpinningSfx);
    }
    
    public void PlayVulcanusFallSfx()
    {
        PlaySfx(_vulcanusFallSfx);
    }

    public void DoCameraShake()
    {
        CameraShake.Shake(_cameraShakeStrength);
    }
}
