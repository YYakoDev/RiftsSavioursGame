using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
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
        _weaponManager.CurrentWeapon.onAttack += AttackEffects;
        if(_weaponManager.CurrentWeapon.WeaponEffects == null) yield break;
        foreach(WeaponEffects fx in _weaponManager.CurrentWeapon.WeaponEffects)
        {
            fx.Initialize(this);
            _weaponManager.CurrentWeapon.onAttack += fx.OnAttackFX;
            _weaponManager.CurrentWeapon.onEnemyHit += fx.OnHitFX;
        }
        //_weaponManager.CurrentWeapon.onEnemyHit += OnHitEffects;

    }

    void AttackEffects()
    {
        FlipPlayer();
        PlayAttackAnimation();
        SelfPush();
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
    public void SlowdownPlayer(float time)
    {
        _player.MovementScript.SlowdownMovement(time);
    }

    void SelfPush()
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -PullForce);
    }
    public void SelfPush(float pullForce)
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -pullForce);
    }

    public void KnockbackPlayer(float knockbackAmount)
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, knockbackAmount);
    }

    void ScreenShake()
    {
        CameraShake.Shake(_shakeStrength, _shakeDuration);
    }
    public void ScreenShake(float strength)
    {
        CameraShake.Shake(strength);
    }
    public void ScreenShake(float strength, float duration)
    {
        CameraShake.Shake(strength, duration);
    }

    public void FreezeGame(float time)
    {
        GameFreezer.FreezeGame(time);
    }


    private void OnDestroy()
    {
        _weaponManager.CurrentWeapon.onAttack -= AttackEffects;
        if(_weaponManager.CurrentWeapon.WeaponEffects == null) return;
        foreach(WeaponEffects fx in _weaponManager.CurrentWeapon.WeaponEffects)
        {
            _weaponManager.CurrentWeapon.onAttack -= fx.OnAttackFX;
            _weaponManager.CurrentWeapon.onEnemyHit -= fx.OnHitFX;
        }
        //_weaponManager.CurrentWeapon.onAttack -= AttackEffects;
        //_weaponManager.CurrentWeapon.onEnemyHit -= OnHitEffects;
    }

}
