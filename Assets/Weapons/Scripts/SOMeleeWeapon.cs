using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = MenuPath + "SOMeleeWeapon")]
public class SOMeleeWeapon : WeaponBase
{
    protected Transform _parentTransform;
    protected Animator _weaponAnimator;
    protected AudioSource _audioSource;
    LayerMask _enemyLayer;
    AudioSource _effectsAudio;
    Camera _mainCamera;
    //public event Action onHolding;
    InputAction.CallbackContext _callbackContextEmpty = new();

    AnimationCurve _cameraAnimCurve, _slowdownCurve;
    protected float _radiusOffset = 0;
    protected Vector3 _attackPoint = Vector2.zero;
    protected int _maxEnemiesToHit = 10;
    private readonly int AtkAnim = Animator.StringToHash("Attack");
    private readonly int AtkAnim2 = Animator.StringToHash("Attack2");
    private readonly int AtkAnim3 = Animator.StringToHash("Attack3");
    float[] _animDurations = new float[3];
    protected List<GameObject> _hittedEnemiesGO = new();
    protected Timer _atkExecutionTimer;


    [Header("Stats")]
    [SerializeField]MeleeWeaponStat _baseStats, _maxStats;
    MeleeWeaponStat _modifiedStats, _upgradeStats;
    [SerializeField] protected Vector2 _rangeOffset;
    [SerializeField]ComboAttack[] _comboAttacks = new ComboAttack[3];
    bool _holding, _attackIsHeavy = false;
    float _holdTime, _resetComboTime;
    Timer _comboWaitTimer;
    int _currentComboIndex = -1;
    const int ComboMaxCount = 3;
    WeaponEffects[] _heavyAtkEffectsInstance;
    const float MediumAtkThreshold = 0.3f, HeavyAtkThreshold = 0.51f; // with this you could have special effects play when this thresholds are met, also when you attack and hit

    [Header("Heavy Attack")]
    [SerializeField] MeleeWeaponStat _heavyAttackBonus;
    [SerializeField, Range(-50, 50)] int _cameraZoomPercent = 25;
    [SerializeField] WeaponEffects[] _heavyAtkEffects;
    [SerializeField] AudioClip _heavyAtkCueSFX, _heavyAtkChargeUpSfx, _heavyAtkSound;
    [SerializeField, Range(-0.2f, 0.5f)] float _pitchSubstraction = 0.2f;
    float _cameraStartSize, _startingAudioPitch = 1f;

    //
    QuickSwitchInfo _switchInfo;

    //properties
    public Animator Animator => _weaponAnimator;
    public AudioSource Audio => _audioSource;
    public float GetAtkSpeed() => _modifiedStats._atkSpeed;
    public float GetPullForce() => _modifiedStats._pullForce;
    public float GetPullDuration() => _modifiedStats._pullDuration;
    public override float GetWeaponCooldown() => _modifiedStats._cooldown;

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _weaponAnimator = prefabTransform.GetComponent<Animator>();
        _audioSource = prefabTransform.GetComponent<AudioSource>();
        _upgradeStats = new();
        _modifiedStats = new(_baseStats);
        _parentTransform = prefabTransform.parent;
        _enemyLayer = weaponManager.EnemyLayer;
        _switchInfo = new(0, false, false);
        _startingAudioPitch = _audioSource.pitch;
        
        _currentComboIndex = -1;
        _resetComboTime = 0f;
        _nextAttackTime = 0f;
        _holdTime = 0f;
        _animDurations[0] = GetAnimationDuration("Attack");
        _animDurations[1] = GetAnimationDuration("Attack2");
        _animDurations[2] = GetAnimationDuration("Attack3");
        _weaponAnimator.speed = _baseStats._atkSpeed;
        SetMaxEnemiesToHit(_baseStats._atkRange);
        SetRadiusOffset(_baseStats._atkRange);

        _atkExecutionTimer = new(_baseStats._damageDelay, false);
        _atkExecutionTimer.onEnd += DoAttackLogic;
        _atkExecutionTimer.Stop();

        //_releaseHoldAtkTimer = new(HeavyAtkThreshold + 0.05f - 0.15f);
        //_releaseHoldAtkTimer.Stop();
        //_releaseHoldAtkTimer.onEnd += TryAttack;

        _comboWaitTimer = new(_attackDuration - Time.deltaTime * 3f);
        _comboWaitTimer.Stop();
        _comboWaitTimer.onEnd += FreezeComboAnimation;

