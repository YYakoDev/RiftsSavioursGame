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
    protected Animator _prefabAnimator;
    [SerializeField]private bool _flipSprite = true;
    [SerializeField]private Vector3 _spawnPosition = new Vector3(-0.55f, 0.25f, 0f);
    [SerializeField]private float _spawnRotation = 0;

    protected int _currentAnim;
    [SerializeField]private AudioClip[] _weaponSounds;
    [SerializeField]private bool _pointCameraOnAttack = false;
    protected Transform _weaponPrefabTransform;

    [Header("Weapon Attack Stats")]
    [SerializeField]protected float _attackCooldown = 0.5f;
    [SerializeField]protected float _attackDuration = 0.35f;
    protected float _nextAttackTime = 0f;


    public event Action onAttack;


    //properties
    public string WeaponName => _name;
    public SOWeaponSpriteAnimationData SpriteAndAnimationData => _SpriteAndAnimationsData;
    public bool FlipSprite => _flipSprite;
    public Vector3 SpawnPosition => _spawnPosition;
    public float SpawnRotation => _spawnRotation;
    public Transform PrefabTransform => _weaponPrefabTransform;
    public float AtkCooldown => _attackCooldown;
    public float AtkDuration => _attackDuration;
    public bool PointCameraOnAttack => _pointCameraOnAttack;

    //anims
    public int Animation => _currentAnim;

    //sound stuff
    public AudioClip Sound => _weaponSounds[Random.Range(0, _weaponSounds.Length)];

    public virtual void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        _SpriteAndAnimationsData.SetAnimations();
        _weaponManager = weaponManager;
        _weaponPrefabTransform = prefabTransform;
        _currentAnim = Animator.StringToHash("Attack");
        _prefabAnimator = prefabTransform.GetComponent<Animator>();
    }

    protected virtual void Attack()
    {
        _nextAttackTime = Time.time + _attackCooldown;
        onAttack?.Invoke();
    }

    public virtual void InputLogic()
    {
        if(_nextAttackTime >= Time.time) return;
        //if you dont put a cooldown here everything is going to be fucked
        if(Input.GetButton("Attack"))
        {
            Attack();
        }
    }

    protected abstract void EvaluateStats(SOPlayerAttackStats attackStats);

    public virtual void DrawGizmos(){}
}
