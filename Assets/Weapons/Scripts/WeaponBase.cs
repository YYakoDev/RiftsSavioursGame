using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class WeaponBase: ScriptableObject
{
    //references
    protected WeaponManager _weaponManager;

    //fields
    [Header("Weapon Properties")]
    [SerializeField]private string _name;
    [SerializeField]protected SOWeaponSpriteAnimationData _SpriteAndAnimationsData;
    private const string AtkAnimName = "Attack";
    [SerializeField]private bool _flipSprite = true;
    [SerializeField]private Vector3 _spawnPosition = new Vector3(-0.55f, 0.25f, 0f);
    [SerializeField]private float _spawnRotation = 0;

    protected int _currentAnim;
    [SerializeField]protected AudioClip[] _weaponSounds;
    [SerializeField]private bool _pointCameraOnAttack = false;
    protected Transform _weaponPrefabTransform;

    [Header("Weapon Attack Stats")]
    [SerializeField]protected float _attackCooldown = 0.5f;
    [SerializeField, Range(0f, 2f)] protected float _pullForce = 0;
    protected float _attackDuration = 0.35f;
    protected float _nextAttackTime = 0f;


    public event Action onAttack;
    public event Action onEnemyHit;


    //properties
    public string WeaponName => _name;
    public SOWeaponSpriteAnimationData SpriteAndAnimationData => _SpriteAndAnimationsData;
    public bool FlipSprite => _flipSprite;
    public Vector3 SpawnPosition => _spawnPosition;
    public float SpawnRotation => _spawnRotation;
    public Transform PrefabTransform => _weaponPrefabTransform;
    public float AtkDuration => _attackDuration;
    public bool PointCameraOnAttack => _pointCameraOnAttack;

    //anims
    public int Animation => _currentAnim;

    //sound stuff
    protected AudioClip _attackSound;
    public AudioClip Sound => _attackSound;

    public virtual void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        _weaponManager = weaponManager;
        _weaponPrefabTransform = prefabTransform;
        _currentAnim = Animator.StringToHash(AtkAnimName);
        _attackDuration = GetAnimationDuration(AtkAnimName);

    }

    protected virtual void Attack(float weaponCooldown)
    {
        _nextAttackTime = Time.time + weaponCooldown;
        _attackSound = _weaponSounds[Random.Range(0, _weaponSounds.Length)];
        InvokeOnAttack();
    }

    public virtual void InputLogic()
    {
        if(_nextAttackTime >= Time.time) return;
        //if you dont put a cooldown here everything is going to be fucked
        if(Input.GetButton("Attack"))
        {
            Attack(_attackCooldown);
        }
    }

    protected float GetAnimationDuration(string animName)
    {
        return _SpriteAndAnimationsData.AnimatorOverride[animName].averageDuration;
    }
    public virtual float GetPullForce()
    {
        return _pullForce;
    }

    protected abstract void EvaluateStats(SOPlayerAttackStats attackStats);
    protected void InvokeOnEnemyHit()
    {
        onEnemyHit?.Invoke();
    }
    protected void InvokeOnAttack()
    {
        onAttack?.Invoke();
    }
    public virtual void DrawGizmos(){}
}
