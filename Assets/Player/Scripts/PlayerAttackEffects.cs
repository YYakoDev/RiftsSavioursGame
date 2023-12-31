using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    [SerializeField]PlayerManager _player;
    [SerializeField]WeaponManager _weaponManager;
    Transform _weaponPrefab;

    float PullForce => _weaponManager.CurrentWeapon.GetPullForce();
    float AttackDuration => _weaponManager.CurrentWeapon.AtkDuration;
    public Transform WeaponPrefab => _weaponPrefab;


    private IEnumerator Start()
    {
        yield return null;
        yield return null;

        _weaponPrefab = _weaponManager.WeaponPrefab.transform;
        _weaponManager.CurrentWeapon.onAttack += AttackEffects;
        /*if(_weaponManager.CurrentWeapon.WeaponEffects == null) yield break;
        foreach(WeaponEffects fx in _weaponManager.CurrentWeapon.WeaponEffects)
        {
            fx.Initialize(this);
            _weaponManager.CurrentWeapon.onAttack += fx.OnAttackFX;
            _weaponManager.CurrentWeapon.onEnemyHit += fx.OnHitFX;
        }*/
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
        float xPoint = _weaponPrefab.position.x - transform.position.x;
        _player.MovementScript.CheckForFlip(xPoint, AttackDuration);
    }

    void PlayAttackAnimation()
    {
        _player.AnimController.PlayStated(PlayerAnimationsNames.Attack);
    }

    public void SlowdownPlayer()
    {
        _player.MovementScript.SlowdownMovement(AttackDuration);
    }

    /*public void SlowdownPlayer(float time)
    {
        _player.MovementScript.SlowdownMovement(time);
    }*/

    void SelfPush()
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -PullForce);
    }
    /*public void SelfPush(float pullForce)
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -pullForce);
    }*/

    public void KnockbackPlayer(float knockbackAmount)
    {
        _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, knockbackAmount);
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
        /*if(_weaponManager.CurrentWeapon.WeaponEffects == null) return;
        foreach(WeaponEffects fx in _weaponManager.CurrentWeapon.WeaponEffects)
        {
            _weaponManager.CurrentWeapon.onAttack -= fx.OnAttackFX;
            _weaponManager.CurrentWeapon.onEnemyHit -= fx.OnHitFX;
        }*/
    }

}
