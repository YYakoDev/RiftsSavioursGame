using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class WeaponPrefab : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private WeaponBase _weaponBase;
    private readonly int OnEquipAnim = Animator.StringToHash("OnEquip");
    //private readonly int AttackAnim1 = Animator.StringToHash("Attack");

    //SFX 
    private AudioSource _audio;
    [SerializeField]private AudioClip _onEquipSFX;

    public SpriteRenderer Renderer => _spriteRenderer;
    public Animator Animator => _animator;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<SpriteRenderer>(ref _spriteRenderer);
        thisGO.CheckComponent<Animator>(ref _animator);
        thisGO.CheckComponent<AudioSource>(ref _audio);
    }
    
    IEnumerator Start()
    {
        yield return null;
       
        //_animator.ForcePlay(OnEquipAnim);
        _weaponBase.onAttack += AttackEffects;
    }

    private void OnEnable() {
        _animator.ForcePlay(OnEquipAnim);
        
    }

    // Update is called once per frame
    void Update()
    {
        _weaponBase.UpdateLogic();
    }

    void AttackEffects()
    {
        PlayAttackAnimation();
        PlayAttackSound();
    }
    void PlayAttackAnimation()
    {
        _animator.Play(_weaponBase.Animation);
        Debug.Log("Playing attack animation");
        if(_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _weaponBase.Animation)
        {
            StartCoroutine(ReplayAnimation(_animator, _weaponBase.Animation));
        }
    }
    void PlayAttackSound()
    {
        _audio.PlayWithVaryingPitch(_weaponBase.Sound);
    }

    IEnumerator ReplayAnimation(Animator animator, int hash)
    {
        while(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            yield return null;
        }
        _animator.Play(hash);
    }

    public void SetWeaponBase(WeaponBase weapon)
    {
        _weaponBase = weapon;
        _spriteRenderer.sprite = _weaponBase.SpriteAndAnimationData.Sprite;
        _spriteRenderer.flipX = _weaponBase.FlipSprite;

        _animator.runtimeAnimatorController = _weaponBase.SpriteAndAnimationData.AnimatorOverride;
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
        _weaponBase.onAttack -= AttackEffects;
    }
}
