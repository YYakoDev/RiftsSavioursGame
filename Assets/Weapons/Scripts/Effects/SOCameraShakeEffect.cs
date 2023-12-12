using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponEffects/CameraShakeFX")]
public class SOCameraShakeEffect : WeaponEffects
{
    [SerializeField]float _strength;
    [SerializeField]float _duration;
    public override void OnAttackFX()
    {
        
    }
    public override void OnHitFX(Vector3 pos)
    {
        _effects.ScreenShake(_strength, _duration);
    }
}
