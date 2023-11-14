using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "ScriptableObjects/Weapons/Melee Weapon")]
public class MeleeWeapon : WeaponBase
{
    [Header("References")]
    [SerializeField]WeaponFX _weaponFXPrefab;
    GameObject _weaponFXObject;
    WeaponFX _weaponFXInstance;
    Transform _parentTransform;
    LayerMask _enemyLayer;
    //private Animator _weaponAnimator;
    private AudioSource _audio;
    [SerializeField]AudioClip[] _weaponSounds;
    private AudioClip _sound => _weaponSounds[Random.Range(0, _weaponSounds.Length)];

    [Header("Stats")]
    [SerializeField]float _attackRange = 0.5f;
    [SerializeField]int _attackDamage = 5;
    int _maxEnemiesToHit = 10;
    [SerializeField, Range(0,2.25f)]float _knockbackForce = 0.1f;
    //float _followupTime;
    //[SerializeField]float _comboInputTime;
    //bool _executeCombo;
    //int _attacksExecuted = 0;
    //Timer _atkDurationTimer;
    //bool _waitForComboInput;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _nextAttackTime = 0;
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;
        _maxEnemiesToHit = 2 + (int)(_attackRange * 5);

        _audio = prefabTransform.GetComponent<AudioSource>();
    }
    protected override void Attack()
    {
        base.Attack(); //this calls the onAttackEvent and also sets the cooldown  i use this to play the attack animation and stop the autotargetting and other things
        _audio?.PlayWithVaryingPitch(_sound);
        
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
            if(hittedEnemiesGO[i].gameObject.TryGetComponent<IKnockback>(out IKnockback knockbackable))
            {
                knockbackable.KnockBackLogic.ApplyForce(_parentTransform.position, _knockbackForce);
            }
        }

    }
    protected override void EvaluateStats(SOPlayerAttackStats attackStats)
    {
        //codear esto para que se modifiquen las stats del arma pero sin escalar hasta el infinito sin querer
        //Mirar el oldweapon system!
    }

    public override void DrawGizmos()
    {
        //if(_weaponFXObject == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_weaponPrefabTransform.position, _attackRange);
    }
}
