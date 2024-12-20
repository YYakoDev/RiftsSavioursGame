using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseMovement
{
    public bool Enabled = true;

    [Header("References")]
    Transform _transform;
    EnemyBrain _enemy;
    Rigidbody2D _rb;
    SortingOrderController _sortOrderController;
    float _realSpeed;
    float _elapsedTime = 0f;
    float _accelerationTime = 1f;
    AnimationCurve _accelerationCurve;
    private bool _stopStun = false;
    Timer _stunStopperTimer;
    Timer _movementStopTimer;
    bool _stopMovement;
    FlipLogic _flipLogic;
    bool IsFlipped => _flipLogic.IsFlipped;


    //KNOCKBACK LOGIC
    private Knockbackeable _knockbackLogic;
    bool knockbackEnabled;
    public Knockbackeable KnockbackLogic => _knockbackLogic;
    public bool StopMovement
    {
        get => _stopMovement;
        set => _stopMovement = value;
    }

    //properties


    public EnemyBaseMovement(Transform transform, EnemyBrain enemy, float spriteOffset)
    {
        _transform = transform;
        _enemy = enemy;
        _rb = enemy.Rigidbody;
        _sortOrderController = new SortingOrderController(transform, _enemy.Renderer, spriteOffset);
        _knockbackLogic = new(transform, _rb, OnKnockbackChange, _enemy.Stats.KnockbackResistance);
        _movementStopTimer = new(0f);
        _movementStopTimer.onEnd += ResumeMovement;
        _stunStopperTimer = new(0f);
        _stunStopperTimer.onEnd += ResumeStunLogic;
        _movementStopTimer.Stop();
        knockbackEnabled = false;
        _accelerationCurve = TweenCurveLibrary.GetCurve(CurveTypes.EaseInOut);
        _flipLogic = new(_enemy.Renderer.transform, true, false, 0.15f + Random.Range(-0.055f, 0.055f));
    }

    public void ReInitialize(Transform transform, EnemyBrain enemy, float spriteOffset = 0.25f)
    {
        _transform = transform;
        _enemy = enemy;
        _rb = enemy.Rigidbody;
        _sortOrderController.ReInitialize(transform, _enemy.Renderer, spriteOffset);
        _knockbackLogic.ReInitialize(transform, _rb, OnKnockbackChange, enemy.Stats.KnockbackResistance);
        _movementStopTimer.ChangeTime(0f);
        _movementStopTimer.Stop();
        knockbackEnabled = false;
    }

    public void OnEnableLogic()
    {
        Enabled = false;
        KnockbackLogic.StopKnockback();
        Enabled = true;
        _stopStun = false;
        ResumeMovement();
        _elapsedTime = 0;
    }

    public void UpdateLogic()
    {
        _flipLogic.UpdateLogic();
        if(!Enabled) return;
        _movementStopTimer.UpdateTime();
        _stunStopperTimer.UpdateTime();
    }
    public void PhysicsLogic()
    {
        if(knockbackEnabled) _knockbackLogic.ApplyKnockback();
        if(!Enabled) return;
        
    }

    public void Move(Vector2 direction)
    {
        if(_stopMovement || knockbackEnabled) return;
        var fixedDeltaTime = Time.fixedDeltaTime;
        var vectorZero = Vector2.zero;
        _rb.velocity = vectorZero;
        //Vector2 directionToMove = _avoidanceBehaviour.ResultDirection * (_enemy.Stats.Speed * Time.fixedDeltaTime);        
        _elapsedTime += fixedDeltaTime;
        float percent = _elapsedTime / _accelerationTime;
        if(percent <= 1.1f)  _realSpeed = Mathf.Lerp(0, _enemy.Stats.Speed, _accelerationCurve.Evaluate(percent));
        Vector2 directionToMove = (direction + _enemy.EnemyDetector.AvoidanceDirection).normalized * (_realSpeed * fixedDeltaTime);
        if(directionToMove == vectorZero) return;
        _rb.MovePosition((Vector2)_transform.position + directionToMove);

        _flipLogic.FlipCheck(direction.x);
        
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
        if(!Enabled || _stopStun) return;
        _stopMovement = true;
        float realDuration = duration - duration * _enemy.Stats.StunResistance / 100f;
        _movementStopTimer.ChangeTime(realDuration);
        _movementStopTimer.Start();
        _enemy.Animation?.ChangeAnimatorSpeed(0.5f, realDuration + 0.1f);
        _elapsedTime = _accelerationTime / 2f + _accelerationTime * _enemy.Stats.StunResistance / 100f;
        //Iddle(); // <--- hit animation instead of iddle
    }
    public void StopStun(float time)
    {
        _stopStun = true;
        _stunStopperTimer.ChangeTime(time);
        _stunStopperTimer.Start();
    }
    void ResumeStunLogic() => _stopStun = false;
    public void ResumeMovement()
    {
        _stopMovement = false;
    }

    ~EnemyBaseMovement()
    {
        _movementStopTimer.onEnd -= ResumeMovement;
        _stunStopperTimer.onEnd -= ResumeStunLogic;
    }

}
