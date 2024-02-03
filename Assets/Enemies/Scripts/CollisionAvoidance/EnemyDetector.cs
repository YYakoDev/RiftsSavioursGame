using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField]LayerMask _enemyLayer;
    float _detectionRadius = 1f;
    Collider2D[] _colliders;
    Vector3 _avoidanceDirection = Vector2.zero;
    Transform _parent;
    int _maxCollidersToDetect = 4;
    float _detectionRate = 0.1f; // 1/10 seconds
    float _nextDetectionTime; //the actual timer
    float _randomizedFrameWait; //randomized wait time to avoid all enemies detecting at the same time

    //properties
    public Vector2 AvoidanceDirection => _avoidanceDirection;

    public void SetRadius(float radius) => _detectionRadius = radius;

    // Start is called before the first frame update
    void Start()
    {
        _parent = transform.parent;
        _randomizedFrameWait = Random.Range(-0.035f,0.049f);
        _nextDetectionTime = _detectionRate + _randomizedFrameWait;
        _colliders = new Collider2D[_maxCollidersToDetect + Random.Range(-1,2)];
        _detectionRadius += Random.Range(-0.24f,0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if(_nextDetectionTime <= 0)
        {
            Detection(_parent.position);
            _nextDetectionTime = _detectionRate + _randomizedFrameWait;
        }else
        {
            _nextDetectionTime -= 1 * Time.deltaTime;
        }
    }

    void Detection(Vector3 selfPosition)
    {
        int count = Physics2D.OverlapCircleNonAlloc(selfPosition, _detectionRadius, _colliders, _enemyLayer);
        _avoidanceDirection = Vector3.zero; //reseting avoidance direction 
        for(int i = 0; i < count;i++)
        {
            if(_colliders[i] == null || _colliders[i].transform == _parent) continue;
            _avoidanceDirection += selfPosition - _colliders[i].transform.position;
        }
    }

}
