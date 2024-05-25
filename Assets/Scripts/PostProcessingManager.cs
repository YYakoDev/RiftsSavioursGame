using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingManager : MonoBehaviour
{
    static Volume globalVolume;
    static Dictionary<VolumeComponent, float> _defaultValues = new();
    private void Awake() {
        globalVolume = GetComponent<Volume>();
        if(globalVolume == null) gameObject.SetActive(false);
        var effects = globalVolume.profile.components;
        for (int i = 0; i < effects.Count; i++)
        {
            var type = effects[i].GetType();
            if(type == typeof(Vignette))
            {
                var effect = effects[i] as Vignette;
                _defaultValues.Add(effect, ((float)effect.intensity));
            }else if(type == typeof(LensDistortion))
            {
                var effect = effects[i] as LensDistortion;
                _defaultValues.Add(effect, ((float)effect.intensity));
            }else if(type == typeof(MotionBlur))
            {
                var effect = effects[i] as MotionBlur;
                _defaultValues.Add(effect, ((float)effect.intensity));
            }
        }
        //SetVignette(1f);
        //SetMotionBlur(1f);
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
    public static void SetLensDistortion(float intensity)
    {
        var lensDist = AddOrGetEffect<LensDistortion>();
        lensDist.intensity.Override(intensity);
    }

    public static float GetDefaultValue<T>() where T : VolumeComponent
    {
        foreach(var keyValuePair in _defaultValues)
        {
            var type = keyValuePair.Key.GetType();
            if(type == typeof(T)) return keyValuePair.Value;
        }
        return 0f;
    }

    public static void RestoreDefaultValues()
    {  
        foreach(var componentPairValue in _defaultValues)
        {
            var type = componentPairValue.Key.GetType();
            if(type == typeof(Vignette))
            {
                SetVignette(componentPairValue.Value);

            }else if(type == typeof(LensDistortion))
            {
                SetLensDistortion(componentPairValue.Value);

            }else if(type == typeof(MotionBlur))
            {
                SetMotionBlur(componentPairValue.Value);
            }
        }

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
