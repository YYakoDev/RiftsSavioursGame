using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponEffects/CameraShakeFX")]
public class SOCameraShakeEffect : WeaponEffects
{
    [SerializeField]float _strength;
    [SerializeField, Range(0f, 0.035f)]float _duration;
    public override void OnAttackFX()
    {
        
    }
    public override void OnHitFX(Vector3 pos)
    {
        _effects.ScreenShake(_strength, _duration);
    }
}
