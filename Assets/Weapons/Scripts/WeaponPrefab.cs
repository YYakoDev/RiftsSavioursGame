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

    private static readonly int AttackAnimation = Animator.StringToHash("Attack");

    private void Awake() {
        gameObject.CheckComponent<SpriteRenderer>(ref _spriteRenderer);
        gameObject.CheckComponent<Animator>(ref _animator);
    }
    
    IEnumerator Start()
    {
        yield return null;
        _spriteRenderer.sprite = _weaponBase.WeaponSprite;
        _spriteRenderer.flipX = _weaponBase.FlipSprite;
        _animator.runtimeAnimatorController = _weaponBase.AnimatorOverrideController;

        _weaponBase.onAttack += PlayAttackAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        _weaponBase.InputLogic();
    }

    void PlayAttackAnimation()
    {
        _animator.Play(AttackAnimation);
    }

    public void SetWeaponBase(WeaponBase weapon)
    {
        _weaponBase = weapon;
    }


    private void OnDrawGizmosSelected() {
        if(_weaponBase == null) return;
        _weaponBase.DrawGizmos();
    }

    private void OnDestroy() {
        _weaponBase.onAttack -= PlayAttackAnimation;
    }
}
