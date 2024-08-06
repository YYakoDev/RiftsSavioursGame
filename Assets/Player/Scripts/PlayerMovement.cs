using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerManager))]
public class PlayerMovement : MonoBehaviour, IKnockback
{
    //References
    [Header("References")]
    [SerializeField]ParticleSystem _dustEffect, _dashParticleEffect;
    PlayerManager _player;
    [SerializeField] PlayerHealthManager _healthManager;
    GameObject _spriteGameObject;

    //Movement
    Vector2 _movement;
    float _realSpeed, _elapsedAcceleration = 0f;
    [SerializeField] AnimationCurve _curve;
    // Dash
    KeyInput _dashInput;
    bool _isDashing, _dashOnCooldown, _dashLogicInitialized;
    Vector2 _dashDirection;
    [SerializeField] float _dashDuration = 0.1f, _dashForce = 1f;
    float _dashCooldown;
    Timer _dashDurationTimer, _dashCooldownTimer;
    [SerializeField] DashFXPrefab _dashFXPrefab;
    [SerializeField] LayerMask _enemyLayer, _resourceLayer;
    LayerMask _playerLayer;
    DashFXPrefab _dashFXInstance;
    Transform _dashFXTransform;
    public event Action onDash;


    //Slowdown when attacking
    float _slowdown = 1f;
    float _slowdownTime = 1f;

    //Flipping sprite based on direction
    FlipLogic _flipLogic;

    //sort the order of the sprite based on movement
    SortingOrderController _sortingOrderController;
    [SerializeField]private float _sortingOrderOffset; //this adds an offset to the position detected on the sprite    
    //aiming

