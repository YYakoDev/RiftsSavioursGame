using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase: ScriptableObject
{
    //references
    protected WeaponManager _weaponManager;

    //fields
    [Header("Weapon Properties")]
    [SerializeField]private string _name;
    [SerializeField]private Sprite _sprite;
    [SerializeField]private bool _flipSprite = true;
    [SerializeField]private Vector3 _spawnPosition = new Vector3(-0.55f, 0.25f, 0f);
    [SerializeField]private float _spawnRotation = 0;
    [SerializeField]private AnimatorOverrideController _animatorOverrideController;
    [SerializeField]bool _pointCameraOnAttack = false;
    protected Transform _weaponPrefabTransform;

    [Header("Weapon Attack Stats")]
    [SerializeField]protected float _attackCooldown = 0.5f;
    protected float _nextAttackTime = 0f;


    public event Action onAttack;


    //properties
    public string WeaponName => _name;
    public Sprite WeaponSprite => _sprite;
    public bool FlipSprite => _flipSprite;
    public AnimatorOverrideController AnimatorOverrideController => _animatorOverrideController;
    public Vector3 SpawnPosition => _spawnPosition;
    public float SpawnRotation => _spawnRotation;
    public Transform PrefabTransform => _weaponPrefabTransform;
    public float AttackCooldown => _attackCooldown;
    public bool PointCameraOnAttack => _pointCameraOnAttack;

    public virtual void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        _weaponManager = weaponManager;
        _weaponPrefabTransform = prefabTransform;
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
