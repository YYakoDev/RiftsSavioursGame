using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement
{
    public bool Enabled = true;

    [Header("References")]
    Transform _transform;
    EnemyBrain _enemy;
    SortingOrderController _sortOrderController;

    //
    Timer _movementStopTimer;
    bool _stopMovement;
    bool _isFlipped;


    //KNOCKBACK LOGIC
    private Knockbackeable _knockbackLogic;
    bool knockbackEnabled;
    public Knockbackeable KnockbackLogic => _knockbackLogic;


    //properties


    public EnemyBaseMovement(Transform transform, EnemyBrain enemy, float spriteOffset)
    {
        _transform = transform;
        _enemy = enemy;
        _sortOrderController = new SortingOrderController(transform, _enemy.Renderer, spriteOffset);
        _knockbackLogic = new(transform, _enemy.Rigidbody, OnKnockbackChange, _enemy.Stats.KnockbackResistance);
        _movementStopTimer = new(0f);
        _movementStopTimer.onEnd += ResumeMovement;
        _movementStopTimer.Stop();
        knockbackEnabled = false;
    }

    public void UpdateLogic()
    {
        if(!Enabled) return;
        _movementStopTimer.UpdateTime();
    }
    public void PhysicsLogic()
    {
        if(!Enabled) return;
        if(knockbackEnabled) _knockbackLogic.ApplyKnockback();
        
    }

    public void Move(Vector2 direction)
    {
        MoveLogic(direction, direction);
    }
    public void Move(Vector2 direction, Vector2 flipCheckDirection)
    {
        MoveLogic(direction, flipCheckDirection);
    }

    void MoveLogic(Vector2 direction, Vector2 flipDir)
    {
        if(_stopMovement || knockbackEnabled) return;
        _enemy.Rigidbody.velocity = Vector2.zero;
        //Vector2 directionToMove = _avoidanceBehaviour.ResultDirection * (_enemy.Stats.Speed * Time.fixedDeltaTime);        
        Vector2 directionToMove = direction * (_enemy.Stats.Speed * Time.fixedDeltaTime);
        if(directionToMove == Vector2.zero) return;
        _enemy.Rigidbody.MovePosition((Vector2)_transform.position + directionToMove);

        CheckForFlip(flipDir);
        
        _sortOrderController.SortOrder();
        _enemy.Animation.PlayMove();

    }

    public void Iddle()
    {
        _enemy.Animation.PlayIddle();
    }

    
    void OnKnockbackChange(bool change)
    {
        knockbackEnabled = change;
        if(!change)Stun(_enemy.Stats.StunDuration); // the !change means the knockback has been deactivated/finished
        //apply knockback/hit animation
    }

    void Stun(float duration)
    {
        if(!Enabled) return;
        _stopMovement = true;
        _movementStopTimer.ChangeTime(duration);
        _movementStopTimer.Start();
        _enemy.Animation?.ChangeAnimatorSpeed(0.5f, duration + 0.1f);
        Iddle(); // <--- hit animation instead of iddle
    }
    public void ResumeMovement()
    {
        _stopMovement = false;
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
            Vector3 invertedScale = _transform.localScale;
            invertedScale.x *= -1;
            _transform.localScale = invertedScale;
        }
    }
    ~EnemyBaseMovement()
    {
        _movementStopTimer.onEnd -= ResumeMovement;
    }

}
