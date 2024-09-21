using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponEffects/CameraShakeDefaultTimeFX")]
public class SOCameraShakeDefaultTime : WeaponEffects
{
    [SerializeField]float _strength;
    public override void OnHitFX(Transform enemy)
    {
        _effects.ScreenShake(_strength);
    }
}
