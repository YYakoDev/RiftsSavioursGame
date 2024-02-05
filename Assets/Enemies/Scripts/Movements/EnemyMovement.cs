using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IKnockback
{
    EnemyBrain _brain;
    EnemyBaseMovement _movementClass;
    SOEnemyMovementBehaviour _behaviour;
    public Knockbackeable KnockbackLogic => _movementClass.KnockbackLogic;
    //references
    public void Init(EnemyBaseMovement movementLogic, SOEnemyMovementBehaviour movementBehaviour)
    {
        _movementClass = movementLogic;
        _behaviour = movementBehaviour;
    }

    private void Awake() {
        _brain = GetComponent<EnemyBrain>();
    }

    private void OnEnable() {
        _movementClass?.OnEnableLogic();
    }
    private void Start() {
        _brain.Collisions.OnCollisionTriggered += DoKnockbackOnCollision;
    }
    private void Update() {
        _movementClass.UpdateLogic();
        _behaviour?.UpdateLogic();
    }
    private void FixedUpdate() {
        if(!_movementClass.Enabled) return;
        _movementClass.PhysicsLogic();
        _behaviour?.PhysicsLogic();
        Movement();
    }

    void DoKnockbackOnCollision(Transform emitter)
    {
        _movementClass.StopStun(0.265f);
        _movementClass.KnockbackLogic.SetKnockbackData(emitter, _brain.Stats.Speed / 5f, 0.15f, true);
    }

    private void Movement()
    {
        _behaviour.Action();
        _movementClass.Move(_behaviour.GetDirection());
    }

    private void OnDestroy() {
        _brain.Collisions.OnCollisionTriggered -= DoKnockbackOnCollision;
    }
}
