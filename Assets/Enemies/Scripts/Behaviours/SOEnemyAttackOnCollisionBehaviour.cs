using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath +"AttackOnCollision")]
public class SOEnemyAttackOnCollisionBehaviour : SOEnemyAttackBehaviour
{
    public override void Initialize(EnemyBrain brain)
    {
        base.Initialize(brain);
        _brain.HealthManager.onDeath += UnsubscribeEvents;
        _brain.Collisions.OnCollisionTriggered += Attack;
    }
    void Attack(Transform target)
    {
        _brain.AttackLogic.Attack(target.gameObject);
    }

    void UnsubscribeEvents()
    {
        _brain.HealthManager.onDeath -= UnsubscribeEvents;
        _brain.Collisions.OnCollisionTriggered -= Attack;
    }

    ~SOEnemyAttackOnCollisionBehaviour()
    {
        UnsubscribeEvents();
    }
}
