using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SelfKnockbackOnHit")]
public class SOSelfKnockOnHit : WeaponEffects
{
    [SerializeField, Range(0f, 33f)]float _amount = 0.33f;
    float _cooldown;
    float _nextApplyTime;
    public override void Initialize(WeaponBase weapon)
    {
        base.Initialize(weapon);
        _cooldown = weapon.AtkDuration - 0.1f;
    }
    public override void OnHitFX(Vector3 pos)
    {
        if(_nextApplyTime >= Time.time) return;
        _effects.KnockbackPlayer(pos, _amount);
        _nextApplyTime = Time.time + _cooldown;
    }
}
