using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "KnockbackPlayer")]
public class SOKnockbackPlayerEffect : WeaponEffects
{
    [SerializeField] float _amount;
    public override void OnAttackFX()
    {
        _effects.KnockbackPlayer(_amount);
    }
}
