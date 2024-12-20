using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "FreezeGameFX")]
public class SOFreezeGameEffect : WeaponEffects
{
    [SerializeField]float _freezeTime = 0.06f;
    public override void OnAttackFX()
    {
        base.OnAttackFX();
    }
    public override void OnHitFX(Transform pos)
    {
        base.OnHitFX(pos);
        _effects.FreezeGame(_freezeTime);
    }
}
