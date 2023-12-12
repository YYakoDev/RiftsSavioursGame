using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerManager))]
public class PlayerMovement : MonoBehaviour, IKnockback
{
    //References
    PlayerManager _player;
    GameObject _spriteGameObject;
    [SerializeField]ParticleSystem _dustEffect;

    //Movement
    Vector2 _movement;
    float _realSpeed;

    //Slowdown when attacking
    float _slowdownTime = 0f;

    //Flipping sprite based on direction
    bool isFlipped;
    float _lockFlipTime = 0f;

    //sort the order of the sprite based on movement
    SortingOrderController _sortingOrderController;
    [SerializeField]private float _sortingOrderOffset; //this adds an offset to the position detected on the sprite    

    //properties
    //public Vector2 Movement => _movement;
    //public bool IsFlipped => isFlipped;
    public int FacingDirection => (isFlipped) ? -1 : 1;
    private float MovementSpeed => _player.Stats.Speed;
    private float SlowdownMultiplier => _player.Stats.SlowdownMultiplier;
    public event Action<Vector2> OnMovement;
    //Knockback Stuff
    Knockbackeable _knockbackLogic;
    public Knockbackeable KnockbackLogic { get => _knockbackLogic;}
    
    void Awake()
    {
        //components
        gameObject.CheckComponent<PlayerManager>(ref _player);

        //script that handles the knockback effect
        //there is a problem with this part, you cant really update the speed if the player gets an upgrade
        //you should handle that through the onStatsChange Event on the playerstats script
        if(_knockbackLogic == null)_knockbackLogic = new Knockbackeable(transform, _player.RigidBody, true);
        
        //script that handles the sorting order based on its position
        if(_sortingOrderController == null)_sortingOrderController = new SortingOrderController(transform, _player.Renderer, _sortingOrderOffset);
        
    }
    void Start()
    {
        _spriteGameObject = _player.Renderer.gameObject;
        _realSpeed = MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        if(_slowdownTime > 0)
        {
            _slowdownTime -= Time.deltaTime;
        }else
        {
            _realSpeed = MovementSpeed;
        }
    }

    private void FixedUpdate()
    {
        if(_movement.sqrMagnitude > 0.1f) Move();
        else Iddle();
        if(_knockbackLogic.Enabled) _knockbackLogic.ApplyKnockback();
    }

    void Iddle()
    {
        _player.AnimController.PlayStated(PlayerAnimationsNames.Iddle);
        _sortingOrderController.SortOrder();
        _dustEffect.Stop();
    }

    void Move()
    {
        _dustEffect.Play();
        Vector2 direction = (Vector2)transform.position + _movement.normalized * (_realSpeed *Time.fixedDeltaTime);
        _player.RigidBody.MovePosition(direction);
        
        CheckForFlip(_movement.normalized.x);

        //change sprite sorting order based on its position
        _sortingOrderController.SortOrder();

        _player.AnimController.PlayStated(PlayerAnimationsNames.Run);

        //OnMovement.Invoke(_movement);
    }

    public void CheckForFlip(float direction, float lockFlipTime = 0f)
    {
        if(_lockFlipTime > Time.time)return;
        _lockFlipTime = Time.time + lockFlipTime;

        if(direction < 0 && !isFlipped)
        {
            Flip();

        }else if(direction > 0 && isFlipped)
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
        _realSpeed = MovementSpeed * SlowdownMultiplier;
    }
}
