using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "PullForceFX")]
public class SOPullForceFX : SOMeleeWeaponEffect
{
    public override void OnAttackFX()
    {
        base.OnAttackFX();
        _effects.SelfPush(_meleeWeapon.GetPullForce(), _meleeWeapon.GetPullDuration());
    }    
}
