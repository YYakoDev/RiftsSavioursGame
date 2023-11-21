using UnityEngine;

public class EnemyAttackLogic
{
    Transform _transform;
    int _damage;
    float _knockbackForce;

    public EnemyAttackLogic(Transform ownTransform, int damage, float knockbackForce)
    {
        _transform = ownTransform;
        _damage = damage;
        _knockbackForce = knockbackForce;
    }

    public void Attack(GameObject objective)
    {
        if(objective.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
            if(objective.TryGetComponent<IKnockback>(out IKnockback knockbackable))
            {
                knockbackable.KnockbackLogic.SetKnockbackData(_transform.position, _knockbackForce);
            }
        }
    }
}
