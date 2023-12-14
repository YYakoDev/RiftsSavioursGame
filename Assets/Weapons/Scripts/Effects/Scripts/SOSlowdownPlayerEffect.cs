using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SlowdownPlayer")]
public class SOSlowdownPlayerEffect : WeaponEffects
{
    public override void OnAttackFX()
    {
        _effects.SlowdownPlayer();
    }
}
