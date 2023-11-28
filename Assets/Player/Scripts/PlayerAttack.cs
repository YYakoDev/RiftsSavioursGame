using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]PlayerManager _player;
    [SerializeField]WeaponParentAiming _weaponAiming;
    [SerializeField]WeaponManager _weaponManager;
    [SerializeField, Range(0f,2f)]float _weaponPullForce = 0.4f;

    float AttackDuration => _weaponManager.CurrentWeapon.AtkDuration;


    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        _weaponManager.CurrentWeapon.onAttack += PlayerAttackEffects;
        _weaponManager.CurrentWeapon.onEnemyHit += OnHitEffects;
    }

    void PlayerAttackEffects()
    {
        FlipPlayer();
        PlayAttackAnimation();
        SlowdownPlayer();
        SelfPush();
    }
    void OnHitEffects()
    {
        FreezeGame();
        KnockbackPlayer();
        ScreenShake();
    }
    void FlipPlayer()
    {
        if(_weaponAiming.AutoTargetting)
        {
            _player.MovementScript.CheckForFlip(_weaponAiming.PointingDirection.x, AttackDuration);
        }else
        {
            _player.MovementScript.CheckForFlip(_weaponAiming.MouseDirection.x, AttackDuration);
        }
    }

    void PlayAttackAnimation()
    {
        _player.AnimController.PlayStated(PlayerAnimationsNames.Attack);
    }

    void SlowdownPlayer()
    {
        _player.MovementScript.SlowdownMovement(AttackDuration);
    }

    void SelfPush()
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponManager.CurrentWeapon.PrefabTransform.position, -_weaponPullForce);
    }

    void KnockbackPlayer()
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponManager.CurrentWeapon.PrefabTransform.position, 0.2f);
    }

    void ScreenShake()
    {
        CameraShake.Shake(0.6f);
    }

    void FreezeGame()
    {
        GameFreezer.FreezeGame(0.04f);
    }


    private void OnDestroy()
    {
        _weaponManager.CurrentWeapon.onAttack -= PlayerAttackEffects;
        _weaponManager.CurrentWeapon.onEnemyHit -= OnHitEffects;
    }

}
