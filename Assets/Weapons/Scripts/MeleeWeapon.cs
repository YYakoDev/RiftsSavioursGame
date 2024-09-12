using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "New Melee Weapon", menuName = MenuPath + "Melee Weapon")]
public class MeleeWeapon : WeaponBase
{
    [Header("References")]
    protected Transform _parentTransform;
    protected Animator _weaponAnimator;
    protected AudioSource _audioSource;
    protected LayerMask _enemyLayer;

    [Header("Stats")]
    protected MeleeWeaponStats _modifiedStats;
    [SerializeField, Range(0f, 10f)] protected float _pullForce = 0;
    [SerializeField, Range(0f,1.5f)] protected float _pullDuration = 0.1f;
    [SerializeField]protected float _attackRange = 0.5f;
    [SerializeField, Range(0.1f, 4f)] protected float _attackSpeed = 1f;
    [SerializeField] protected Vector2 _rangeOffset;
    protected float _radiusOffset = 0;
    protected Vector3 _attackPoint = Vector2.zero;
    [SerializeField]protected int _attackDamage = 5;
    [SerializeField, Range(0, 100)] protected int _criticalChance = 10;
    [SerializeField] protected float _criticalDamageMultiplier = 2;
    [SerializeField, Range(0f, 1f)] protected float _damageDelay = 0.2f;
    protected int _maxEnemiesToHit = 10;
    [SerializeField, Range(0f, 3.25f)]protected float _knockbackForce = 0.35f;
    [SerializeField, Range(0f,1f)] protected float _animationDelayTime = 0f;
    [SerializeField, Range(0f, 1f)] protected float _delayPercentage = 0f;
    protected bool _delayingAnimation = false;
    private readonly int AtkAnim = Animator.StringToHash("Attack");
    protected List<GameObject> _hittedEnemiesGO = new();
    protected Timer _atkExecutionTimer, _delayTimer;


    //properties
    public virtual float GetPullForce() => _modifiedStats._pullForce;
    public virtual float GetPullDuration() => _pullDuration;
    public override float GetWeaponCooldown() => _modifiedStats._cooldown;
    public virtual float GetAtkSpeed() => _modifiedStats._atkSpeed;
    public Animator Animator => _weaponAnimator;
    public AudioSource Audio => _audioSource;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _weaponAnimator = prefabTransform.GetComponent<Animator>();
        _audioSource = prefabTransform.GetComponent<AudioSource>();
        _weaponAnimator.speed = _attackSpeed;
        _nextAttackTime = 0;
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;
        SetMaxEnemiesToHit(_attackRange);
        SetRadiusOffset(_attackRange);
        _atkExecutionTimer = new(_damageDelay, false);
        _atkExecutionTimer.onEnd += DoAttackLogic;
        _atkExecutionTimer.Stop();

        _delayTimer = new(_animationDelayTime);
        _delayTimer.Stop();
        _delayTimer.onEnd += StopAnimationDelay;

