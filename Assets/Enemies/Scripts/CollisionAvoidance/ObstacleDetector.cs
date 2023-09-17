using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ObstacleDetector : MonoBehaviour
{
    [SerializeField]AvoidanceData _data;
    CircleCollider2D _collider;

    private void Awake() {
        gameObject.CheckComponent<CircleCollider2D>(ref _collider);
    }
    public void Initialize(AvoidanceData data, float detectionRadius)
    {
        _data = data;
        _collider.radius = detectionRadius;
    }

    /*public void Detect(AIData data, float detectionRadius, bool showGizmos = true)
    {
        DetectionRadius = detectionRadius;
        ShowGizmos = showGizmos;

        _colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, ObstacleAndPlayerMask);
        data.Obstacles = _colliders.ToArray();
    }*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.isTrigger)return;
        _data.Obstacles.Add(other);

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.isTrigger)return;
        _data.Obstacles.Remove(other);
    }



}
