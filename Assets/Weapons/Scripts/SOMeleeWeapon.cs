using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = MenuPath + "SOMeleeWeapon")]
public class SOMeleeWeapon : WeaponBase
{
    protected Transform _parentTransform;
    protected Animator _weaponAnimator;
    protected AudioSource _audioSource;
    LayerMask _enemyLayer;
    AudioSource _effectsAudio;
    Camera _mainCamera;
    public event Action onHolding;
    
    AnimationCurve _cameraAnimCurve, _slowdownCurve;
    protected float _radiusOffset = 0;
    protected Vector3 _attackPoint = Vector2.zero;
    protected int _maxEnemiesToHit = 10;
    protected bool _delayingAnimation = false;
    private readonly int AtkAnim = Animator.StringToHash("Attack");
    protected List<GameObject> _hittedEnemiesGO = new();
    protected Timer _atkExecutionTimer, _delayTimer;


    [Header("Stats")]
    [SerializeField]MeleeWeaponStat _baseStats;
    MeleeWeaponStat _modifiedStats;
    [SerializeField] protected Vector2 _rangeOffset;
    [SerializeField]ComboAttack[] _comboAttacks = new ComboAttack[3];
    bool _holding;
    float _holdTime;
    Timer _releaseAtkTimer, _holdTriggerTimer;
    WeaponEffects[] _attackEffects, _heavyAtkEffectsInstance;
    const float MediumAtkThreshold = 0.4f, HeavyAtkThreshold = 0.75f; // with this you could have special effects play when this thresholds are met, also when you attack and hit
    [Header("Heavy Attack")]
    [SerializeField] MeleeWeaponStat _heavyAttackBonus;
    [SerializeField, Range(0, 50)] int _cameraZoomPercent = 25;
    [SerializeField] WeaponEffects[] _heavyAtkEffects;
    [SerializeField] AudioClip _heavyAtkCueSFX, _heavyAtkChargeUpSfx, _heavyAtkSound;
    float _cameraStartSize;

    //properties
    public float GetAtkSpeed() => _modifiedStats._atkSpeed;
    public float GetPullForce() => _modifiedStats._pullForce;
    public override float GetWeaponCooldown() => _modifiedStats._cooldown;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        _weaponAnimator = prefabTransform.GetComponent<Animator>();
        _audioSource = prefabTransform.GetComponent<AudioSource>();
        _modifiedStats = _baseStats;
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;
        base.Initialize(weaponManager, prefabTransform);

        _nextAttackTime = 0f;
        _holdTime = 0f;
        _weaponAnimator.speed = _baseStats._atkSpeed;
        SetMaxEnemiesToHit(_baseStats._atkRange);
        SetRadiusOffset(_baseStats._atkRange);

        _atkExecutionTimer = new(_baseStats._damageDelay, false);
        _atkExecutionTimer.onEnd += DoAttackLogic;
        _atkExecutionTimer.Stop();

        _holdTriggerTimer = new(0.15f);
        _holdTriggerTimer.Stop();
        _holdTriggerTimer.onEnd += HoldingLogic;

        _releaseAtkTimer = new(HeavyAtkThreshold + 0.05f);
        _releaseAtkTimer.Stop();
        _releaseAtkTimer.onEnd += TryAttack;

