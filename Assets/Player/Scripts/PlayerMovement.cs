using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerManager))]
public class PlayerMovement : MonoBehaviour, IKnockback
{
    //References
    [Header("References")]
    [SerializeField] PlayerHealthManager _healthManager;
    PlayerManager _player;
    [SerializeField] WeaponAiming _aimingLogic;
    [SerializeField] ParticleSystem _dustEffect, _dashParticleEffect;
    GameObject _spriteGameObject;

    [Header("Input")]
    [SerializeField] InputActionReference _movementInput, _dashInput;

    //Movement
    Vector2 _movement, _lastMovement;
    float _realSpeed, _elapsedAcceleration = 0f;
    [SerializeField] AnimationCurve _curve;


    // Dash
    bool _isDashing, _dashOnCooldown, _dashLogicInitialized;
    Vector2 _dashDirection, _startingDashPosition;
    float _dashCooldown, _elapsedDashTime = 0f, _dashDuration = 0.25f;
    AnimationCurve _dashCurve;
    Timer _dashDurationTimer, _dashCooldownTimer;
    [SerializeField] DashFXPrefab _dashFXPrefab;
    [SerializeField] LayerMask _enemyLayer, _resourceLayer;
    LayerMask _playerLayer;
    DashFXPrefab _dashFXInstance, _backDashFXInstance;
    public event Action onDash;


    //Slowdown when attacking
    float _slowdown = 1f, _slowdownTime = 1f;

    //Flipping sprite based on direction
    FlipLogic _flipLogic;

    //sort the order of the sprite based on movement
    SortingOrderController _sortingOrderController;
    [SerializeField] private float _sortingOrderOffset; //this adds an offset to the position detected on the sprite    
    //aiming

