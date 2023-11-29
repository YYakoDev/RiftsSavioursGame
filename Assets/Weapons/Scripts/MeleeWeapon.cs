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
    protected LayerMask _enemyLayer;

    [Header("Stats")]
    [SerializeField]protected float _attackRange = 0.5f;
    [SerializeField]protected int _attackDamage = 5;
    protected int _maxEnemiesToHit = 10;
    [SerializeField, Range(0,2.25f)]protected float _knockbackForce = 0.35f;
    private readonly int AtkAnim = Animator.StringToHash("Attack");
    protected List<GameObject> _hittedEnemiesGO = new();
    protected Timer _atkExecutionTimer;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _nextAttackTime = 0;
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;
        _maxEnemiesToHit = 3 + (int)(_attackRange * 5);

        _atkExecutionTimer = new(_attackDuration / 4f, false);
        _atkExecutionTimer.onEnd += DoAttackLogic;
        _atkExecutionTimer.Stop();
    }

    public override void InputLogic()
    {
        _atkExecutionTimer.UpdateTime();
        base.InputLogic();
    }

    protected override void Attack(float cooldown)
    {
        //this calls the onAttackEvent and also sets the cooldown.
        base.Attack(cooldown); 
        //InstantiateFX();
        Collider2D[] hittedEnemies =  Physics2D.OverlapCircleAll(_weaponPrefabTransform.position, _attackRange, _enemyLayer);
        if(hittedEnemies.Length == 0) return;

        _hittedEnemiesGO.Clear();
        for(int i = 0; i < hittedEnemies.Length; i++)
        {
            if(_hittedEnemiesGO.Contains(hittedEnemies[i].gameObject)) continue;
            _hittedEnemiesGO.Add(hittedEnemies[i].gameObject);
        }
        _atkExecutionTimer.ResetTime();
        _atkExecutionTimer.Start();
    }
    protected virtual void DoAttackLogic()
    {
        AttackLogic(_attackDamage, _knockbackForce);
    }
    protected void AttackLogic(int damage, float knockbackForce)
    {
        if(_hittedEnemiesGO.Count == 0) return;
        for(int i = 0; i < _hittedEnemiesGO.Count; i++)
        {
            if(i >= _maxEnemiesToHit)break;
            if(_hittedEnemiesGO[i] == null)continue;

            if(_hittedEnemiesGO[i].TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.TakeDamage(damage);
                PopupsManager.Create(_hittedEnemiesGO[i].transform.position + Vector3.up * 0.75f, damage);
            }
            if(_hittedEnemiesGO[i].TryGetComponent<IKnockback>(out var knockbackable))
            {
                knockbackable.KnockbackLogic.SetKnockbackData(_parentTransform.position, knockbackForce);
            }

            //you can spawn hit fx in this part
        }
        InvokeOnEnemyHit();
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
