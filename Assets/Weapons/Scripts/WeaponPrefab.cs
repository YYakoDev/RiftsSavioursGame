using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource), typeof(WhiteBlinkEffect))]
public class WeaponPrefab : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private WeaponBase _weaponBase;
    private WhiteBlinkEffect _blinkFX;
    private readonly int OnEquipAnim = Animator.StringToHash("OnEquip");
    //private readonly int AttackAnim1 = Animator.StringToHash("Attack");

    //SFX 
    private AudioSource _audio;
    [SerializeField]private AudioClip _onEquipSFX;

    public SpriteRenderer Renderer => _spriteRenderer;
    public Animator Animator => _animator;
    public WhiteBlinkEffect BlinkFX => _blinkFX;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<SpriteRenderer>(ref _spriteRenderer);
        thisGO.CheckComponent<Animator>(ref _animator);
        thisGO.CheckComponent<AudioSource>(ref _audio);
        thisGO.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);
    }
    
    private void OnEnable() {
        if(_animator.runtimeAnimatorController == null) return;
        _animator.Play(OnEquipAnim);
        
    }

    // Update is called once per frame
    void Update()
    {
        _weaponBase?.UpdateLogic();
    }

    void AttackEffects()
    {
        _blinkFX.Stop();
        PlayAttackAnimation();
        PlayAttackSound();
    }
    void PlayAttackAnimation()
    {
        _animator.Play(_weaponBase.Animation, 0, 0f);
        /*if(_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _weaponBase.Animation)
        {
            StartCoroutine(ReplayAnimation(_animator, _weaponBase.Animation));
        }*/
    }
    void PlayAttackSound()
    {
        _audio.PlayOneShot(_weaponBase.Sound);
    }

    IEnumerator ReplayAnimation(Animator animator, int hash)
    {
        while(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            yield return null;
        }
        _animator.Play(hash, 0, 0f);
    }

    public void SetWeaponBase(WeaponBase weapon)
    {
        weapon.WeaponEvents.OnAttack -= AttackEffects;
        _weaponBase = weapon;
        Animator.enabled = false;
        _animator.runtimeAnimatorController = null;
        _spriteRenderer.enabled = false;
        _spriteRenderer.sprite = null;
        _spriteRenderer.sprite = weapon.SpriteAndAnimationData.Sprite;
        _spriteRenderer.flipX = weapon.FlipSprite;
        Renderer.color = Color.white;
        _spriteRenderer.enabled = true;
        _animator.runtimeAnimatorController = weapon.SpriteAndAnimationData.AnimatorOverride;
        Animator.enabled = true;
        weapon.WeaponEvents.OnAttack += AttackEffects;
        PlayUnsheateSFX();
    }

    //THIS IS BEING CALLED BY AN ANIMATION EVENT
    public void PlayUnsheateSFX()
    {
        _audio.PlayWithVaryingPitch(_onEquipSFX);
    }


    private void OnDrawGizmosSelected() {
        if(_weaponBase == null) return;
        _weaponBase.DrawGizmos();
    }

    private void OnDestroy() {
        if(_weaponBase != null)_weaponBase.WeaponEvents.OnAttack -= AttackEffects;
    }
}

