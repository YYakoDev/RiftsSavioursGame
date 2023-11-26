using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/TripleComboMeleeWeapon")]
public class TripleComboMeleeWeapon : MeleeWeapon
{
    float _inputTime;
    int currentComboIndex = 0;
    public float FollowUpCooldown = 0.1f;
    public float FullComboCooldown => _attackCooldown;
    public readonly int Atk1Hash = Animator.StringToHash("Attack");
    public readonly int Atk2Hash = Animator.StringToHash("Attack2");
    public readonly int Atk3Hash = Animator.StringToHash("Attack3");
    
    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
    }
    public override void InputLogic()
    {
        base.InputLogic();
    }
    protected override void Attack()
    {
        base.Attack();
    }
    protected override void EvaluateStats(SOPlayerAttackStats attackStats)
    {
        base.EvaluateStats(attackStats);
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();
    }
}
