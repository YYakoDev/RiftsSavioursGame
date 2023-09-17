using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovement : MonoBehaviour, IMovement, ITargetPositionProvider
{
    [SerializeField]EnemyBrain _brain;
    [SerializeField]EnemyDetector _enemyDetector;
    SortingOrderController _sortOrderController;
    Transform _target;

    bool _isFlipped;
    bool _stopMovement = false;

    //properties
    public Transform TargetTransform { get => _target; set => _target = value;}
    public int FacingDirection => 1;
    public bool StopMoving { get => _stopMovement;}
    

    private void Awake() 
    {
        gameObject.CheckComponent<EnemyBrain>(ref _brain);
        if(_enemyDetector == null) _enemyDetector = GetComponentInChildren<EnemyDetector>();
    }

    private void OnEnable() {
        _stopMovement = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _brain.HealthManager.onDeath += StopMovement;
        if(_sortOrderController == null) _sortOrderController = new SortingOrderController(transform, _brain.Renderer, -0.1f, 25);
        if(_target == null)//TEST ONLY
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log(gameObject.name + " has no target, setting player as target");
        }
    }

    private void FixedUpdate()
    {
        if(_stopMovement) return;
        Move();
    }

    public void Move()
    {
        if(_target == null)return;
        if(Vector2.Distance(transform.position, _target.position) < 0.2f)return;

        Vector2 direction = (_target.position - transform.position) + (Vector3)_enemyDetector.AvoidanceDirection;
        direction.Normalize();
        Vector2 movement = direction * (_brain.Stats.Speed * Time.fixedDeltaTime);
        
        _brain.Rigidbody.MovePosition((Vector2)transform.position + movement);
        _brain.Animation.PlayMove();
        CheckForFlip(direction);
        _sortOrderController.SortOrder();
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

    private void OnDestroy() {
        _brain.HealthManager.onDeath -= StopMovement;
    }


}