        _modifiedStats = new(_attackCooldown, _pullForce, _attackRange, _attackSpeed, _knockbackForce, _damageDelay, _attackDamage, 0);
    }

    public override void UpdateLogic()
    {
        _atkExecutionTimer.UpdateTime();
        DelayLogic();
    }


    protected override void TryAttack(InputAction.CallbackContext obj)
    {
        if(_deactivated) return;
        if(_nextAttackTime >= Time.time) return;
        Attack(_modifiedStats._cooldown);
    }

    protected override void Attack(float cooldown)
    {
        //this calls the onAttackEvent and also sets the cooldown.
        base.Attack(cooldown);
        StartDelay();
        //InstantiateFX();
        SetAttackPoint();
        if(!DetectEnemies(_modifiedStats._atkRange)) return;
        
        _atkExecutionTimer.Start();
    }
    protected virtual void SetAttackPoint()
    {
        Vector3 prefabPos = _weaponPrefabTransform.position;
        Vector3 directionFromParent = prefabPos - _parentTransform.position;
        directionFromParent.Normalize();
        Vector2 rangeOffset = new Vector2(_rangeOffset.x * Mathf.Sign(directionFromParent.x), _rangeOffset.y * Mathf.Sign(directionFromParent.y));
        _attackPoint = _weaponPrefabTransform.position + (Vector3)rangeOffset + directionFromParent * _radiusOffset;

    }
    protected virtual bool DetectEnemies(float atkRange)
    {
        Collider2D[] hittedEnemies =  Physics2D.OverlapCircleAll(_attackPoint, atkRange, _enemyLayer);
        if(hittedEnemies.Length == 0) return false;

        _hittedEnemiesGO.Clear();
        for(int i = 0; i < hittedEnemies.Length; i++)
        {
            if(_hittedEnemiesGO.Contains(hittedEnemies[i].gameObject)) continue;
            _hittedEnemiesGO.Add(hittedEnemies[i].gameObject);
        }
        return true;
    }
    protected virtual void DoAttackLogic()
    {
        AttackLogic(_modifiedStats._atkDmg, _modifiedStats._knockbackForce);
    }
    protected virtual void AttackLogic(int damage, float knockbackForce)
    {
        if(_hittedEnemiesGO.Count == 0) return;
        for(int i = 0; i < _hittedEnemiesGO.Count; i++)
        {
            if(i >= _maxEnemiesToHit)break;
            var hitEnemy = _hittedEnemiesGO[i];
            if(hitEnemy == null || !hitEnemy.activeSelf)continue;

            if(hitEnemy.TryGetComponent<IDamageable>(out IDamageable damageable)) 
                ApplyDamage(hitEnemy.transform, damageable, damage);
            
            if(hitEnemy.TryGetComponent<IKnockback>(out var knockbackable)) 
                knockbackable.KnockbackLogic.SetKnockbackData(_parentTransform, knockbackForce);
            

        }
    }

    protected virtual void DelayLogic()
    {
        _delayTimer.UpdateTime();
        if(_delayingAnimation)
        {
            var elapsedTime = _animationDelayTime - _delayTimer.CurrentTime;
            var percent = elapsedTime / _animationDelayTime;
            var delaySpeed = _attackSpeed - _attackSpeed * _delayPercentage;
            var newSpeed = Mathf.Lerp(delaySpeed, _attackSpeed, percent);
            _weaponAnimator.speed = newSpeed;
        }
    }

    protected virtual void StartDelay()
    {
        _delayingAnimation = true;
        _delayTimer.Start(); 
    }
    protected virtual void StopAnimationDelay()
    {
        _delayingAnimation = false;
        _weaponAnimator.speed = _attackSpeed;
    }

    protected virtual void ApplyDamage(Transform enemy, IDamageable entity, int damage)
    {
        int critRoll = Random.Range(0, 101);
        bool critHit = (_criticalChance > critRoll);
        int realDamage = (critHit) ? (int)(damage * _criticalDamageMultiplier) : damage;
        entity.TakeDamage(realDamage);
        var popupType = (critHit) ? DamagePopupTypes.CriticalRed : DamagePopupTypes.Normal;
        PopupsManager.CreateDamagePopup(enemy.position + Vector3.up * 0.9f, realDamage, popupType);
        InvokeOnEnemyHit(enemy.position);
    }

    protected void SetRadiusOffset(float atkRange)
    {
        _radiusOffset = 0.5f * atkRange;
    }
    protected void SetMaxEnemiesToHit(float atkRange)
    {
        _maxEnemiesToHit = 5 + (int)(atkRange * 10);
    }
    public override void EvaluateStats(SOPlayerAttackStats attackStats)
    {
        //codear esto para que se modifiquen las stats del arma pero sin escalar hasta el infinito sin querer
        //Mirar el oldweapon system!
        _modifiedStats._atkDmg = (int)((_attackDamage + (attackStats.BaseDamageAddition - 1)) * attackStats.DamageMultiplier);
        _modifiedStats._atkRange = _attackRange + (attackStats.AttackRange - 1);
        _modifiedStats._cooldown = _attackCooldown - (attackStats.AttackCooldown - 1);
        _modifiedStats._knockbackForce = _knockbackForce +( attackStats.AttackKnockback - 1);
        _modifiedStats._atkSpeed = _attackSpeed + (attackStats.AttackSpeed - 1);
        SetRadiusOffset(_modifiedStats._atkRange);
        SetMaxEnemiesToHit(_modifiedStats._atkRange);
        _modifiedStats._damageDelay = _damageDelay / _modifiedStats._atkSpeed;
        _modifiedStats._cooldown = Mathf.Clamp(_modifiedStats._cooldown, 0.1855f, 100f);
    }

    public override void DrawGizmos()
    {
        DrawAttackRange(_attackRange);
    }
    protected void DrawAttackRange(float atkRange)
    {
        Gizmos.color = Color.red;
        Vector3 prefabPos = _weaponPrefabTransform.position;
        Vector3 directionFromParent = prefabPos - _parentTransform.position;
        directionFromParent.Normalize();
        Vector2 rangeOffset = new Vector2(_rangeOffset.x * Mathf.Sign(directionFromParent.x), _rangeOffset.y * Mathf.Sign(directionFromParent.y));
        Vector3 point = _weaponPrefabTransform.position + (Vector3)rangeOffset + directionFromParent * _radiusOffset;
        Gizmos.DrawWireSphere(point, atkRange);
    }
    protected struct MeleeWeaponStats
    {
        public float _cooldown, _pullForce, _atkRange, _atkSpeed, _knockbackForce, _damageDelay;
        public int _atkDmg, _staminaConsumption;

        public MeleeWeaponStats(float cooldown, float pullForce, float atkRange, float atkSpeed, float knockbackForce, float delay, int atkDmg, int staminaConsumption)
        {
            _cooldown = cooldown;
            _pullForce = pullForce;
            _atkRange = atkRange;
            _atkSpeed = atkSpeed;
            _knockbackForce = knockbackForce;
            _damageDelay = delay;
            _atkDmg = atkDmg;
            _staminaConsumption = staminaConsumption;
        }
    }
}
