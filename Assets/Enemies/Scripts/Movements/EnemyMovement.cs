using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AvoidanceBehaviourBrain))]
public class EnemyMovement : MonoBehaviour, IMovement, IKnockback
{
    //references
    [Header("References")]
    EnemyBrain _enemy;
    SortingOrderController _sortOrderController;
    AvoidanceBehaviourBrain _avoidanceBehaviour;


    [Header("Movement Stats")]
    bool _stopMovement = false;
    bool _isFlipped;

    [Header("Sorting Sprite Order")]
    [SerializeField]float _offsetSortOrderPosition;



    //KNOCKBACK LOGIC
    Knockbackeable _knockbackeable;
    

    //properties
    public int FacingDirection => (_isFlipped) ? -1 : 1;
    public Knockbackeable KnockBackLogic { get => _knockbackeable;}
    public bool StopMoving { get => _stopMovement;}
    public bool knockback { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake() 
    {
        gameObject.CheckComponent<EnemyBrain>(ref _enemy);
        gameObject.CheckComponent<AvoidanceBehaviourBrain>(ref _avoidanceBehaviour);

        if(_knockbackeable == null) _knockbackeable = new Knockbackeable(transform);
        if(_sortOrderController == null) _sortOrderController = new SortingOrderController(transform, _enemy.Renderer, _offsetSortOrderPosition);
        
    }
    private void OnEnable()
    {
        _enemy.HealthManager.onDeath += StopMovement;
        _enemy.HealthManager.onDeath += DisableKnockback;
        _stopMovement = false;
        _knockbackeable.Enabled = true;  
    }
    private void FixedUpdate() 
    {
        if(_stopMovement) return;

        if(_avoidanceBehaviour.ResultDirection.sqrMagnitude > 0.1f) Move();
        else Iddle();
    }

    public void Move()
    {
        _enemy.Rigidbody.velocity = Vector2.zero;

        //Vector2 resultDirection = _avoidanceBehaviour.ResultDirection;
        Vector2 directionToMove = _avoidanceBehaviour.ResultDirection * (_enemy.Stats.Speed * Time.fixedDeltaTime);        
        if(directionToMove == Vector2.zero) return;
        _enemy.Rigidbody.MovePosition((Vector2)transform.position + directionToMove);

        CheckForFlip(_avoidanceBehaviour.TargetInterestMap.InterestDirection);
        
        _sortOrderController.SortOrder();
        _enemy.Animation.PlayMove();

    }

    public void Iddle()
    {
        _enemy.Animation.PlayIddle();
    }

    public void StopMovement()
    {
        _stopMovement = true;
    }

    void CheckForFlip(Vector2 direction)
    {
        if(direction.x > 0 && _isFlipped)
        {
            Flip();
        }
        else if(direction.x < 0 && !_isFlipped)
        {
            Flip();
        }

        void Flip()
        {
            _isFlipped = !_isFlipped;
            Vector3 invertedScale = transform.localScale;
            invertedScale.x *= -1;
            transform.localScale = invertedScale;
        }
    }

    void DisableKnockback()
    {
        _knockbackeable.Enabled = false;
    }

    private void OnDisable() {
        _enemy.HealthManager.onDeath -= StopMovement;
        _enemy.HealthManager.onDeath -= DisableKnockback;
        _stopMovement = false;
    }
   
}