    [Header("AudioStuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] float _stepSoundCooldown = 0.3f;
    float _nextStepSound = 0f;
    [SerializeField] AudioClip[] _stepSounds;
    private AudioClip _stepSound => _stepSounds[Random.Range(0, _stepSounds.Length)];


    //properties
    public Vector2 Movement => _movement;
    //public bool IsFlipped => isFlipped;
    public float DashDuration => _dashDuration;
    public float DashForce => _dashForce;
    public float DashCooldown => _dashCooldown;
    public FlipLogic FlipLogic => _flipLogic;
    public int FacingDirection => (_flipLogic.IsFlipped) ? -1 : 1;
    private float MovementSpeed => _player.Stats.Speed;
    private float SlowdownMultiplier => _player.Stats.SlowdownMultiplier;

    //Knockback Stuff
    Knockbackeable _knockbackLogic;
    bool _knockbackEnabled;
    public Knockbackeable KnockbackLogic { get => _knockbackLogic;}
    public bool KnockbackEnabled => _knockbackEnabled;


    void Awake()
    {
        //components
        gameObject.CheckComponent<PlayerManager>(ref _player);
        
        //script that handles the knockback effect
        //there is a problem with this part, you cant really update the speed if the player gets an upgrade
        //you should handle that through the onStatsChange Event on the playerstats script
        if(_knockbackLogic == null)_knockbackLogic = new Knockbackeable(transform, _player.RigidBody, OnKnockbackChange, Mathf.RoundToInt(_player.Stats.KnockbackResistance));
        
        //script that handles the sorting order based on its position
        if(_sortingOrderController == null)_sortingOrderController = new SortingOrderController(transform, _player.Renderer, _sortingOrderOffset);

    }
    IEnumerator Start()
    {
        YYInputManager.OnMovement += SetMovement;
        _spriteGameObject = _player.Renderer.gameObject;
        _flipLogic = new(_spriteGameObject.transform, true, false, 0.15f);
        _realSpeed = MovementSpeed;
        _slowdown = 1f;
        if(_curve == null) _curve = TweenCurveLibrary.GetCurve(CurveTypes.EaseInOut);
        yield return null;
        if(_player.DashData != null) InitializeDashLogic();
        _dashParticleEffect.Stop();
        _playerLayer = gameObject.layer;
    }

    void InitializeDashLogic()
    {
        _dashDurationTimer = new(_dashDuration);
        _dashDurationTimer.Stop();
        _dashDurationTimer.onEnd += StopDash;
        UpdateDashCooldown();
        
        _dashInput = YYInputManager.GetKey(KeyInputTypes.Dash);
        _dashInput.OnKeyPressed += SetDash;
        _player.Stats.onStatsChange += UpdateDashCooldown;
        if(_dashFXInstance == null) _dashFXInstance = Instantiate(_dashFXPrefab);
        _dashFXInstance.SetDashData(_player, _healthManager, _audio);
        _dashFXTransform = _dashFXInstance.transform;
        _dashFXTransform.SetParent(transform, false);
        _dashLogicInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_slowdownTime > 0)
        {
            _slowdownTime -= Time.deltaTime;
        }else
        {
            _slowdown = 1f;
        }
        if(_dashLogicInitialized)
        {
            _dashDurationTimer.UpdateTime();
            _dashCooldownTimer.UpdateTime();
        }
        _flipLogic?.UpdateLogic();
    }
    private void FixedUpdate()
    {
        if(_isDashing)
        {
            DashMovement(_dashDirection, _player.Stats.DashSpeed);
            return;
        }
        if(KnockbackEnabled) _knockbackLogic.ApplyKnockback();
        else if(_movement.sqrMagnitude > 0.1f) Move();
        else Iddle();
    }

    public void SetMovement(Vector2 movementInput)
    {
        _movement = movementInput;
        _movement.Normalize();
    }

    public void SetDash()
    {
        if(_dashOnCooldown) return;
        if(_movement == Vector2.zero) return;
        _dashOnCooldown = true;
        _dashCooldownTimer.Start();
        _isDashing = true;
        _dashDirection = _movement.normalized * _dashForce;
        _dashDurationTimer.Start();
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_enemyLayer), true);
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_resourceLayer), true);
        _dashParticleEffect.Play();
        onDash?.Invoke();
        var rot = _dashFXTransform.rotation.eulerAngles;
        rot.z = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
        _dashFXTransform.rotation = Quaternion.Euler(rot);
    }
    public void StopDash()
    {
        _isDashing = false;
        _dashParticleEffect.Stop();
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_enemyLayer), false);
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_resourceLayer), false);
    }

    void UpdateDashCooldown()
    {
        if(_dashCooldown != _player.Stats.DashCooldown)
        {
            _dashCooldown = _player.Stats.DashCooldown;
            if(_dashCooldownTimer == null)
            {
                _dashCooldownTimer = new(_dashCooldown);
                _dashCooldownTimer.Stop();
                _dashCooldownTimer.onEnd += ReadyDash;
            }
            else
            {
                _dashCooldownTimer.ChangeTime(_dashCooldown);
            }
        }
    }

    void ReadyDash() => _dashOnCooldown = false;

    void Iddle()
    {
        _player.AnimController.PlayStated(PlayerAnimationsNames.Iddle);
        _sortingOrderController.SortOrder();
        _dustEffect.Stop();
        if(_elapsedAcceleration > 0) _elapsedAcceleration -= Time.deltaTime * 2f;
    }

    void Move()
    {
        if(_elapsedAcceleration < 2f) _elapsedAcceleration += Time.deltaTime;
        var speed = Mathf.Lerp(MovementSpeed * 0.85f * _slowdown , MovementSpeed * _slowdown * 1.35f, _curve.Evaluate(_elapsedAcceleration));
        Vector2 direction = (Vector2)transform.position + _movement * (speed *Time.fixedDeltaTime);
        _player.RigidBody.MovePosition(direction);
        MovementEffects();
    }
    void DashMovement(Vector2 moveDirection, float speed)
    {
        Vector2 direction = (Vector2)transform.position + moveDirection * (speed *Time.fixedDeltaTime);
        _player.RigidBody.MovePosition(direction);
        MovementEffects();
    }

    void MovementEffects()
    {
        _dustEffect.Play();
        _audio.PlayWithCooldown(_stepSound, _stepSoundCooldown, ref _nextStepSound);
        //change sprite sorting order based on its position
        _sortingOrderController.SortOrder();
        _player.AnimController.PlayStated(PlayerAnimationsNames.Run);

        //if(_autoAiming && _aimingScript.EnemyResultsCount > 0) _flipLogic.FlipCheck(_aimingScript.TargetPoint.x, 0f);
        //else _flipLogic.FlipCheck(_movement.x);
    }

    public void OnKnockbackChange(bool change)
    {
        _knockbackEnabled = change;
    }

    

    public void SlowdownMovement(float slowdownTime)
    {
        _slowdownTime = slowdownTime;
        _slowdown = SlowdownMultiplier;
    }
    public void SlowdownMovement(float duration, float force)
    {
        _slowdownTime = duration;
        _slowdown = force - SlowdownMultiplier;
        if(_slowdown <= 0) _slowdown = 0.1f;
    }

    private void OnDestroy() {
        YYInputManager.OnMovement -= SetMovement;
        if(!_dashLogicInitialized) return;
        _dashDurationTimer.onEnd -= StopDash;
        _dashInput.OnKeyPressed -= SetDash;
        _player.Stats.onStatsChange -= UpdateDashCooldown;
    }
}