    [Header("AudioStuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] float _stepSoundCooldown = 0.3f;
    float _nextStepSound = 0f;
    [SerializeField] AudioClip[] _stepSounds;
    private AudioClip _stepSound => _stepSounds[Random.Range(0, _stepSounds.Length)];


    bool _autoAim;

    //properties
    public Vector2 Movement => _movement;
    public Vector2 LastMovement => _lastMovement;
    //public bool IsFlipped => isFlipped;
    public float DashCooldown => _dashCooldown;
    public FlipLogic FlipLogic => _flipLogic;
    public int FacingDirection => (_flipLogic.IsFlipped) ? -1 : 1;
    private float MovementSpeed => _player.Stats.Speed;
    private float SlowdownMultiplier => _player.Stats.SlowdownMultiplier;
    public float DashDuration => _dashDuration;

    //Knockback Stuff
    Knockbackeable _knockbackLogic;
    bool _knockbackEnabled;


    public Knockbackeable KnockbackLogic { get => _knockbackLogic; }
    public bool KnockbackEnabled => _knockbackEnabled;


    void Awake()
    {
        //components
        gameObject.CheckComponent<PlayerManager>(ref _player);

        //script that handles the knockback effect
        //there is a problem with this part, you cant really update the speed if the player gets an upgrade
        //you should handle that through the onStatsChange Event on the playerstats script
        if (_knockbackLogic == null) _knockbackLogic = new Knockbackeable(transform, _player.RigidBody, OnKnockbackChange, Mathf.RoundToInt(_player.Stats.KnockbackResistance));

        //script that handles the sorting order based on its position
        if (_sortingOrderController == null) _sortingOrderController = new SortingOrderController(transform, _player.Renderer, _sortingOrderOffset);

    }
    IEnumerator Start()
    {
        _movementInput.action.performed += SetMovement;
        _movementInput.action.canceled += SetIdle;
        _aimingLogic.OnAimingChange += ChangeAiming;
        _spriteGameObject = _player.Renderer.gameObject;
        _flipLogic = new(_spriteGameObject.transform, true, false, 0.15f);
        _realSpeed = MovementSpeed;
        _slowdown = 1f;
        if (_curve == null) _curve = TweenCurveLibrary.EaseOutCirc;
        _dashCurve = TweenCurveLibrary.EaseOutCirc;
        yield return null;
        if (_player.CharacterData.DashData != null) InitializeDashLogic();
        _dashParticleEffect.Stop();
        _playerLayer = gameObject.layer;
    }

    void InitializeDashLogic()
    {
        _dashDurationTimer = new(_player.CharacterData.DashData.DashDuration);
        _dashDurationTimer.Stop();
        _dashDurationTimer.onEnd += StopDash;
        UpdateDashCooldown();

        _dashInput.action.performed += SetDash;
        _player.Stats.onStatsChange += UpdateDashCooldown;

        _dashFXInstance = Instantiate(_dashFXPrefab);
        _dashFXInstance.SetDashReferences(_player.CharacterData.DashData);
        _dashFXInstance.transform.SetParent(transform, false);

        _backDashFXInstance = Instantiate(_dashFXPrefab);
        _backDashFXInstance.SetDashReferences(_player.CharacterData.BackDashData);
        _backDashFXInstance.transform.SetParent(transform, false);

        _dashLogicInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_slowdownTime > 0)
        {
            _slowdownTime -= Time.deltaTime;
        }
        else
        {
            _slowdown = 1f;
        }
        if (_dashLogicInitialized)
        {
            _dashDurationTimer.UpdateTime();
            _dashCooldownTimer.UpdateTime();
        }
        _flipLogic?.UpdateLogic();
    }
    private void FixedUpdate()
    {
        if (_isDashing)
        {
            DashMovement(_dashDirection);
            return;
        }
        if (KnockbackEnabled) _knockbackLogic.ApplyKnockback();
        else if (_movement.sqrMagnitude > 0.1f) Move();
        else Iddle();
    }

    public void SetMovement(InputAction.CallbackContext obj)
    {
        _movement = _movementInput.action.ReadValue<Vector2>();
        _movement.Normalize();
        _lastMovement = _movement;
    }
    void SetIdle(InputAction.CallbackContext obj)
    {
        _movement = Vector2.zero;
    }

    public void SetDash(InputAction.CallbackContext obj)
    {
        if(_dashOnCooldown || _isDashing) return;
        _dashCooldownTimer.Start();
        _dashOnCooldown = true;
        _isDashing = true;
        _startingDashPosition = transform.position;

        bool backDash = true;
        if(_movement.sqrMagnitude > 0.1f)
        {
            var mouseDir = (YYInputManager.i.GetMousePosition() - transform.position).normalized;
            _dashDirection = _movement.normalized;
            if(!_autoAim) backDash = (Mathf.Sign(_movement.x) != Mathf.Sign(FacingDirection));
            else backDash = false;

        }else
        {
            var mouseOppositeDir = (transform.position - YYInputManager.i.GetMousePosition()).normalized;
            Vector2 finalDir = new
            (
                GetVectorValue(mouseOppositeDir.x), GetVectorValue(mouseOppositeDir.y)
            );
            finalDir.Normalize();
            if(finalDir.sqrMagnitude < 0.05f) finalDir = Vector2.right * -FacingDirection;
            _dashDirection =  finalDir;
            
                float GetVectorValue(float value)
                {
                    return (value > 0.12f) ?  1f : (value < -0.12f) ? -1f : 0f;
                }
        }

        var animation = (backDash) ? PlayerAnimationsNames.BackDash : PlayerAnimationsNames.ForwardDash;
        var prefab = (backDash) ? _backDashFXInstance : _dashFXInstance;
        var dashForce = prefab.DashData.ForceMultiplier;
        _dashDirection *= (dashForce * _player.Stats.DashSpeed * Time.fixedDeltaTime);
        prefab.Play(_dashDirection);
        _dashDirection += (Vector2)transform.position;
        _dashDuration = prefab.DashData.DashDuration;
        _dashDurationTimer.ChangeTime(_dashDuration);
        _dashDurationTimer.Start();
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_enemyLayer), true);
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_resourceLayer), true);
        _dashParticleEffect.Play();
        onDash?.Invoke();
        _healthManager.SetInvulnerabilityTime(_player.Stats.DashInvulnerabilityTime + _dashDuration);
        _flipLogic.LockFlip(_dashDuration / 2f);
        if(prefab.DashData.DoBlinkFX) _healthManager.BlinkFX.Play();
        _player.AnimController.UnlockAnimator();
        _player.AnimController.PlayWithDuration(animation, true);
        if(backDash) _elapsedAcceleration = 0f;
        _knockbackLogic.StopKnockback();

    }
    public void StopDash()
    {
        _isDashing = false;
        _elapsedDashTime = 0f;
        _dashParticleEffect.Stop();
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_enemyLayer), false);
        Physics2D.IgnoreLayerCollision(_playerLayer, HelperMethods.GetLayerMaskIndex(_resourceLayer), false);
        _healthManager.BlinkFX.Stop();
        
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
        if(_slowdown <= 0.05f) return;
        if(_elapsedAcceleration < 2f) _elapsedAcceleration += Time.deltaTime;
        var speed = Mathf.Lerp(MovementSpeed * 0.5f * _slowdown , MovementSpeed * _slowdown * 1.1f, _curve.Evaluate(_elapsedAcceleration));
        Vector2 direction = (Vector2)transform.position + _movement * (speed *Time.fixedDeltaTime);
        _player.RigidBody.MovePosition(direction);
        MovementEffects();
        _audio.PlayWithCooldown(_stepSound, _stepSoundCooldown, ref _nextStepSound);
        _player.AnimController.PlayStated(PlayerAnimationsNames.Run);
    }
    void DashMovement(Vector2 moveDirection)
    {
        _elapsedDashTime += Time.fixedDeltaTime;
        var percent = _elapsedDashTime / _dashDuration;
        _player.RigidBody.MovePosition(Vector2.Lerp(_startingDashPosition, moveDirection, _dashCurve.Evaluate(percent)));
        MovementEffects();
        //_elapsedAcceleration = 0f; //only if it is a backstep??
    }

    void MovementEffects()
    {
        _dustEffect.Play();
        //change sprite sorting order based on its position
        _sortingOrderController.SortOrder();

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
        if(_slowdown <= 0) _slowdown = 0f;
    }

    void ChangeAiming(bool state) => _autoAim = state;

    private void OnDestroy() {
        _movementInput.action.performed -= SetMovement;
        _movementInput.action.canceled -= SetIdle;
        _aimingLogic.OnAimingChange -= ChangeAiming;
        if(!_dashLogicInitialized) return;
        _dashInput.action.performed -= SetDash;
        _dashDurationTimer.onEnd -= StopDash;
        _player.Stats.onStatsChange -= UpdateDashCooldown;
    }
}