        _effectsAudio = _weaponManager.AtkEffects.Audio;
        _mainCamera = HelperMethods.MainCamera;
        _cameraStartSize = _mainCamera.orthographicSize;
        _cameraAnimCurve = TweenCurveLibrary.EaseInExpo;
        _slowdownCurve = TweenCurveLibrary.EaseInCirc;

        Debug.Log("init");

        //_delayTimer = new(_animationDelayTime);
        //_delayTimer.Stop();
        //_delayTimer.onEnd += StopAnimationDelay;

    }

    protected override void InitializeFXS()
    {
        base.InitializeFXS();
        _heavyAtkEffectsInstance = new WeaponEffects[_heavyAtkEffects.Length];
        for (int i = 0; i < _heavyAtkEffectsInstance.Length; i++)
        {
            _heavyAtkEffectsInstance[i] = Instantiate(_heavyAtkEffects[i]);
            _heavyAtkEffectsInstance[i].Initialize(this);
        }
        for (int i = 0; i < _comboAttacks.Length; i++)
        {
            _comboAttacks[i].InitializeEffects(this);
        }
    }

    public override void SetWeaponActive(bool active)
    {
        base.SetWeaponActive(active);
        if(active)
        {
            _hittedEnemiesGO = new();
            _atkExecutionTimer.Stop();
            //_weaponAnimator = _weaponManager.PrefabInstance.GetComponent<Animator>();
            _weaponAnimator.speed = _modifiedStats._atkSpeed;
        }
    }

    protected override void SubscribeInput()
    {
        _attackKey.action.performed += Hold;
        _attackKey.action.canceled += StopHolding;
    }
    public override void UnsubscribeInput()
    {
        _attackKey.action.performed -= Hold;
        _attackKey.action.canceled -= StopHolding;
        _initialized = false;
    }

    public override void UpdateLogic()
    {
        if(_deactivated) return;
        _comboWaitTimer.UpdateTime();
        _atkExecutionTimer.UpdateTime();

        if(_resetComboTime > 0f && !_holding)
        {
            _resetComboTime -= Time.deltaTime;
            if(_resetComboTime <= 0f)
            {
                ResetCombo();
            }
        }

        if(_holding)
        {
            var percent = _holdTime / HeavyAtkThreshold;
            if(percent > 1f) return;
            _holdTime += Time.deltaTime;
            var newSize = _cameraStartSize - _cameraStartSize * (float)(_cameraZoomPercent / 100f);
            _mainCamera.orthographicSize = Mathf.Lerp(_cameraStartSize, newSize, _cameraAnimCurve.Evaluate(percent)); //find a way to restore the camera size!
            var slowdownForce = Mathf.Lerp(1f, 0.4f, _slowdownCurve.Evaluate(percent));
            _weaponManager.AtkEffects.SlowdownPlayer(Time.deltaTime * 5f, slowdownForce);
            if(_holdTime >= HeavyAtkThreshold)
            {
                //make weapon blink and player blink. Also show a little popup that says "atk charged"
                _weaponManager.AtkEffects.SlowdownPlayer(60f, slowdownForce);
                _effectsAudio.Stop();
                _effectsAudio.PlayOneShot(_heavyAtkCueSFX);
                //_holding = false;
                _weaponManager.AtkEffects.BlinkWeapon();
                _weaponManager.AtkEffects.BlinkPlayer();
                NotificationSystem.SendNotification(NotificationType.Top, "Heavy attack charged!", null, 0.75f, 0.6f, 0.189f);
                //TryAttack();
            }
        }
    }
    void Hold(InputAction.CallbackContext obj)
    {
        if(_deactivated) return;
        if(_holding) return;
        if(_nextAttackTime + 0.05f - (HeavyAtkThreshold) >= Time.time) return;
        _effectsAudio.PlayWithVaryingPitch(_heavyAtkChargeUpSfx);
        _holding = true;
    }

    void StopHolding(InputAction.CallbackContext obj)
    {
        if(!_holding) return;
        TryAttack();
        _holding = false;
        //_releaseHoldAtkTimer.End();
    }

    void FreezeComboAnimation() => _weaponAnimator.speed = 0f;
    

    void ResetCombo()
    {
        _weaponAnimator.speed = _modifiedStats._atkSpeed;
        //_weaponAnimator.SetTrigger("Exit");//
        _nextAttackTime = Time.time + _modifiedStats._cooldown * ((float)(_currentComboIndex + 1) / 3f);
        _currentComboIndex = -1;
    }

    public override QuickSwitchInfo GetSwitchInfo()
    {
        _resetComboTime += Time.deltaTime;
        if(_nextAttackTime - _modifiedStats._cooldown / 2f >= Time.time) return null;
        _switchInfo.SetComboIndex(_currentComboIndex);
        _switchInfo.SetHeavyAttack(_holdTime >= HeavyAtkThreshold);
        _switchInfo.SetHoldingButtonState(_holding);
        return _switchInfo;
    }

    public override void QuickSwitch(QuickSwitchInfo info)
    {
        _resetComboTime += 0.1f;
        base.QuickSwitch(info);
        var index = info.ComboIndex + 1;
        if(index >= ComboMaxCount) index = 0;
        if(info.IsHeavyAttack) _holdTime = HeavyAtkThreshold;
        TryAttack();
        if(info.WasHoldingHeavyAtkButton)
        {
            if(_attackKey.action.IsInProgress())
            {
                Debug.Log("You were holding the attack button");
                _effectsAudio.PlayWithVaryingPitch(_heavyAtkChargeUpSfx);
                _holding = true;
            }
        }
    }

    void TryAttack()
    {
        TryAttack(_callbackContextEmpty);
    }
    protected override void TryAttack(InputAction.CallbackContext obj)
    {
        if(_deactivated) return;
        if(_holdTime < HeavyAtkThreshold) _effectsAudio.Stop();
        if(_nextAttackTime >= Time.time) return;
        _currentComboIndex++;
        if(_currentComboIndex >= ComboMaxCount) _currentComboIndex = 0;
        
        _weaponManager.AtkEffects.SlowdownPlayer(0f, 0f);
        _holding = false;
        CameraEffects.ResetScale();
        _attackSound = _comboAttacks[_currentComboIndex].AtkSound;
        _currentAnim = _currentComboIndex switch
        {
            0 => AtkAnim,
            1 => AtkAnim2,
            2 => AtkAnim3,
            _ => AtkAnim
        };
        //Debug.Log("First Attack Duration:   " + _animDurations[0]);
        var currentComboStats = _comboAttacks[_currentComboIndex].Stats;
        //_comboWaitTime = _animDurations[_currentComboIndex];


        //see if the holdtime has reached any thresholds and execute the attacks based on that
        _modifiedStats.GetStats(_baseStats);
        _modifiedStats.Add(currentComboStats);
        _modifiedStats.Add(_upgradeStats);
        _attackIsHeavy = false;
        if(_holdTime >= HeavyAtkThreshold) //heavy atk
        {
            _attackIsHeavy = true;
            _modifiedStats.Add(_heavyAttackBonus);
            _audioSource.pitch = _startingAudioPitch - _pitchSubstraction;
        }else
        {
            _audioSource.pitch = _startingAudioPitch;
        }
        var currentAnimDuration = _animDurations[_currentComboIndex] / _modifiedStats._atkSpeed;

        _resetComboTime = currentAnimDuration + 0.3f; //this 0.25f could be replaced with a variable called _comboWaitTime
        var cooldown = currentAnimDuration - (Time.deltaTime * 2f);
        //Debug.Log("current anim duration: " + currentAnimDuration);
        if(_currentComboIndex == ComboMaxCount-1)
        {
            cooldown = _resetComboTime + _modifiedStats._cooldown;
            _comboWaitTimer.Stop();
        }
        _weaponAnimator.speed = _modifiedStats._atkSpeed;
        _holdTime = 0f;
        _attackDuration = currentAnimDuration;
        if(_currentComboIndex < ComboMaxCount-1)
        {
            _comboWaitTimer.ChangeTime(currentAnimDuration - (currentAnimDuration * 0.05f));
            _comboWaitTimer.Start();
        }
        _atkExecutionTimer.ChangeTime(_modifiedStats._damageDelay);
        Attack(cooldown);

    }

    protected override void Attack(float cooldown)
    {
        PlayAtkFXS(_comboAttacks[_currentComboIndex].Effects);
        if(_attackIsHeavy) PlayAtkFXS(_heavyAtkEffectsInstance);
        _nextAttackTime = Time.time + cooldown;
        InvokeOnAttack();
        SetAttackPoint();
        SetRadiusOffset(_modifiedStats._atkRange);
        SetMaxEnemiesToHit(_modifiedStats._atkRange);
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
    protected void DoAttackLogic() => AttackLogic(_modifiedStats._atkDmg, _modifiedStats._knockbackForce);
    
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
        bool critHit = (_modifiedStats._criticalChance > critRoll);
        int realDamage = (critHit) ? (int)(damage * _modifiedStats._criticalDamageMultiplier) : damage;
        entity.TakeDamage(realDamage);
        InvokeOnEnemyHit(enemy);
        DamagePopupTypes popupType = (critHit) ? DamagePopupTypes.CriticalRed : (_attackIsHeavy) ? DamagePopupTypes.CriticalYellow : DamagePopupTypes.Normal;
        PopupsManager.CreateDamagePopup(enemy.position + Vector3.up * 0.85f, realDamage, popupType);
        PlayHitFXS(_comboAttacks[_currentComboIndex].Effects, enemy);
        if(_attackIsHeavy) PlayHitFXS(_heavyAtkEffectsInstance, enemy);
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
        _upgradeStats._atkDmg = Mathf.RoundToInt((_baseStats._atkDmg + attackStats.BaseDamageAddition - 1) * attackStats.DamageMultiplier) - _baseStats._atkDmg;
        _upgradeStats._atkRange = Mathf.RoundToInt(attackStats.AttackRange - 1f);
        _upgradeStats._cooldown = -(attackStats.AttackCooldown - 1f);
        _upgradeStats._knockbackForce = ( attackStats.AttackKnockback - 1f);
        _upgradeStats._atkSpeed = (attackStats.AttackSpeed - 1f);
        _upgradeStats._criticalChance = Mathf.RoundToInt(attackStats.CriticalChance - 1f);
        _upgradeStats._criticalDamageMultiplier = (attackStats.CriticalDamageMultiplier - 1f);
        ClampStats();
    }

    protected virtual void ClampStats()
    {
        if(_maxStats == null) return;
        if(_maxStats._atkDmg > 0) _upgradeStats._atkDmg = Mathf.Clamp(_upgradeStats._atkDmg, 0, _maxStats._atkDmg);
        if(_maxStats._atkRange > 0) _upgradeStats._atkRange = Mathf.Clamp(_upgradeStats._atkRange, 0, _maxStats._atkRange);
        if(_maxStats._atkSpeed > 0) _upgradeStats._atkSpeed = Mathf.Clamp(_upgradeStats._atkSpeed, 0, _maxStats._atkSpeed);
        if(_maxStats._knockbackForce > 0) _upgradeStats._knockbackForce = Mathf.Clamp(_upgradeStats._knockbackForce, 0, _maxStats._knockbackForce);
        if(_maxStats._cooldown > 0) _upgradeStats._cooldown = Mathf.Clamp(_upgradeStats._cooldown, _maxStats._cooldown, 60f);
        if(_maxStats._criticalChance > 0) _upgradeStats._criticalChance = Mathf.Clamp(_upgradeStats._criticalChance, 0, _maxStats._criticalChance);
        if(_maxStats._criticalDamageMultiplier > 0) _upgradeStats._criticalDamageMultiplier = Mathf.Clamp(_upgradeStats._criticalDamageMultiplier, 0, _maxStats._criticalDamageMultiplier);
    }

    [Serializable]
    protected class ComboAttack
    {
        [SerializeField] MeleeWeaponStat _stats;
        [SerializeField] WeaponEffects[] _effects;
        WeaponEffects[] _initializedFxs;
        [SerializeField] AudioClip[] _sounds = new AudioClip[1];
        public MeleeWeaponStat Stats => _stats;
        public WeaponEffects[] Effects => _initializedFxs;
        public AudioClip AtkSound => _sounds[Random.Range(0, _sounds.Length)];

        public void InitializeEffects(WeaponBase weapon)
        {
            Array.Resize<WeaponEffects>(ref _initializedFxs, _effects.Length);
            for (int i = 0; i < _effects.Length; i++)
            {
                _initializedFxs[i] = Instantiate(_effects[i]);
                _initializedFxs[i].Initialize(weapon);
            }
        }
    }

    private void OnValidate() {
        if(_comboAttacks.Length > 3)
        {
            Array.Resize<ComboAttack>(ref _comboAttacks, 3);
        }
    }
}

