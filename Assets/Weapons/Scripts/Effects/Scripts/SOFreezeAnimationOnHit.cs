using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "FreezeAnimationOnHIT")]
public class SOFreezeAnimationOnHit : SOMeleeWeaponEffect
{
    [SerializeField] float _freezeTime = 0.1f;
    public override void Initialize(WeaponBase weapon)
    {
        base.Initialize(weapon);
    }

    public override void OnHitFX(Transform pos)
    {
        base.OnHitFX(pos);
        _meleeWeapon.Animator.speed = 0f;
        YYExtensions.i.ExecuteEventAfterTime(_freezeTime, ResetAnimatorSpeed);
    }

    void ResetAnimatorSpeed()
    {
        _meleeWeapon.Animator.speed = _meleeWeapon.GetAtkSpeed();
    }
}
