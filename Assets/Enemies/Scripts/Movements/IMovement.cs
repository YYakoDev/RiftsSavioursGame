using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement
{
    int FacingDirection { get; }
    bool StopMoving { get; }
    public void Move();
    public void StopMovement();
}
