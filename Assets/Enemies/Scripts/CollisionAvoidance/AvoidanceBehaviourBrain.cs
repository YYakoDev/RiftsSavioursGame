using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AvoidanceData))]
public class AvoidanceBehaviourBrain : MonoBehaviour
{
    [Header("References")]
    //EnemyBrain _brain;
    AvoidanceData _avoidanceData;
    ObstacleAvoidance _obstacleAvoidance;
    TargetInterestMap _targetInterestMap;
    [SerializeField]ObstacleDetector _obstacleDetector;
    [SerializeField]EnemyDetector _enemyDetector;

    [Header("Obstacle Avoidance Stats")]
    [SerializeField]float _colliderDetectionRadius = 1.4f;
    [SerializeField]float _distanceSensitivity = 0.6f;
    //[SerializeField]bool _showDebugDanger = true;
    //[SerializeField]bool _showDebugTargetDir = true;
    [SerializeField]Vector3 _raycastPositionOffset;
    
    Vector2 _targetPositionOffset;
    Vector2 _resultDirection = Vector2.zero; 
    
    //properties
    public Vector2 ResultDirection => _resultDirection;
    public ObstacleAvoidance ObstacleAvoidance => _obstacleAvoidance;
    public TargetInterestMap TargetInterestMap => _targetInterestMap;
    public Vector3 ownPositionWithOffset => transform.position + _raycastPositionOffset;

    private void Awake() 
    {
        //gameObject.CheckComponent<EnemyBrain>(ref _brain);
        gameObject.CheckComponent<AvoidanceData>(ref _avoidanceData);
        if(_obstacleDetector == null)_obstacleDetector = gameObject.GetComponentInChildren<ObstacleDetector>();
        if(_enemyDetector == null)_enemyDetector = gameObject.GetComponentInChildren<EnemyDetector>();
        _targetPositionOffset = Random.insideUnitCircle * 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeData();
    }
    void InitializeData()
    {
        if(_avoidanceData.TargetTransform == null) _avoidanceData.TargetTransform = GameObject.FindGameObjectWithTag("Player").transform;

        _obstacleDetector.Initialize(_avoidanceData, _colliderDetectionRadius);
        _obstacleAvoidance = new ObstacleAvoidance(_avoidanceData);
        _targetInterestMap = new TargetInterestMap(_avoidanceData);

    }
    private void FixedUpdate() 
    {
        Vector3 selfPosition = ownPositionWithOffset;

        //detecting the danger levels towards the obstacle
        _obstacleAvoidance.SetDirection(_colliderDetectionRadius, _distanceSensitivity, selfPosition);
        //detecting the interest level towards the target(player)
        _targetInterestMap.SetDirection(_colliderDetectionRadius, _distanceSensitivity, selfPosition);
        
        //DEBUG
        //if(_showDebugDanger)_obstacleAvoidance.ShowDebugLines(_obstacleAvoidance.DangerDirections, Color.black, selfPosition);   
        //if(_showDebugTargetDir)_targetInterestMap.ShowDebugLines(_targetInterestMap.InterestMap, Color.cyan, selfPosition);

        //direction 
        SetWeightedDirection();

    }

    void SetWeightedDirection()
    {
        _resultDirection = Vector2.zero;
        _resultDirection = (_targetInterestMap.InterestDirection + (_targetPositionOffset)) - _obstacleAvoidance.AverageDangerDirection;
        _resultDirection += _enemyDetector.AvoidanceDirection;
        _resultDirection.Normalize();
    }
    private void OnDrawGizmosSelected() 
    {
        if(Application.isPlaying)return;
        Gizmos.color = Color.green;
        //Gizmos.DrawRay(ownPositionWithOffset, _resultDirection);

        Gizmos.DrawSphere(ownPositionWithOffset, 0.2f);    
    }
}
