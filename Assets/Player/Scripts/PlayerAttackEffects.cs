using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    [SerializeField]PlayerManager _player;
    [SerializeField]WeaponManager _weaponManager;
    [SerializeField]AudioSource _audio;
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

    void PlayAttackAnimation() => _player.AnimController.PlayStated(PlayerAnimationsNames.Attack, AttackDuration);
    public void SlowdownPlayer() => _player.MovementScript.SlowdownMovement(AttackDuration);   
    public void SelfPush(float force) => _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -force);
    void SelfPush() => _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, -PullForce);

    public void KnockbackPlayer(float knockbackAmount) => _player.MovementScript.KnockbackLogic.SetKnockbackData(_weaponPrefab, knockbackAmount);
    

    public void ScreenShake(float strength) => CameraShake.Shake(strength);
    public void ScreenShake(float strength, float duration) => CameraShake.Shake(strength, duration);
    public void FreezeGame(float time) => GameFreezer.FreezeGame(time);
    

    public void PlayAudio(AudioClip clip)
    {
        if(_audio == null) return;
        _audio.Stop();
        _audio.PlayWithVaryingPitch(clip);
    }

    private void OnDestroy()
    {
        _weaponManager.CurrentWeapon.onAttack -= AttackEffects;
    }

}
