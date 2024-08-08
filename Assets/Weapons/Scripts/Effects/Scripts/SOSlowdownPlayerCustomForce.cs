using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SlowdownPlayerCustomForce")]
public class SOSlowdownPlayerCustomForce : WeaponEffects
{
    [SerializeField, Range(0, 100)] int forcePercentage = 50;

    public override void OnAttackFX()
    {
        base.OnAttackFX();
        var forceResult = 1 - (forcePercentage / 100);
        var duration = (_weapon.AtkDuration);
        _effects.SlowdownPlayer(duration, forceResult);
    }

}
