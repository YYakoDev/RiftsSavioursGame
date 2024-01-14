using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovement
{
    public EnemyBaseMovement MovementLogic { get; set; }
}
