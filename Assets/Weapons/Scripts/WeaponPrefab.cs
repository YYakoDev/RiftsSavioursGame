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
    private readonly int AttackAnim1 = Animator.StringToHash("Attack");
    private readonly int AttackAnim2 = Animator.StringToHash("Attack 2");
    private readonly int AttackAnim3 = Animator.StringToHash("Attack 3");
    private int[] _animations = new int[3];
    private int AtkAnimation =>  _animations[Random.Range(0,_animations.Length)];

    //SFX 
    private AudioSource _audio;
    [SerializeField]private AudioClip _onEquipSFX;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<SpriteRenderer>(ref _spriteRenderer);
        thisGO.CheckComponent<Animator>(ref _animator);
        thisGO.CheckComponent<AudioSource>(ref _audio);

        _animations[0] = AttackAnim1;
        _animations[1] = AttackAnim2;
        _animations[2] = AttackAnim3;
    }
    
    IEnumerator Start()
    {
        yield return null;
        _spriteRenderer.sprite = _weaponBase.WeaponSprite;
        _spriteRenderer.flipX = _weaponBase.FlipSprite;

        _animator.runtimeAnimatorController = _weaponBase.AnimatorOverrideController;
        _animator.ForcePlay(OnEquipAnim);
        _weaponBase.onAttack += PlayAttackAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        _weaponBase.InputLogic();
    }

    void PlayAttackAnimation()
    {
        _animator.Play(AtkAnimation);
    }

    public void SetWeaponBase(WeaponBase weapon)
    {
        _weaponBase = weapon;
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
        _weaponBase.onAttack -= PlayAttackAnimation;
    }
}
