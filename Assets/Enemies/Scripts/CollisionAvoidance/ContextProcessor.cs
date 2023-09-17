using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class ContextProcessor
{
    protected AvoidanceData _aiData;
    //protected Transform _objectTransform;

    public ContextProcessor(AvoidanceData aiData)
    {
        _aiData = aiData;
        //_objectTransform = objectTransform;
        //_positionOffset = positionOffset;
    }
    
    public abstract void SetDirection(float detectionRadius, float distanceThreshold, Vector3 selfPosition);
    /*public virtual void ShowDebugLines(float[] scalarsArray, Color linesColor, Vector3 selfPosition)
    {
        if(scalarsArray == null)return;
        for(int i = 0; i < scalarsArray.Length; i++)
        {
            Vector2 direction = Directions.NormalizedDirections[i];
            Vector2 lineDirection = selfPosition + (Vector3)(direction * scalarsArray[i]);
            Debug.DrawLine(selfPosition, lineDirection, linesColor);
        }
    }*/
}
