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

    private readonly int AttackAnim1 = Animator.StringToHash("Attack");
    private readonly int AttackAnim2 = Animator.StringToHash("Attack 2");
    private readonly int AttackAnim3 = Animator.StringToHash("Attack 3");
    private int[] _animations = new int[3];
    private int AtkAnimation =>  _animations[Random.Range(0,_animations.Length)];

    private void Awake() {
        gameObject.CheckComponent<SpriteRenderer>(ref _spriteRenderer);
        gameObject.CheckComponent<Animator>(ref _animator);

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


    private void OnDrawGizmosSelected() {
        if(_weaponBase == null) return;
        _weaponBase.DrawGizmos();
    }

    private void OnDestroy() {
        _weaponBase.onAttack -= PlayAttackAnimation;
    }
}
