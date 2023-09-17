using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : ContextProcessor
{
    //private float[] _dangerDirections = new float[8];
    private const string PlayerTag = "Player";
    private Vector2 _dangerDirection = Vector2.zero;

    //properties
    //public float[] DangerMap => _dangerDirections;
    public Vector2 AverageDangerDirection => _dangerDirection;
    public ObstacleAvoidance(AvoidanceData aiData) : base(aiData)
    {
    }

    public override void SetDirection(float detectionRadius, float distanceThreshold, Vector3 selfPosition)
    {
        if(_aiData.Obstacles == null || _aiData.Obstacles.Count == 0)
        {
            ResetDangerDirection();
            return;
        }
    
        foreach(Collider2D collider in _aiData.Obstacles)
        {
            if(collider.CompareTag(PlayerTag))
            {
                ResetDangerDirection();
                break;
            }

            Vector2 directionToObstacle = collider.ClosestPoint(selfPosition) - (Vector2)selfPosition;
            float distanceToObstacle = directionToObstacle.magnitude;

            float weight = distanceToObstacle <= distanceThreshold ? 1 : (detectionRadius - distanceToObstacle) / detectionRadius;
            Vector2 directionToObstacleNormalized = directionToObstacle.normalized;

            ResetDangerDirection();

            for(int i = 0; i < Directions.NormalizedDirections.Length; i++)
            {   
                Vector2 direction = Directions.NormalizedDirections[i];
                float result = Vector2.Dot(directionToObstacleNormalized, direction);
                if(result <= 0)continue;

                float resultingDangerValue = result * weight;

                _dangerDirection += direction * (resultingDangerValue * 1.2f);
                

            }
        }

        void ResetDangerDirection()
        {
            _dangerDirection = Vector2.zero;
        }
    }


}
