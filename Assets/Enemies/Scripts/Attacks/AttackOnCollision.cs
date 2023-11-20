using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnCollision : MonoBehaviour, IEnemyAttack
{
    [SerializeField]EnemyBrain _brain;
    [Range(0,1), SerializeField]float _knockbackForce = 0.2f;

    //properties

    int damage => _brain.Stats.Damage;

    private void Awake()
    {
        if(_brain == null) _brain = GetComponentInParent<EnemyBrain>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Attack(damageable);
            if(other.gameObject.TryGetComponent<IKnockback>(out IKnockback knockbackable))
            {
                //knockbackable.KnockBackLogic.ApplyForce(transform.position, _knockbackForce);
            }
        }
    }

    public void Attack(IDamageable damageable)
    {
        damageable.TakeDamage(damage);
    }
    
}
