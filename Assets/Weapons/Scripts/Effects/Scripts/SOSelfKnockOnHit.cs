using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SelfKnockbackOnHit")]
public class SOSelfKnockOnHit : WeaponEffects
{
    [SerializeField, Range(0f, 33f)]float _amount = 0.33f;

    public override void OnHitFX(Vector3 pos)
    {
        _effects.KnockbackPlayer(_amount);

    }
}
