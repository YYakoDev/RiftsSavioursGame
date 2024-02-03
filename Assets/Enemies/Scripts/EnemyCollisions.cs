using System;
using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    public event Action<Transform> OnCollisionTriggered;
    public void TriggerCollision(Transform emitter) => OnCollisionTriggered?.Invoke(emitter);
}
