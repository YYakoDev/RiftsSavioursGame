using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOMeleeWeaponEffect : WeaponEffects
{
    protected SOMeleeWeapon _meleeWeapon;
    public override void Initialize(WeaponBase weapon)
    {
        base.Initialize(weapon);
        //Debug.Log(weapon.GetType().IsSubclassOf(typeof(MeleeWeapon)));
        var type = weapon.GetType();
        var isSubclass = type.IsSubclassOf(typeof(SOMeleeWeapon));
        if(!isSubclass  && type != typeof(SOMeleeWeapon))
        {
            Debug.Log("removing fx:  " + name + "  from:  " + weapon.name);
            _weapon.RemoveFxFromList(this);
            return;
        }
        this._meleeWeapon = base._weapon as SOMeleeWeapon;
    }

    public override void OnAttackFX()
    {
        base.OnAttackFX();
        if(_meleeWeapon == null)
        {
        }
    }
}
