using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboAttackStat
{
    [SerializeField]public float Cooldown, Range, KnockbackForce, AtkSpeed, PullForce;
    [SerializeField]public int Damage;

    public ComboAttackStat(float cooldown, float range, float knockbackForce, float atkSpeed, float pullForce, int damage)
    {
        Cooldown = cooldown;
        Range = range;
        KnockbackForce = knockbackForce;
        AtkSpeed = atkSpeed;
        PullForce = pullForce;
        Damage = damage;
    }
}
