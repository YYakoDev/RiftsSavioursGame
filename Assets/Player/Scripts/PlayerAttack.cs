using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]PlayerManager _player;
    [SerializeField]WeaponParentAiming _weaponAiming;
    [SerializeField]WeaponManager _weaponManager;
    [SerializeField]float _shakeStrength, _shakeDuration, _gameFreezeTime;
    Transform _weaponPrefab;

    float PullForce => _weaponManager.CurrentWeapon.GetPullForce();
    float AttackDuration => _weaponManager.CurrentWeapon.AtkDuration;


    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        _weaponPrefab = _weaponManager.WeaponPrefab.transform;
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
            if(_weaponAiming.PointingDirection.x != 0) _player.MovementScript.CheckForFlip(_weaponAiming.PointingDirection.x, AttackDuration);
            else
            {
                float xDirection = _weaponPrefab.position.x - transform.position.x;
                _player.MovementScript.CheckForFlip(xDirection, AttackDuration);
            }
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
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -PullForce);
    }

    void KnockbackPlayer()
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, 0.2f);
    }

    void ScreenShake()
    {
        CameraShake.Shake(_shakeStrength, _shakeDuration);
    }

    void FreezeGame()
    {
        GameFreezer.FreezeGame(_gameFreezeTime);
    }


    private void OnDestroy()
    {
        _weaponManager.CurrentWeapon.onAttack -= PlayerAttackEffects;
        _weaponManager.CurrentWeapon.onEnemyHit -= OnHitEffects;
    }

}
