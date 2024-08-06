using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "MuteWeaponOnHit")]
public class SOMuteWeaponOnHit : SOMeleeWeaponEffect
{
    public override void OnHitFX(Vector3 pos)
    {
        base.OnHitFX(pos);
        YYExtensions.i.ExecuteEventAfterTime(_meleeWeapon.AtkDuration / 2.5f, () =>
        {
            _meleeWeapon.Audio.Stop();
            YYExtensions.i.ExecuteEventAfterTime(_meleeWeapon.AtkDuration / 1.5f, () =>
            {
                _meleeWeapon.Audio.volume = 1f;
            });
        });
    }
}
