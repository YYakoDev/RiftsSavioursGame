using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "NormalMovement")]
public class SOEnemyLandMovementBehaviour : SOEnemyMovementBehaviour
{
    //here replace the direction with some avoidance data and all that stuff
    //make a generic C# class and do the avoidance logic there with fields from here like layer mask, detection radius and all that stuff
    public override void Initialize(EnemyBrain brain)
    {
        base.Initialize(brain);
        //construct the avoidance class here
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
