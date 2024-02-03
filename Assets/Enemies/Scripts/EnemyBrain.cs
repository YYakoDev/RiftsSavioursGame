using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealthManager), typeof(EnemyMovement), typeof(Rigidbody2D)),
RequireComponent(typeof(EnemyAnimations), typeof(AudioSource), typeof(EnemyCollisions))]
public class EnemyBrain : MonoBehaviour, ITargetPositionProvider
{
    bool _checkedComponents = false;

    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] GameObject _shadow;
    [SerializeField] EnemyMovement _movement;
    [SerializeField] EnemyDetector _enemyDetector;
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
    public BloodSplatterFX BloodFX => _bloodPrefab;
    public Transform TargetTransform { get => _target; set => _target = value; }

    private void Awake()
    {
        CheckComponents();
        _healthManager.onDeath += DisableComponents;
        
    }
    
    void CheckComponents()
    {
        if(_checkedComponents) return;
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Rigidbody2D>(ref _rb);
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        thisGO.CheckComponent<AudioSource>(ref _audio);
        thisGO.CheckComponent<EnemyHealthManager>(ref _healthManager);
        thisGO.CheckComponent<Dropper>(ref _dropper);
        thisGO.CheckComponent<EnemyAnimations>(ref _animation);
        if(_enemyDetector == null) _enemyDetector = thisGO.GetComponentInChildren<EnemyDetector>();
        thisGO.CheckComponent<EnemyMovement>(ref _movement);
        thisGO.CheckComponent<EnemyAttack>(ref _attack);
        thisGO.CheckComponent<BoxCollider2D>(ref _boxColl);
        thisGO.CheckComponent<CircleCollider2D>(ref _circleColl);
        _colliders = new Collider2D[2]
        {
            _boxColl,
            _circleColl
        };
        _checkedComponents = true;

    }

    public void Initialize(SOEnemy data)
    {
        CheckComponents();

        _intialSprite = data.Sprite;
        _animation.Animator.runtimeAnimatorController = null;
        _renderer.sprite = _intialSprite;
        _animation.Animator.runtimeAnimatorController = data.Animator;
        
        _aiStats = data.Stats;

        _dropper.Clear();
        foreach(Drop drop in data.Drops) _dropper.AddDrop(drop);


        _boxColl.size = data.BoxColliderSize;
        _boxColl.isTrigger = data.IsBoxTrigger;
        _circleColl.radius = data.CircleColliderRadius;
        _circleColl.offset = data.CircleColliderOffset;
        _circleColl.isTrigger = data.IsCircleTrigger;
        _enemyDetector.enabled = data.EnableEnemyDetector;
        _enemyDetector.SetRadius(data.EnemyDetectionRadius);


        SetBehaviours(data.MovementBehaviour, data.AttackBehaviour, data.DeathBehaviour);

        _bloodPrefab = data.BloodFX;
        if(_shadow != null)
        {
            _shadow.SetActive(data.HasShadow);
            _shadow.transform.localPosition = data.ShadowOffset;
            _shadow.transform.localScale = data.ShadowSize;
        }

        _moveSFXs = data.MoveSFXs;
        _onHitSFXs = data.OnHitSFXs;
        _attackSFXs = data.AttackSFXs;
        _deathSFXs = data.DeathSFXs;

        InitializeLogics();
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
        _healthManager.Init(_deathBehaviour);
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
    
    void DisableComponents() => SetComponentsToActive(false);
    
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
