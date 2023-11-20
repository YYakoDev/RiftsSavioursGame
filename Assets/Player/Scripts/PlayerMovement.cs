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
    Knockbackeable _knockeable;
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
    public Vector2 Movement => _movement;
    public bool IsFlipped => isFlipped;
    public int FacingDirection => (isFlipped) ? -1 : 1;
    public Knockbackeable KnockBackLogic { get => _knockeable;}
    private float MovementSpeed => _player.Stats.Speed;
    private float SlowdownMultiplier => _player.Stats.SlowdownMultiplier;

    public bool knockback { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Awake()
    {
        //components
        gameObject.CheckComponent<PlayerManager>(ref _player);

        //script that handles the knockback effect
        if(_knockeable == null)_knockeable = new Knockbackeable(transform);
        
        //script that handles the sorting order based on its position
        if(_sortingOrderController == null)_sortingOrderController = new SortingOrderController(transform, _player.Renderer, _sortingOrderOffset);
        
    }
    private void OnEnable() {
        
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

    private void OnDisable() {
        
    }

}
