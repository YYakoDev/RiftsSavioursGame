using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnTriggerCollision : MonoBehaviour
{
    [SerializeField]EnemyBrain _brain;
    [Range(0,2), SerializeField]float _knockbackForce = 1f;
    EnemyAttackLogic _attackLogic;
    //properties

    int damage => _brain.Stats.Damage;

    private void Awake()
    {
        if(_brain == null) _brain = GetComponentInParent<EnemyBrain>();

        //with this method you cant actually update the knockback force or the damage of the enemies through the course of the game
        _attackLogic = new(transform, damage, _knockbackForce); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _attackLogic.Attack(other.gameObject);
    }
}
