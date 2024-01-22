using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboAttackStat
{
    [SerializeField]public float Cooldown, Range, KnockbackForce, AtkSpeed, PullForce, AtkDelay, PullDuration;
    [SerializeField]public int Damage;
    [SerializeField]public Vector2 RangeOffset;
    [SerializeField]private WeaponEffects[] WeaponFxs;
    [HideInInspector]public WeaponEffects[] UsedEffects = new WeaponEffects[0];
    public ComboAttackStat
    (float cooldown, float range, float knockbackForce, float atkSpeed, float pullForce, int damage, float atkDelay, float pullDuration, Vector2 rangeOffset, WeaponEffects[] fxs)
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
        Initialize();
    }

    public void Initialize()
    {
        SetFXArray();
    }

    void SetFXArray()
    {

        if(WeaponFxs == null) return;
        Array.Resize<WeaponEffects>(ref UsedEffects, WeaponFxs.Length);
        for (int i = 0; i < UsedEffects.Length; i++)
        {
            var baseFx = WeaponFxs[i];
            UsedEffects[i] = GameObject.Instantiate(baseFx);
        }
    }
}