        _effectsAudio = _weaponManager.AtkEffects.Audio;
        _mainCamera = HelperMethods.MainCamera;
        _cameraStartSize = _mainCamera.orthographicSize;
        _cameraAnimCurve = TweenCurveLibrary.EaseInExpo;
        _slowdownCurve = TweenCurveLibrary.EaseInCirc;
        //_delayTimer = new(_animationDelayTime);
        //_delayTimer.Stop();
        //_delayTimer.onEnd += StopAnimationDelay;

    }

    protected override void InitializeFXS()
    {
        base.InitializeFXS();
        /*if (WeaponEffects == null) return;
        Array.Resize<WeaponEffects>(ref _attackEffects, WeaponEffects.Length);
        for (int i = 0; i < _attackEffects.Length; i++)
        {
            var baseFx = WeaponEffects[i];
            _attackEffects[i] = GameObject.Instantiate(baseFx);
            _attackEffects[i]?.Initialize(this);
        }
        _usedEffects = _attackEffects;

        Array.Resize<WeaponEffects>(ref _heavyAtkEffectsInstance, _attackEffects.Length + _heavyAtkEffects.Length);
        for (int i = 0; i < _heavyAtkEffectsInstance.Length; i++)
        {
            if(i < WeaponEffects.Length)
            {
                _heavyAtkEffectsInstance[i] = _attackEffects[i];
            }else
            {
                var fx = _heavyAtkEffects[i - WeaponEffects.Length];
                _heavyAtkEffectsInstance[i] = GameObject.Instantiate(fx);
                _heavyAtkEffectsInstance[i]?.Initialize(this);
            }
            
            
        }*/
    }

    public override void SetWeaponActive(bool active)
    {
        base.SetWeaponActive(active);
        if(active)_weaponAnimator.speed = _modifiedStats._atkSpeed;
    }

    protected override void SubscribeInput()
    {
        _attackKey.OnKeyPressed += Hold;
        _attackKey.OnKeyUp += StopHolding;
    }
    public override void UnsubscribeInput()
    {
        _attackKey.OnKeyPressed -= Hold;
        _attackKey.OnKeyUp -= StopHolding;
    }

    void Hold()
    {
        if(_deactivated) return;
        if(_holding) return;
        if(_nextAttackTime >= Time.time) return;
        _holdTriggerTimer.Start();
        _effectsAudio.PlayWithVaryingPitch(_heavyAtkChargeUpSfx);
        _holding = true;
    }

    void HoldingLogic()
    {
        if(!_holding) return;
        _releaseAtkTimer.Start();
        Debug.Log("Holding");
    }

    void StopHolding()
    {
        _releaseAtkTimer.End();
        if(_holding) _holding = false;
    }

    protected override void TryAttack()
    {
        if(_deactivated) return;
        if(_holdTime < HeavyAtkThreshold) _effectsAudio.Stop();
        if(_nextAttackTime >= Time.time) return;
        CameraEffects.ResetScale();
        _holding = false;
        //see if the holdtime has reached any thresholds and execute the attacks based on that

        if(_holdTime < MediumAtkThreshold)
        {
            _modifiedStats = _baseStats;
        }else if(_holdTime >= MediumAtkThreshold && _holdTime < HeavyAtkThreshold)
        {
            _modifiedStats = _baseStats + (_heavyAttackBonus / 2f);
        }else //heavy atk
        {
            _modifiedStats = _baseStats + _heavyAttackBonus;
        }

        _weaponAnimator.speed = _modifiedStats._atkSpeed;
        _holdTime = 0f;
        Attack(_modifiedStats._cooldown);

    }

    public override void UpdateLogic()
    {
        if(_deactivated) return;
        _holdTriggerTimer.UpdateTime();
        _releaseAtkTimer.UpdateTime();
        _atkExecutionTimer.UpdateTime();
        if(_holding)
        {
            var percent = _holdTime / HeavyAtkThreshold;
            _holdTime += Time.deltaTime;
            var newSize = _cameraStartSize - _cameraStartSize * (float)(_cameraZoomPercent / 100f);
            _mainCamera.orthographicSize = Mathf.Lerp(_cameraStartSize, newSize, _cameraAnimCurve.Evaluate(percent)); //find a way to restore the camera size!
            var slowdownForce = Mathf.Lerp(1f, 0.4f, _slowdownCurve.Evaluate(percent));
            _weaponManager.AtkEffects.SlowdownPlayer(Time.deltaTime * 10f, slowdownForce);
            if(_holdTime > HeavyAtkThreshold)
            {
                _weaponManager.AtkEffects.SlowdownPlayer(100f, slowdownForce);
                _effectsAudio.Stop();
                _effectsAudio.PlayOneShot(_heavyAtkCueSFX);
                _holding = false;


                //TryAttack();
            }
        }
    }

    protected override void Attack(float cooldown)
    {
        //this calls the onAttackEvent and also sets the cooldown.
        base.Attack(cooldown);
        //StartDelay();
        //InstantiateFX();
        SetAttackPoint();
        if(!DetectEnemies(_modifiedStats._atkRange)) return;
        
        _atkExecutionTimer.Start();
    }
    protected void SetAttackPoint()
    {
        Vector3 prefabPos = _weaponPrefabTransform.position;
        Vector3 directionFromParent = prefabPos - _parentTransform.position;
        directionFromParent.Normalize();
        Vector2 rangeOffset = new Vector2(_rangeOffset.x * Mathf.Sign(directionFromParent.x), _rangeOffset.y * Mathf.Sign(directionFromParent.y));
        _attackPoint = _weaponPrefabTransform.position + (Vector3)rangeOffset + directionFromParent * _radiusOffset;

    }
    protected bool DetectEnemies(float atkRange)
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
    protected void DoAttackLogic()
    {
        AttackLogic(_modifiedStats._atkDmg, _modifiedStats._knockbackForce);
    }
    protected void AttackLogic(int damage, float knockbackForce)
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

    protected void ApplyDamage(Transform enemy, IDamageable entity, int damage)
    {
        int critRoll = Random.Range(0, 101);
        bool critHit = (_baseStats._criticalChance > critRoll);
        int realDamage = (critHit) ? (int)(damage * _baseStats._criticalDamageMultiplier) : damage;
        entity.TakeDamage(realDamage);
        PopupsManager.CreateDamagePopup(enemy.position + Vector3.up * 0.8f, realDamage, critHit);
        InvokeOnEnemyHit(enemy.position);
    }

    protected void SetRadiusOffset(float atkRange)
    {
        _radiusOffset = 0.5f * atkRange;
    }
    protected void SetMaxEnemiesToHit(float atkRange)
    {
        _maxEnemiesToHit = 10 + (int)(atkRange * 10f);
    }

    public override void EvaluateStats(SOPlayerAttackStats attackStats)
    {
        //codear esto para que se modifiquen las stats del arma pero sin escalar hasta el infinito sin querer
        //Mirar el oldweapon system!
        _modifiedStats._atkDmg = (int)((_baseStats._atkDmg + (attackStats.BaseDamageAddition - 1)) * attackStats.DamageMultiplier);
        _modifiedStats._atkRange = _baseStats._atkRange + (attackStats.AttackRange - 1f);
        _modifiedStats._cooldown = _attackCooldown - (attackStats.AttackCooldown - 1f);
        _modifiedStats._knockbackForce = _baseStats._knockbackForce +( attackStats.AttackKnockback - 1f);
        _modifiedStats._atkSpeed = _baseStats._atkSpeed + (attackStats.AttackSpeed - 1f);
        SetRadiusOffset(_modifiedStats._atkRange);
        SetMaxEnemiesToHit(_modifiedStats._atkRange);
        _modifiedStats._damageDelay = _baseStats._damageDelay / _modifiedStats._atkSpeed;
        _modifiedStats._cooldown = Mathf.Clamp(_modifiedStats._cooldown, 0.1855f, 100f);
    }

    [Serializable]
    protected class ComboAttack
    {
        [SerializeField] MeleeWeaponStat _stats;
        [SerializeField] WeaponEffects _effects;
        [SerializeField] AudioClip[] _sounds = new AudioClip[1];

        public MeleeWeaponStat Stats => _stats;
        public WeaponEffects Effects => _effects;
        public AudioClip AtkSound => _sounds[Random.Range(0, _sounds.Length)];
    }

    [System.Serializable]protected struct MeleeWeaponStat
    {
        public float _cooldown, _pullForce, _atkRange, _atkSpeed, _knockbackForce, _damageDelay, _criticalDamageMultiplier, _pullDuration;
        public int _atkDmg, _criticalChance, _staminaConsumption;
        public MeleeWeaponStat
        (float cooldown, float pullForce, float atkRange, float atkSpeed, float knockbackForce, float delay, float criticalDamageMultiplier, float pullDuration, int atkDmg, int criticalChance, int staminaConsumption)
        {
            _cooldown = cooldown;
            _pullForce = pullForce;
            _pullDuration = pullDuration;
            _atkRange = atkRange;
            _atkSpeed = atkSpeed;
            _knockbackForce = knockbackForce;
            _damageDelay = delay;
            _atkDmg = atkDmg;
            _criticalDamageMultiplier = criticalDamageMultiplier;
            _criticalChance = criticalChance;
            _staminaConsumption = staminaConsumption;
        }

        public static MeleeWeaponStat operator +(MeleeWeaponStat a, MeleeWeaponStat b)
        {
            return new MeleeWeaponStat
            (
                a._cooldown + b._cooldown,
                a._pullForce + b._pullForce,
                a._atkRange + b._atkRange,
                a._atkSpeed + b._atkSpeed,
                a._knockbackForce + b._knockbackForce,
                a._damageDelay + b._damageDelay,
                a._criticalDamageMultiplier + b._criticalDamageMultiplier,
                a._pullDuration + b._pullDuration,
                a._atkDmg + b._atkDmg,
                a._criticalChance + b._criticalChance,
                a._staminaConsumption + b._staminaConsumption
            );
        }

        public static MeleeWeaponStat operator -(MeleeWeaponStat a, MeleeWeaponStat b)
        {
            return new MeleeWeaponStat
            (
                a._cooldown - b._cooldown,
                a._pullForce - b._pullForce,
                a._atkRange - b._atkRange,
                a._atkSpeed - b._atkSpeed,
                a._knockbackForce - b._knockbackForce,
                a._damageDelay - b._damageDelay,
                a._criticalDamageMultiplier - b._criticalDamageMultiplier,
                a._pullDuration - b._pullDuration,
                a._atkDmg - b._atkDmg,
                a._criticalChance - b._criticalChance,
                a._staminaConsumption - b._staminaConsumption
            );
        }
        public static MeleeWeaponStat operator /(MeleeWeaponStat a, float b)
        {
            return new MeleeWeaponStat
            (
                a._cooldown / b,
                a._pullForce / b,
                a._atkRange / b,
                a._atkSpeed / b,
                a._knockbackForce / b,
                a._damageDelay / b,
                a._criticalDamageMultiplier / b,
                a._pullDuration / b,
                a._atkDmg / (int)b,
                a._criticalChance / (int)b,
                a._staminaConsumption / (int)b
            );
        }
    }

    private void OnValidate() {
        if(_comboAttacks.Length > 3)
        {
            Array.Resize<ComboAttack>(ref _comboAttacks, 3);
        }
    }
}

