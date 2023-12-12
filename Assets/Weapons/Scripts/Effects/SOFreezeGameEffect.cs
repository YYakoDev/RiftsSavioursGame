using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponEffects/FreezeGameFX")]
public class SOFreezeGameEffect : WeaponEffects
{
    [SerializeField]float _freezeTime = 0.06f;
    public override void OnAttackFX()
    {
        base.OnAttackFX();
    }
    public override void OnHitFX(Vector3 pos)
    {
        base.OnHitFX(pos);
        _effects.FreezeGame(_freezeTime);
    }
}
