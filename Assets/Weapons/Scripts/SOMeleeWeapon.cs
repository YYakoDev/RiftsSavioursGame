using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = MenuPath + "SOMeleeWeapon")]
public class SOMeleeWeapon : MeleeWeapon
{
    AudioSource _effectsAudio;
    Camera _mainCamera;
    [SerializeField, Range(0, 50)] int _cameraZoomPercent = 25;
    float _cameraStartSize;
    AnimationCurve _cameraAnimCurve, _slowdownCurve;


    [Header("Stats")]
    new WeaponStats _modifiedStats;
    [SerializeField] private int _staminaConsumption;
    bool _holding;
    float _holdTime;
    const float MediumAtkThreshold = 0.4f, HeavyAtkThreshold = 0.75f; // with this you could have special effects play when this thresholds are met, also when you attack and hit
    WeaponEffects[] _attackEffects, _heavyAtkEffectsInstance;
    [SerializeField] WeaponEffects[] _heavyAtkEffects;
    [SerializeField] AudioClip _heavyAtkCueSFX, _heavyAtkChargeUpSfx;

    //properties
    public override float GetAtkSpeed() => _modifiedStats._atkSpeed;
    public override float GetPullForce() => _modifiedStats._pullForce;
    public override float GetWeaponCooldown() => _modifiedStats._cooldown;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        _modifiedStats = new(_attackCooldown, _pullForce, _attackRange, _attackSpeed, _knockbackForce, _damageDelay, _attackDamage);
        base.Initialize(weaponManager, prefabTransform);
        _nextAttackTime = 0f;
        _holdTime = 0f;
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;


        SetMaxEnemiesToHit(_attackRange);
        SetRadiusOffset(_attackRange);
        _atkExecutionTimer = new(_damageDelay, false);
        _atkExecutionTimer.onEnd += DoAttackLogic;
        _atkExecutionTimer.Stop();

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
        if (WeaponEffects == null) return;
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
            
            
        }
}

    public override void SetWeaponActive(bool active)
    {
        base.SetWeaponActive(active);
        if(active)_weaponAnimator.speed = _modifiedStats._atkSpeed;
    }

    protected override void SubscribeInput()
    {
        _attackKey.OnKeyPressed += Hold;
        _attackKey.OnKeyUp += TryAttack;
    }
    public override void UnsubscribeInput()
    {
        _attackKey.OnKeyPressed -= Hold;
        _attackKey.OnKeyUp -= TryAttack;
    }

    void Hold()
    {
        _holding = true;
        _effectsAudio.PlayWithVaryingPitch(_heavyAtkChargeUpSfx);
    }

    protected override void TryAttack()
    {
        if(_deactivated) return;
        CameraEffects.ResetScale();
        _holding = false;
        if(_holdTime < HeavyAtkThreshold) _effectsAudio.Stop();
        if(_nextAttackTime >= Time.time) return;
        //see if the holdtime has reached any thresholds and execute the attacks based on that

        if(_holdTime < MediumAtkThreshold)
        {
            _audioSource.pitch = 1f;
            _modifiedStats._pullForce = _pullForce;
            _modifiedStats._atkDmg = _attackDamage;
            _modifiedStats._atkSpeed = _attackSpeed;
            _modifiedStats._atkRange = _attackRange;
            _usedEffects = _attackEffects;
        }else if(_holdTime >= MediumAtkThreshold && _holdTime < HeavyAtkThreshold)
        {
            _modifiedStats._pullForce = _pullForce + 0.2f;
            _modifiedStats._atkDmg = _attackDamage + 1;
            _modifiedStats._atkSpeed = _attackSpeed;
            _modifiedStats._atkRange = _attackRange + 0.2f;
            _usedEffects = _attackEffects;
        }else //heavy atk
        {
            _modifiedStats._pullForce = _pullForce + 1f;
            _modifiedStats._atkDmg = _attackDamage + 3;
            _modifiedStats._atkSpeed = _attackSpeed + 0.2f;
            _modifiedStats._atkRange = _attackRange + 0.45f;
            _usedEffects = _heavyAtkEffectsInstance;
            _audioSource.pitch = 0.85f;
            Debug.Log("Heavy attack!");
        }

        _weaponAnimator.speed = _modifiedStats._atkSpeed;
        _holdTime = 0f;
        Attack(_modifiedStats._cooldown);

    }

    public override void UpdateLogic()
    {
        if(_deactivated) return;
        _atkExecutionTimer.UpdateTime();

        if(_holding)
        {
            var percent = _holdTime / HeavyAtkThreshold;
            _holdTime += Time.deltaTime;
            var newSize = _cameraStartSize - _cameraStartSize * (float)(_cameraZoomPercent / 100f);
            _mainCamera.orthographicSize = Mathf.Lerp(_cameraStartSize, newSize, _cameraAnimCurve.Evaluate(percent)); //find a way to restore the camera size!
            var slowdownForce = Mathf.Lerp(1f, 0f, _slowdownCurve.Evaluate(percent));
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
    protected override void SetAttackPoint()
    {
        Vector3 prefabPos = _weaponPrefabTransform.position;
        Vector3 directionFromParent = prefabPos - _parentTransform.position;
        directionFromParent.Normalize();
        Vector2 rangeOffset = new Vector2(_rangeOffset.x * Mathf.Sign(directionFromParent.x), _rangeOffset.y * Mathf.Sign(directionFromParent.y));
        _attackPoint = _weaponPrefabTransform.position + (Vector3)rangeOffset + directionFromParent * _radiusOffset;

    }
    protected override bool DetectEnemies(float atkRange)
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
    protected override void DoAttackLogic()
    {
        AttackLogic(_modifiedStats._atkDmg, _modifiedStats._knockbackForce);
    }
    protected override void AttackLogic(int damage, float knockbackForce)
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

    protected override void DelayLogic()
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

    protected override void StartDelay()
    {
        _delayingAnimation = true;
        _delayTimer.Start(); 
    }
    protected override void StopAnimationDelay()
    {
        _delayingAnimation = false;
        _weaponAnimator.speed = _attackSpeed;
    }

    protected override void ApplyDamage(Transform enemy, IDamageable entity, int damage)
    {
        int critRoll = Random.Range(0, 101);
        bool critHit = (_criticalChance > critRoll);
        int realDamage = (critHit) ? (int)(damage * _criticalDamageMultiplier) : damage;
        entity.TakeDamage(realDamage);
        PopupsManager.CreateDamagePopup(enemy.position + Vector3.up * 0.8f, realDamage, critHit);
        InvokeOnEnemyHit(enemy.position);
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
    protected class WeaponStats
    {
        public float _cooldown, _pullForce, _atkRange, _atkSpeed, _knockbackForce, _damageDelay;
        public int _atkDmg;

        public WeaponStats(float cooldown, float pullForce, float atkRange, float atkSpeed, float knockbackForce, float delay, int atkDmg)
        {
            _cooldown = cooldown;
            _pullForce = pullForce;
            _atkRange = atkRange;
            _atkSpeed = atkSpeed;
            _knockbackForce = knockbackForce;
            _damageDelay = delay;
            _atkDmg = atkDmg;
        }
    }
}

