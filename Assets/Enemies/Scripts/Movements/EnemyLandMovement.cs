using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyLandMovement : MonoBehaviour, IKnockback, IEnemyMovement
{   
    //[SerializeField] AvoidanceBehaviourBrain _avoidanceBehaviour;
    EnemyBaseMovement _movementClass;

    public Knockbackeable KnockbackLogic => _movementClass.KnockbackLogic;

    public EnemyBaseMovement MovementLogic { get => _movementClass; set => _movementClass = value; }

    //references
    private void Awake() {
        //gameObject.CheckComponent<AvoidanceBehaviourBrain>(ref _avoidanceBehaviour);
    }
    private void OnEnable() {
        _movementClass?.OnEnableLogic();
    }
    private void Update() {
        _movementClass.UpdateLogic();
    }
    private void FixedUpdate() {
        _movementClass.PhysicsLogic();
        Movement();
    }

    void Movement()
    {
        if(!_movementClass.Enabled) return;
        //_movementClass.Move(_avoidanceBehaviour.ResultDirection.normalized, _avoidanceBehaviour.TargetInterestMap.InterestDirection);
    }
}
