using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AStarUnit : MonoBehaviour
{
    //References
    [SerializeField]Transform _target;
    Rigidbody2D _rb;

    //
    Vector3 _lastTargetPosition = Vector3.zero;
    [SerializeField]float _speed = 5;
    Vector3[] _path;
    Vector3 _currentWaypoint = Vector3.zero;
    //Vector3 _nextWaypoint = Vector3.zero;
    int _targetIndex;
    AStarPathResult _pathResult = AStarPathResult.PathUnsuccessful;
    bool _followTarget = false;

    //float _requestFrequency = 0.1f;
    //float _nextRequestTime;

    //Reposition & Avoidance
    [SerializeField]float _detectionRadius = 2;
    //[SerializeField]LayerMask _playerLayer;
    [SerializeField]LayerMask _obstacleLayer;
    bool _isAvoiding;
    float _avoidanceCountdown;
    Collider2D _obstacleCollider;


    //debug - gizmos
    [SerializeField]bool _showGizmosPath = false;
    Vector3 _directionToWaypoint = Vector3.zero;
    Vector3 _directionToTarget = Vector3.zero;



    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
       
    }

    private IEnumerator Start() {
        yield return null;
        AStarPathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
        //InvokeRepeating("DetectPlayer", 1, 0.1f);
    }

    private void Update()
    {
        //do the request frequency thing here
        //cut and paste the distance check from the fixedupdate
        if(_isAvoiding)
        {
            if(_avoidanceCountdown > 0)
            {
                _avoidanceCountdown -= Time.deltaTime;
            }else
            {
                _isAvoiding = false;
                _obstacleCollider = null;
                AStarPathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
            }
        }
    }    
    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPos = _target.position;
        _directionToTarget = targetPos - currentPosition;
        _directionToTarget.Normalize();

        if(_followTarget)
        {
            //Debug.Log("Moving to target position");
            MoveToDirection(currentPosition, _directionToTarget);
        }else if(_pathResult == AStarPathResult.StartingPositionUnwalkable)
        {
            if(_obstacleCollider == null)
            {
                _obstacleCollider = Physics2D.OverlapCircle(currentPosition, _detectionRadius, _obstacleLayer);
            }
            if(_obstacleCollider == null)
            {
                _followTarget = true;
                return;
            }
            
            //Debug.Log("Avoiding Obstacle");
            Vector3 directionFromObstacle = currentPosition - _obstacleCollider.transform.position;
            directionFromObstacle.Normalize();
            directionFromObstacle += _directionToTarget * Time.fixedDeltaTime;
            MoveToDirection(currentPosition, directionFromObstacle);
            
        }else if(_pathResult != AStarPathResult.PathUnsuccessful)
        {
            //Debug.Log("Following Path");
            FollowPath(currentPosition, _directionToTarget);
        }
 
        if(targetPos != _lastTargetPosition && !_isAvoiding && Vector3.Distance(currentPosition, targetPos) > _detectionRadius * 2)
        {
            //Debug.Log("Requesting New Path");
            AStarPathRequestManager.RequestPath(currentPosition, targetPos, OnPathFound);
            _lastTargetPosition = targetPos;
            //Debug.Log("Path Result:  " + _pathResult);

        }
    }

    void FollowPath(Vector3 currentPosition, Vector3 directionToTarget)
    {
        _directionToWaypoint = _currentWaypoint - currentPosition;
        _directionToWaypoint.Normalize();
        
        /*if(Vector2.Dot(_directionToWaypoint, directionToTarget) < -0.45f)
        {
            Debug.Log("Setting new direction to waypoint");
            _directionToWaypoint += directionToTarget * 1.35f;
            _directionToWaypoint.Normalize();  
            //Debug.Log("Setting new direction to waypoint, current waypoint is in: "  + _currentWaypoint + "  Coordinates");
            _currentWaypoint = currentPosition + _directionToWaypoint;
        }*/

        if(Vector3.Distance(currentPosition, _currentWaypoint) < 0.25f) SetNewWaypoint();
        MoveToDirection(currentPosition, _directionToWaypoint);

        void SetNewWaypoint()
        {
            _targetIndex++;
            if(_targetIndex >= _path.Length)
            {
                //path end reached proceding to move to the player position in the fixed update
                _followTarget = true;
                return;
            }
            _currentWaypoint = _path[_targetIndex];
            //_nextWaypoint = (_targetIndex+1 >= _path.Length) ? _currentWaypoint += directionToTarget : _path[_targetIndex++];
        }
    }
    void MoveToDirection(Vector3 currentPosition, Vector3 direction)
    {
        Vector3 finalDirection = currentPosition + direction * (_speed * Time.fixedDeltaTime);
        _rb.MovePosition(finalDirection);
    }

    void OnPathFound(Vector3[] newPath, AStarPathResult pathResult)
    {
        _pathResult = pathResult;
        
        switch(_pathResult)
        {
            case AStarPathResult.PathSuccessful:
            {
                if(newPath.Length == 0) return;
                _path = newPath;
                _targetIndex = 0;
                _currentWaypoint = _path[0];
                _followTarget = false;
                _isAvoiding = false;
            }break;

            case AStarPathResult.TargetUnreachable:
            {
                //Debug.Log("Target is Unreacheable");
                Vector3 currentPosition = transform.position;
                Vector3 targetPosition = _target.position;
                if(_path != null)
                {
                    if(_targetIndex < _path.Length)
                    {
                        _pathResult = AStarPathResult.PathSuccessful;
                        _followTarget = false;
                        _currentWaypoint = _path[_targetIndex];
                    }
                }
            }break;
            case AStarPathResult.StartingPositionUnwalkable:
            {
                _followTarget = false;
                _isAvoiding = true;
                _avoidanceCountdown = 0.3f;
            }
            break;
        }
    }



    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + _directionToWaypoint);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + _directionToTarget);

        if(_path == null || !_showGizmosPath) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        for (int i = _targetIndex; i < _path.Length; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(_path[i], Vector3.one);

            if(i == _targetIndex)
            {
                Gizmos.DrawLine(transform.position, _path[i]);
            }else
            {
                Gizmos.DrawLine(_path[i-1], _path[i]);
            }
        }
    }

}
