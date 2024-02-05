using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOEnemyMovementBehaviour : SOEnemyBehaviour
{
    protected Transform _target;
    protected Transform _enemyTransform;
    protected Vector2 _direction = Vector2.zero;
    public override void Initialize(EnemyBrain brain)
    {
        base.Initialize(brain);
        _target = brain.TargetTransform;
        _enemyTransform = brain.transform;
        //Debug.Log(_target);
    }
    public virtual void UpdateLogic(){}
    public virtual void PhysicsLogic(){}

    public void UpdateTarget(Transform target) 
    {
        _target = target;
    }

    public override void Action()
    {
        base.Action();
        _direction = (_target.position - _enemyTransform.position);
    }
    public virtual Vector2 GetDirection() => _direction;
}
