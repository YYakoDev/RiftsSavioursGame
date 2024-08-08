using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffects : MonoBehaviour
{
    Camera _mainCamera;
    [SerializeField]WeaponManager _weaponManager;
    [SerializeField]PlayerManager _player;
    [SerializeField]CameraTarget _cameraTargetting;
    int _targetIndex = -1;
    [SerializeField]AudioSource _audio;
    WeaponBase _currentWeapon;
    Transform weaponPrefab => _currentWeapon.PrefabTransform;
    public AudioSource Audio => _audio;
    float AttackDuration => _currentWeapon.AtkDuration;


    private void Awake() {
        _weaponManager.OnWeaponChange += SwitchCurrentWeapon;
    }

    private IEnumerator Start()
    {
        _mainCamera = Camera.main;
        yield return null;
        yield return null;
        _targetIndex = _cameraTargetting.AddTarget(weaponPrefab);
        
    }

    void SwitchCurrentWeapon(WeaponBase weapon)
    {
        if(_currentWeapon != null) _currentWeapon.onAttack -= AttackEffects;
        _currentWeapon = weapon;
        _currentWeapon.onAttack += AttackEffects;
    }
    

    void AttackEffects()
    {
        FlipPlayer();
        PlayAttackAnimation();
        if(_currentWeapon.PointCameraOnAttack)_cameraTargetting.SwitchTarget(_targetIndex, AttackDuration + 0.25f);
    }
    
    void PlayAttackAnimation() => _player.AnimController.PlayStated(PlayerAnimationsNames.Attack, AttackDuration);
    void FlipPlayer() => _player.MovementScript.FlipLogic.FlipCheck(weaponPrefab.position.x - _player.Position.x, 0.25f);
    public void SlowdownPlayer() => _player.MovementScript.SlowdownMovement(AttackDuration);   
    public void SlowdownPlayer(float force) => _player.MovementScript.SlowdownMovement(AttackDuration, force);   
    public void SlowdownPlayer(float duration, float force) => _player.MovementScript.SlowdownMovement(duration, force);   
    public void SelfPush(float force) => _player.MovementScript.KnockbackLogic.SetKnockbackData(_mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition), -force, ignoreResistance: true);
    public void SelfPush(float force, float duration) => _player.MovementScript.KnockbackLogic.SetKnockbackData(_mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition), -force, duration, true);

    public void KnockbackPlayer(float knockbackAmount) => _player.MovementScript.KnockbackLogic.SetKnockbackData(weaponPrefab.position, knockbackAmount, forceMultiplier: 0.25f);
    public void KnockbackPlayer(Vector3 emitterPosition, float knockbackAmount) => _player.MovementScript.KnockbackLogic.SetKnockbackData(emitterPosition, knockbackAmount, forceMultiplier: 0.25f);
    

    public void ScreenShake(float strength) => CameraEffects.Shake(strength);
    public void ScreenShake(float strength, float duration) => CameraEffects.Shake(strength, duration);
    public void FreezeGame(float time) => GameFreezer.FreezeGame(time);
    

    public void PlayAudio(AudioClip clip)
    {
        if(_audio == null) return;
        _audio.Stop();
        _audio.PlayWithVaryingPitch(clip);
    }

    private void OnDestroy()
    {
        _weaponManager.OnWeaponChange -= SwitchCurrentWeapon;
        _currentWeapon.onAttack -= AttackEffects;
    }

}
