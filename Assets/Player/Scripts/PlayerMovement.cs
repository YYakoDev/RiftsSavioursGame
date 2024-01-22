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
    [SerializeField] WeaponAiming _aimingScript;
    [SerializeField]ParticleSystem _dustEffect;
    PlayerManager _player;
    GameObject _spriteGameObject;

    [Header("AudioStuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] float _stepSoundCooldown = 0.3f;
    float _nextStepSound = 0f;
    [SerializeField] AudioClip[] _stepSounds;
    private AudioClip _stepSound => _stepSounds[Random.Range(0, _stepSounds.Length)];
    //Movement
    Vector2 _movement;
    Action<Vector2> OnMovement;
    float _realSpeed;

    [SerializeField]AnimationCurve _accelerationCurve;
    float _elapsedAccelerationTime = 0f;

    //Slowdown when attacking
    float _slowdown = 1f;
    float _slowdownTime = 1f;

    //Flipping sprite based on direction
    bool isFlipped;
    float _lockFlipTime = 0f;

    //sort the order of the sprite based on movement
    SortingOrderController _sortingOrderController;
    [SerializeField]private float _sortingOrderOffset; //this adds an offset to the position detected on the sprite    

    //aiming
    bool _autoAiming = false;

    //properties
    //public Vector2 Movement => _movement;
    //public bool IsFlipped => isFlipped;
    public int FacingDirection => (isFlipped) ? -1 : 1;
    private float AccelerationTime => _player.Stats.AccelerationTime;
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
        if(_knockbackLogic == null)_knockbackLogic = new Knockbackeable(transform, _player.RigidBody, OnKnockbackChange, _player.Stats.KnockbackResistance);
        
        //script that handles the sorting order based on its position
        if(_sortingOrderController == null)_sortingOrderController = new SortingOrderController(transform, _player.Renderer, _sortingOrderOffset);
        
    }
    void Start()
    {
        YYInputManager.OnMovement += SetMovement;
        _aimingScript.OnAimingChange += ChangeAimMode;
        _spriteGameObject = _player.Renderer.gameObject;
        _realSpeed = MovementSpeed;
        _elapsedAccelerationTime = 0f;
        _slowdown = 1f;
        //_accelerationCurve = TweenCurveLibrary.GetCurve(CurveTypes.EaseInOut);
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
            _realSpeed = MovementSpeed;
        }
        
    }

    void SetMovement(Vector2 movementInput)
    {
        _movement = movementInput;
        _movement.Normalize();
    }

    private void FixedUpdate()
    {
        if(KnockbackEnabled) _knockbackLogic.ApplyKnockback();
        else if(_movement.sqrMagnitude > 0.1f) Move();
        else Iddle();
    }

    void Iddle()
    {
        _player.AnimController.PlayStated(PlayerAnimationsNames.Iddle);
        _sortingOrderController.SortOrder();
        _dustEffect.Stop();
        _elapsedAccelerationTime = 0f;
    }

    void Move()
    {
        _dustEffect.Play();
        _audio.PlayWithCooldown(_stepSound, _stepSoundCooldown, ref _nextStepSound);
        _elapsedAccelerationTime += Time.fixedDeltaTime;
        float percent = _elapsedAccelerationTime / AccelerationTime;
        if(percent <= 1.1f) _realSpeed = Mathf.Lerp(0, MovementSpeed, _accelerationCurve.Evaluate(percent)) * _slowdown;

        Vector2 direction = (Vector2)transform.position + _movement * (_realSpeed *Time.fixedDeltaTime);
        _player.RigidBody.MovePosition(direction);
        
        CheckForFlip(_movement.x);

        //change sprite sorting order based on its position
        _sortingOrderController.SortOrder();

        _player.AnimController.PlayStated(PlayerAnimationsNames.Run);

        OnMovement?.Invoke(_movement);
    }

    public void OnKnockbackChange(bool change)
    {
        _knockbackEnabled = change;
    }

    void ChangeAimMode(bool state) => _autoAiming = state;
    


    public void CheckForFlip(float xDirection, float lockFlipTime = 0f)
    {
        if(_autoAiming && _aimingScript.EnemyResultsCount > 0)
        {
            _lockFlipTime = 0f;
            xDirection = _aimingScript.TargetPoint.x;
        }
        if(_lockFlipTime > Time.time)return;
        _lockFlipTime = Time.time + lockFlipTime;

        if(xDirection < 0 && !isFlipped)
        {
            Flip();

        }else if(xDirection > 0 && isFlipped)
        {
            Flip();
        }

        void Flip()
        {
            isFlipped = !isFlipped;
            //_player.Renderer.flipX = isFlipped;

            Vector3 invertedScale = _spriteGameObject.transform.localScale;
            invertedScale.x *= -1;
            _spriteGameObject.transform.localScale = invertedScale;
        }
    }

    public void SlowdownMovement(float slowdownTime)
    {
        _slowdownTime = slowdownTime;
        _slowdown = SlowdownMultiplier;
        _elapsedAccelerationTime = 0f;
    }
    public void SlowdownMovement(float duration, float force)
    {
        _slowdownTime = duration;
        _slowdown = force - SlowdownMultiplier;
        if(_slowdown <= 0) _slowdown = 0.1f;
        _elapsedAccelerationTime = 0f;
    }

    private void OnDestroy() {
        YYInputManager.OnMovement -= SetMovement;
        _aimingScript.OnAimingChange -= ChangeAimMode;
    }
}
