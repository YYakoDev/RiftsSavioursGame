using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollisionDetection : MonoBehaviour
{
    [SerializeField] LayerMask _enemyLayer;
    Collider2D[] _collidersDetected = new Collider2D[20];
    [SerializeField] float _detectionRadius;
    [SerializeField] float _distanceThreshold;
    int _detectionResults = 0;
    private float _detectionRate = 0.1f;
    Timer _collisionDetectionTimer;

    private void Awake() {
        _collisionDetectionTimer = new(_detectionRate, true);
        _collisionDetectionTimer.onEnd += DetectCollisions;
    }

    private void Update() {
        _collisionDetectionTimer.UpdateTime();
    }

    void DetectCollisions()
    {
        Vector2 currentPos = transform.position;
        int results = Physics2D.OverlapCircleNonAlloc(currentPos, _detectionRadius, _collidersDetected);
        if(results == 0) return;
        Debug.Log("Results of the collision detection = " + results);
        
        EnemyCollisions[] enemyObjects = new EnemyCollisions[results];
        for (int i = 0; i < results; i++)
        {
            var collider = _collidersDetected[i];
            GameObject enemyObj = collider.gameObject;
            if(collider == null || !collider.isTrigger || !enemyObj.activeInHierarchy) continue;
            AddEnemy(enemyObj.GetComponent<EnemyCollisions>(), i);
        }

        void AddEnemy(EnemyCollisions enemyComponent, int index)
        {
            //check if it already contains that obj
            bool contains = false;
            for (int i = 0; i < enemyObjects.Length; i++)
            {
                var component = enemyObjects[i];
                if(component == enemyComponent) contains = true;
            }
            if(contains) return;
            enemyObjects[index] = enemyComponent;
        }

        Debug.Log("Added enemies to the checks array:   " + enemyObjects.Length);
    }

    private void OnDestroy() {
        _collisionDetectionTimer.onEnd -= DetectCollisions;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

}
