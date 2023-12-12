using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffects : ScriptableObject
{
    protected PlayerAttackEffects _effects;

    public virtual void Initialize(PlayerAttackEffects atkEffects)
    {
        _effects = atkEffects;
    }
    public virtual void OnAttackFX()
    {
    }

    public virtual void OnHitFX(Vector3 pos)
    {
    }
}
