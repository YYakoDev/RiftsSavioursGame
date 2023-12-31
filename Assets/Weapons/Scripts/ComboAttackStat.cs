using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboAttackStat
{
    [SerializeField]public float Cooldown, Range, KnockbackForce, AtkSpeed, PullForce, AtkDelay;
    [SerializeField]public int Damage;
    [SerializeField]public Vector2 RangeOffset;
    [SerializeField]public WeaponEffects[] WeaponFxs;

    public ComboAttackStat
    (float cooldown, float range, float knockbackForce, float atkSpeed, float pullForce, int damage, float atkDelay, Vector2 rangeOffset, WeaponEffects[] fxs)
    {
        Cooldown = cooldown;
        Range = range;
        KnockbackForce = knockbackForce;
        AtkSpeed = atkSpeed;
        PullForce = pullForce;
        Damage = damage;
        AtkDelay = atkDelay;
        RangeOffset = rangeOffset;
        WeaponFxs = fxs;
    }
}
