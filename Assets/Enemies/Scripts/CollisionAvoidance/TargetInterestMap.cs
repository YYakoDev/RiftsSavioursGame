using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInterestMap : ContextProcessor
{
    Transform _target;
    //float[] _interestMap = new float[8];
    Vector2 _interestDirection = Vector2.zero;
    
    //Properties
    //public float[] InterestMap => _interestMap;
    public Vector2 InterestDirection => _interestDirection;

    public TargetInterestMap(AvoidanceData aiData) : base(aiData)
    {
        _target = aiData.TargetTransform;
    }


    public override void SetDirection(float detectionRadius, float distanceThreshold, Vector3 selfPosition)
    {
        Vector3 directionToTarget = _target.position - selfPosition;
        Vector2 directionToTargetNormalized = directionToTarget.normalized;
        if(Vector3.Distance(selfPosition, selfPosition + (Vector3)directionToTargetNormalized) < 0.25f)
        {
            _interestDirection = directionToTargetNormalized;
            return;
        }

        _interestDirection = Vector2.zero;
        for(int i = 0; i < Directions.NormalizedDirections.Length; i++)
        {
            //_interestMap[i] = 0;
            Vector2 direction = Directions.NormalizedDirections[i];
            float result = Vector2.Dot(directionToTargetNormalized, direction);
            if(result >= 0)
            {   
                _interestDirection += direction * result;
            }
        
        }
    }
}

