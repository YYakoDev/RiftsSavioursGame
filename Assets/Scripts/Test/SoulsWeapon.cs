using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsWeapon : MonoBehaviour
{
    //[SerializeField] Animator _playerAnimator;
    [SerializeField] Animator _weaponAnimator;
    [SerializeField] TestWeaponSlash _weaponSlash;
    //Animator _parentAnimator;
    [SerializeField]private LayerMask _enemyLayer;
    [SerializeField]float _attackCooldown = 0.3f;
    float _nextAttackTime;

    [SerializeField]AudioSource _weaponSound;
    [SerializeField] AudioClip _atkSound;

    int atkIndex;
    string AttackAnim => (atkIndex > 0) ? "Atk2" : "Atk";
    float SlashYDirection => (atkIndex > 0) ? -1 : 1;

    private void Start() {
        YYInputManager.GetKey(KeyInputTypes.Attack).OnKeyHold += TryAttack;
    }
    void TryAttack()
    {
        if(Time.time > _nextAttackTime) Attack();
    }

    void Attack()
    {
        _nextAttackTime = Time.time + _attackCooldown;
        _weaponAnimator.Play(AttackAnim);
        _weaponSound.PlayOneShot(_atkSound);
        _weaponSlash.Play();
        Vector3 flippedScale = _weaponSlash.transform.localScale;
        flippedScale.y = SlashYDirection;
        _weaponSlash.transform.localScale = flippedScale;
        SetAtkIndex();
        Collider2D[] hittedEnemies =  Physics2D.OverlapCircleAll(_weaponSlash.transform.position, 2f, _enemyLayer);
        if(hittedEnemies.Length == 0) return;

        List<GameObject> hittedEnemiesGO = new List<GameObject>();
        for(int i = 0; i < hittedEnemies.Length; i++)
        {
            if(hittedEnemiesGO.Contains(hittedEnemies[i].gameObject))
            {
                continue;
            };
            hittedEnemiesGO.Add(hittedEnemies[i].gameObject);
        }
        

        for(int i = 0; i < hittedEnemiesGO.Count; i++)
        {
            if(hittedEnemiesGO[i] == null)continue;

            if(hittedEnemiesGO[i].TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(11);
                //PopupsManager.Create(hittedEnemiesGO[i].transform.position + Vector3.up * 0.75f, 11);
            }
            if(hittedEnemiesGO[i].gameObject.TryGetComponent<IKnockback>(out IKnockback knockbackable))
            {
                knockbackable.KnockbackLogic.SetKnockbackData(transform, 1f);
            }
        }

        //projectile.transform.SetParent(null);


    }

    void SetAtkIndex()
    {
        atkIndex++;
        if(atkIndex > 1) atkIndex = 0;
    }

    private void OnDestroy() {
        YYInputManager.GetKey(KeyInputTypes.Attack).OnKeyHold -= TryAttack;
    }
}
