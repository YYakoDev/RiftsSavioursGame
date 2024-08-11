using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOMeleeWeaponEffect : WeaponEffects
{
    protected MeleeWeapon _meleeWeapon;
    public override void Initialize(WeaponBase weapon)
    {
        base.Initialize(weapon);
        //Debug.Log(weapon.GetType().IsSubclassOf(typeof(MeleeWeapon)));
        var type = weapon.GetType();
        var isSubclass = type.IsSubclassOf(typeof(MeleeWeapon));
        if(!isSubclass  && type != typeof(MeleeWeapon))
        {
            Debug.Log("removing fx:  " + name);
            _weapon.RemoveFxFromList(this);
            return;
        }
        this._meleeWeapon = base._weapon as MeleeWeapon;
    }
}
