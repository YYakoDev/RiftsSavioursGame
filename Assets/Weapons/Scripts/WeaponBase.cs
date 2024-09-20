using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public abstract class WeaponBase: ScriptableObject, IQuickSwitchHandler
{
    [NonSerialized]protected bool _initialized = false;
    public const string MenuPath = "ScriptableObjects/Weapons/";
    //references
    protected WeaponManager _weaponManager;
    [SerializeField]protected InputActionReference _attackKey => _weaponManager.AttackInputRef;

    protected bool _deactivated = false;

    //fields
    [Header("Weapon Properties")]
    [SerializeField]private string _name;
    [SerializeField, TextArea]private string _description;
    [SerializeField]protected SOWeaponSpriteAnimationData _SpriteAndAnimationsData;
    [SerializeField]private WeaponEffects[] _effects;
    protected WeaponEffects[] _usedEffects = new WeaponEffects[0];
    private const string AtkAnimName = "Attack";
    protected int _currentAnim;
    [SerializeField]protected AudioClip[] _weaponSounds;
    protected bool _randomizeSounds = true;
    [SerializeField]private bool _pointCameraOnAttack = false;
    protected Transform _weaponPrefabTransform;


    [Header("Weapon Attack Stats")]
    [SerializeField]protected float _attackCooldown = 0.5f;
    protected float _attackDuration = 0.35f;
    protected float _nextAttackTime = 0f;


    public event Action onAttack;
    public event Action<Vector3> onEnemyHit;


    //properties
    public bool Initialized => _initialized;

    public string WeaponName => _name;
    public string Description => _description;
    public SOWeaponSpriteAnimationData SpriteAndAnimationData => _SpriteAndAnimationsData;
    public WeaponEffects[] WeaponEffects => _effects;
    public bool FlipSprite => _SpriteAndAnimationsData.FlipSprite;
    public Vector3 SpawnPosition => _SpriteAndAnimationsData.SpawnPosition;
    public float SpawnRotation => _SpriteAndAnimationsData.SpawnRotation;
    public Transform PrefabTransform => _weaponPrefabTransform;
    public float AtkDuration => _attackDuration;
    public bool PointCameraOnAttack => _pointCameraOnAttack;

    //anims
    public int Animation => _currentAnim;

    //sound stuff
    protected AudioClip _attackSound;
    public AudioClip Sound => _attackSound;
    public PlayerAttackEffects FxsScript => _weaponManager.AtkEffects;

    public virtual void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        _weaponManager = weaponManager;
        _weaponPrefabTransform = prefabTransform;
        _currentAnim = Animator.StringToHash(AtkAnimName);
        _attackDuration = GetAnimationDuration(AtkAnimName);
        onAttack = null;
        onEnemyHit = null;
        SubscribeInput();
        InitializeFXS();
        _initialized = true;
    }
    protected virtual void SubscribeInput()
    {
        _attackKey.action.started += TryAttack;
    }
    public virtual void UnsubscribeInput()
    {
        _attackKey.action.started -= TryAttack;
        _initialized = false;
    }
    protected virtual void InitializeFXS()
    {
        if(_effects == null) return;
        Array.Resize<WeaponEffects>(ref _usedEffects, _effects.Length);
        for (int i = 0; i < _usedEffects.Length; i++)
        {
            var baseFx = _effects[i];
            if(baseFx == null) continue;
            _usedEffects[i] = GameObject.Instantiate(baseFx);
            _usedEffects[i]?.Initialize(this);
        }
    }

    protected virtual void Attack(float weaponCooldown)
    {
        _nextAttackTime = Time.time + weaponCooldown;
        if(_randomizeSounds) _attackSound = _weaponSounds[Random.Range(0, _weaponSounds.Length)];
        InvokeOnAttack();
    }
    public virtual void UpdateLogic()
    {
        if(_deactivated) return;
    }
    
    protected virtual void TryAttack(InputAction.CallbackContext obj)
    {
        if(_deactivated) return;
        if(_nextAttackTime >= Time.time) return;
        Attack(_attackCooldown);
    }

    protected float GetAnimationDuration(string animName)
    {
        return _SpriteAndAnimationsData.AnimatorOverride[animName].averageDuration;
    }

    public virtual void SetWeaponActive(bool active)
    {
        _deactivated = !active;
    }

    public virtual float GetWeaponCooldown() => _attackCooldown;

    public abstract void EvaluateStats(SOPlayerAttackStats attackStats);
    protected void InvokeOnAttack()
    {
        onAttack?.Invoke();
        PlayAtkFXS(_usedEffects);
    }
    protected void InvokeOnEnemyHit(Vector3 enemyPos)
    {
        onEnemyHit?.Invoke(enemyPos);
        PlayHitFXS(_usedEffects, enemyPos);
    }

    protected virtual void PlayAtkFXS(WeaponEffects[] effects)
    {
        foreach(WeaponEffects fx in effects)
        {
            if(fx == null) continue;
            fx.OnAttackFX();
        }
    }
    protected virtual void PlayHitFXS(WeaponEffects[] effects, Vector3 pos)
    {
        foreach(WeaponEffects fx in effects)
        {
            if(fx == null) continue;
            fx.OnHitFX(pos);
        }
    }
    public virtual void RemoveFxFromList(WeaponEffects fx)
    {
        int index = -1;
        for (int i = 0; i < _usedEffects.Length; i++)
        {
            if(_usedEffects[i] == fx)
            {
                index = i;
                break;
            }
        }
        if(index != -1) _usedEffects[index] = null;
    }

    public virtual void DrawGizmos(){}

    public virtual QuickSwitchInfo GetSwitchInfo() => null;

    public virtual void QuickSwitch(QuickSwitchInfo info)
    {}

    ~WeaponBase()
    {
        UnsubscribeInput();
    }
}
