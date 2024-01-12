using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffects : ScriptableObject
{
    public const string MenuPath = "ScriptableObjects/WeaponEffects/";
    protected PlayerAttackEffects _effects;
    protected WeaponBase _weapon;

    public virtual void Initialize(WeaponBase weapon)
    {
        _weapon = weapon;
        _effects = weapon.FxsScript;
    }
    public virtual void OnAttackFX()
    {
    }

    public virtual void OnHitFX(Vector3 pos)
    {
    }
}
