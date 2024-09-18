using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyHealthManager), typeof(EnemyMovement), typeof(Rigidbody2D)),
RequireComponent(typeof(EnemyAnimations), typeof(AudioSource), typeof(EnemyCollisions))]
public class EnemyBrain : MonoBehaviour
{
    public static event Action OnEnemyDeath;
    bool _checkedComponents = false;

    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] GameObject _shadow;
    [SerializeField] EnemyMovement _movement;
    [SerializeField] EnemyDetector _enemyDetector;
    [SerializeField] EnemyCollisions _collisions;
    [SerializeField] EnemyAnimations _animation;
    [SerializeField] EnemyHealthManager _healthManager;
    [SerializeField] Dropper _dropper;
    [SerializeField] EnemyAttack _attack;
    [SerializeField] BoxCollider2D _boxColl;
    [SerializeField] CircleCollider2D _circleColl;
    [SerializeField]AudioSource _audio;
    Collider2D[] _colliders;
    Transform _target;
    Sprite _intialSprite = null;
    AIStats _aiStats;
    EnemyBaseMovement _movementLogic;
    EnemyAttackLogic _attackLogic;
    SOEnemyMovementBehaviour _movementBehaviour;
    SOEnemyAttackBehaviour _attackBehaviour;
    SOEnemyBehaviour _deathBehaviour;
    private BloodSplatterFX _bloodPrefab;
    private ShadowData _shadowFxData;
    AudioClip[] _moveSFXs, _onHitSFXs, _attackSFXs, _deathSFXs;

    //properties
    public Rigidbody2D Rigidbody => _rb;
    public SpriteRenderer Renderer => _renderer;
    public AudioSource Audio => _audio;
    public AIStats Stats => _aiStats;
    public EnemyHealthManager HealthManager => _healthManager;
    public EnemyAnimations Animation => _animation;
    public EnemyAttackLogic AttackLogic => _attackLogic;
    public EnemyBaseMovement MovementLogic => _movementLogic;
    public EnemyDetector EnemyDetector => _enemyDetector;
    public EnemyCollisions Collisions => _collisions;
    public BloodSplatterFX BloodFX => _bloodPrefab;
    public Transform TargetTransform 
    {   
        get => _target;
        set => _target = value;
    }

    private void Awake()
    {
        if(!_checkedComponents) CheckComponents();
        _healthManager.onDeath += DisableComponents;
        
    }
    
    void CheckComponents()
    {
        if(_checkedComponents) return;
        _checkedComponents = true;
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Rigidbody2D>(ref _rb);
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        thisGO.CheckComponent<AudioSource>(ref _audio);
        thisGO.CheckComponent<EnemyHealthManager>(ref _healthManager);
        thisGO.CheckComponent<Dropper>(ref _dropper);
        thisGO.CheckComponent<EnemyAnimations>(ref _animation);
        if(_enemyDetector == null) _enemyDetector = thisGO.GetComponentInChildren<EnemyDetector>();
        thisGO.CheckComponent<EnemyCollisions>(ref _collisions);
        thisGO.CheckComponent<EnemyMovement>(ref _movement);
        thisGO.CheckComponent<EnemyAttack>(ref _attack);
        thisGO.CheckComponent<BoxCollider2D>(ref _boxColl);
        thisGO.CheckComponent<CircleCollider2D>(ref _circleColl);
        _shadow = transform.Find("Shadow2").gameObject;
        _colliders = new Collider2D[2]
        {
            _boxColl,
            _circleColl
        };
        _aiStats = new();

        _animation.Animator.keepAnimatorControllerStateOnDisable = true;
    }

    public void Initialize(SOEnemy data, Transform target, DifficultyStats stats = null)
    {
        CheckComponents();
        _target = target;

        _intialSprite = data.Sprite;
        _animation.Animator.runtimeAnimatorController = null;
        _renderer.sprite = _intialSprite;
        _animation.Animator.runtimeAnimatorController = data.Animator;
        _aiStats.SetValues(data.Stats);
        _aiStats.AddDifficultyStats(stats);

        _dropper.Clear();
        foreach(Drop drop in data.Drops) _dropper.AddDrop(drop);


        _boxColl.size = data.BoxColliderSize;
        _boxColl.offset = data.BoxColliderOffset;
        _boxColl.isTrigger = data.IsBoxTrigger;
        _circleColl.radius = data.CircleColliderRadius;
        _circleColl.offset = data.CircleColliderOffset;
        _circleColl.isTrigger = data.IsCircleTrigger;
        _enemyDetector.enabled = data.EnableEnemyDetector;
        _enemyDetector.SetRadius(data.EnemyDetectionRadius);

        SetBehaviours(data.MovementBehaviour, data.AttackBehaviour, data.DeathBehaviour);

        _bloodPrefab = data.BloodFX;
        if(data.BlinkMaterial != null)_healthManager.BlinkFX.SetMaterial(data.BlinkMaterial);
        _shadowFxData = new(data.HasShadow, data.ShadowOffset, data.ShadowSize);
        SetShadow();
        _moveSFXs = data.MoveSFXs;
        _onHitSFXs = data.OnHitSFXs;
        _attackSFXs = data.AttackSFXs;
        _deathSFXs = data.DeathSFXs;

        InitializeLogics();
    }



    public struct ShadowData
    {
        public ShadowData(bool hasShadow, Vector3 pos, Vector3 size)
        {
            _hasShadow = hasShadow;
            _position = pos;
            _size = size;
        }
        bool _hasShadow;
        Vector3 _position, _size;
        public bool HasShadow => _hasShadow;
        public Vector3 Position => _position;
        public Vector3 Size => _size;
    }
    void SetShadow()
    {
        _shadow.SetActive(_shadowFxData.HasShadow);
        _shadow.transform.localPosition = _shadowFxData.Position;
        _shadow.transform.localScale = _shadowFxData.Size;
    }

    void SetBehaviours(SOEnemyMovementBehaviour movement, SOEnemyAttackBehaviour atk, SOEnemyBehaviour death)
    {
        _movementBehaviour = (movement == null) ? null : Instantiate(movement);
        _attackBehaviour = (atk == null) ? null : Instantiate(atk);
        _deathBehaviour = (death == null) ? null : Instantiate(death);

        _movementBehaviour?.Initialize(this);
        _attackBehaviour?.Initialize(this);
        _deathBehaviour?.Initialize(this);
    }
    void InitializeLogics()
    {
        if(_movementLogic == null) _movementLogic = new(transform, this, 0.25f);
        else _movementLogic.ReInitialize(transform, this);
        if(_attackLogic == null) _attackLogic = new(transform, _aiStats.Damage, _aiStats.KnockbackForce);
        else _attackLogic.ReInitialize(transform, _aiStats.Damage, _aiStats.KnockbackForce);
        _movement.Init(_movementLogic, _movementBehaviour);
        _attack.Init(this, _attackLogic, _attackBehaviour);
        _healthManager.Init(this, _deathBehaviour);
    }

    public AudioClip GetMoveSfx() => GetRandomSound(_moveSFXs);
    public AudioClip GetOnHitSfx() => GetRandomSound(_onHitSFXs);
    public AudioClip GetOnDeathSfx() => GetRandomSound(_deathSFXs);
    public AudioClip GetAttackSfx() => GetRandomSound(_attackSFXs);

    private AudioClip GetRandomSound(AudioClip[] sfxSelection)
    {
        if(sfxSelection == null || sfxSelection.Length <= 0) return null;
        else return sfxSelection[Random.Range(0, sfxSelection.Length)];
    }

    public void PlaySound(AudioClip clip)
    {
        if(clip == null) return;
        _audio.PlayWithVaryingPitch(clip);
    }

    #region Components State and enable & disable events
    void SetComponentsToActive(bool state)
    {
        foreach(Collider2D coll in _colliders)
        {
            coll.enabled = state;
        }
        _healthManager.enabled = state;
        if(_movementLogic != null) _movementLogic.Enabled = state;
    }
    void EnableComponents() => SetComponentsToActive(true);
    
    void DisableComponents() 
    {
        OnEnemyDeath?.Invoke();
        SetComponentsToActive(false);
    }
    
    private void OnEnable() {
        EnableComponents();
        _renderer.enabled = true;
    }
    private void OnDisable() {
        
        _renderer.sprite = _intialSprite;
        _renderer.enabled = false;
        _renderer.sprite = _intialSprite;
    }

    private void OnDestroy() {
        _healthManager.onDeath -= DisableComponents;
    }
    #endregion


   
    
}
