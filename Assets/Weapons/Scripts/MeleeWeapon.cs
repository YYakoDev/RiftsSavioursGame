using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "ScriptableObjects/Weapons/Melee Weapon")]
public class MeleeWeapon : WeaponBase
{
    [Header("References")]
    //[SerializeField]WeaponFX _weaponFXPrefab;
    //GameObject _weaponFXObject;
    //WeaponFX _weaponFXInstance;
    Transform _parentTransform;
    LayerMask _enemyLayer;

    [Header("Stats")]
    [SerializeField]float _attackRange = 0.5f;
    [SerializeField]int _attackDamage = 5;
    int _maxEnemiesToHit = 10;
    [SerializeField, Range(0,2.25f)]float _knockbackForce = 0.35f;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _nextAttackTime = 0;
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;
        _maxEnemiesToHit = 3 + (int)(_attackRange * 5);
    }
    protected override void Attack()
    {
        //this calls the onAttackEvent and also sets the cooldown.
        base.Attack(); 
        //InstantiateFX();
        Collider2D[] hittedEnemies =  Physics2D.OverlapCircleAll(_weaponPrefabTransform.position, _attackRange, _enemyLayer);
        if(hittedEnemies.Length == 0) return;

        List<GameObject> hittedEnemiesGO = new List<GameObject>();
        for(int i = 0; i < hittedEnemies.Length; i++)
        {
            if(hittedEnemiesGO.Contains(hittedEnemies[i].gameObject)) continue;
            hittedEnemiesGO.Add(hittedEnemies[i].gameObject);
        }
        

        for(int i = 0; i < hittedEnemiesGO.Count; i++)
        {
            if(hittedEnemiesGO[i] == null)continue;
            if(i >= _maxEnemiesToHit)break;

            if(hittedEnemiesGO[i].TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(_attackDamage);
                PopupsManager.Create(hittedEnemiesGO[i].transform.position + Vector3.up * 0.75f, _attackDamage * 10);
            }
            if(hittedEnemiesGO[i].gameObject.TryGetComponent<IKnockback>(out var knockbackable))
            {
                knockbackable.KnockbackLogic.SetKnockbackData(_parentTransform.position, _knockbackForce);
            }
        }

    }

    /*public void InstantiateFX()
    {
        Vector3 spawnPosition = _weaponPrefabTransform.position + _weaponPrefabTransform.right * -1 * _attackRange;
        Vector3 flippedScale = _parentTransform.localScale;
        flippedScale.x *= flippedScale.y * -1;


        if(_weaponFXInstance == null)
        {
            _weaponFXObject = Instantiate(_weaponFXPrefab.gameObject, spawnPosition, Quaternion.identity);
            _weaponFXObject.transform.localScale = flippedScale;
            //_weaponFXObject.transform.parent = _parentTransform;
            _weaponFXInstance = _weaponFXObject.GetComponent<WeaponFX>();
            _weaponFXInstance.Initialize(_attackDuration);
        }else
        {
            _weaponFXObject.SetActive(true);
            _weaponFXObject.transform.position = spawnPosition;
            _weaponFXObject.transform.localScale = flippedScale;
            _weaponFXInstance.Initialize(_attackDuration);
        }
    }*/

    protected override void EvaluateStats(SOPlayerAttackStats attackStats)
    {
        //codear esto para que se modifiquen las stats del arma pero sin escalar hasta el infinito sin querer
        //Mirar el oldweapon system!
    }

    public override void DrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_weaponPrefabTransform.position, _attackRange);
    }
}
