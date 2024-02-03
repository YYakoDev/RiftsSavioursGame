using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IKnockback
{
    EnemyBaseMovement _movementClass;
    SOEnemyMovementBehaviour _behaviour;
    public Knockbackeable KnockbackLogic => _movementClass.KnockbackLogic;
    //references
    public void Init(EnemyBaseMovement movementLogic, SOEnemyMovementBehaviour movementBehaviour)
    {
        _movementClass = movementLogic;
        _behaviour = movementBehaviour;
    }
    private void OnEnable() {
        _movementClass?.OnEnableLogic();
        
    }
    private void Update() {
        _movementClass.UpdateLogic();
        _behaviour.UpdateLogic();
    }
    private void FixedUpdate() {
        if(!_movementClass.Enabled) return;
        _movementClass.PhysicsLogic();
        _behaviour.PhysicsLogic();
        Movement();
    }

    private void Movement()
    {
        _behaviour.Action();
        _movementClass.Move(_behaviour.GetDirection());
    }
}
