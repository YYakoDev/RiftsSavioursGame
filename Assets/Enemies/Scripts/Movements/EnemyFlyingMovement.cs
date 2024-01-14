using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovement : MonoBehaviour, ITargetPositionProvider, IKnockback, IEnemyMovement
{
    EnemyBaseMovement _movementClass;
    [SerializeField] EnemyDetector _enemyDetector;
    Transform _target;
    //
    public Transform TargetTransform { get => _target; set => _target = value; }
    public Knockbackeable KnockbackLogic => _movementClass.KnockbackLogic;

    public EnemyBaseMovement MovementLogic { get => _movementClass; set => _movementClass = value; }

    private void Awake()
    {
        if (_enemyDetector == null) _enemyDetector = GetComponentInChildren<EnemyDetector>();
    }

    private void OnEnable()
    {
        _movementClass.ResumeMovement();
    }
    void Start()
    {
        if (_target == null)//TEST ONLY
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log(gameObject.name + " has no target, setting player as target");
        }
    }

    private void Update()
    {
        _movementClass.UpdateLogic();
    }

    private void FixedUpdate()
    {
        _movementClass.PhysicsLogic();
        Movement();
    }

    public void Movement()
    {
        if(!_movementClass.Enabled) return;
        if (_target == null) return;
        if (Vector2.Distance(transform.position, _target.position) < 0.2f) return;

        Vector2 direction = (_target.position - transform.position) + (Vector3)_enemyDetector.AvoidanceDirection;
        direction.Normalize();
        _movementClass.Move(direction);
    }

}
