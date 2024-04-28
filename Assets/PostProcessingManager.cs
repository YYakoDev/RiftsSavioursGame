using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    static Volume globalVolume;

    private void Awake() {
        globalVolume = GetComponent<Volume>();
        if(globalVolume == null) gameObject.SetActive(false);
        SetVignette(1f);
        SetMotionBlur(1f);
    }

    public static void SetVignette(float intensity)
    {
        var vignette = AddOrGetEffect<Vignette>();
        vignette.intensity.Override(intensity);
    }

    public static void SetMotionBlur(float intensity)
    {
        var motionBlur = AddOrGetEffect<MotionBlur>();
        motionBlur.intensity.Override(intensity);
    }

    private static T AddOrGetEffect<T>(bool activateFX = true) where T : VolumeComponent
    {
        var profile = globalVolume.profile;
        T effect = null;
        if(profile.Has<T>()) profile.TryGet<T>(out effect);
        else effect = profile.Add<T>();
        effect.active = activateFX;
        return effect;
    }
}
