using UnityEngine;

public interface IKnockback
{
    public Vector3 KnockbackEmitter {set;}
    public float EmitterForce {set;}
    public void KnockbackLogic();
}