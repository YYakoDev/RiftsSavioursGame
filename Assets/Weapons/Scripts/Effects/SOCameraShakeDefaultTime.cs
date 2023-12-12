using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponEffects/CameraShakeDefaultTimeFX")]
public class SOCameraShakeDefaultTime : WeaponEffects
{
    [SerializeField]float _strength;
    public override void OnHitFX(Vector3 pos)
    {
        _effects.ScreenShake(_strength);
    }
}
