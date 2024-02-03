using UnityEngine;

[System.Serializable]
public class AIStats
{
    [Header("Stats")]
    [SerializeField]private float _speed = 3f;
    [SerializeField]private float _stunDuration = 0.25f;
    [SerializeField, Range(0,100)] private int _stunResistance = 5, _knockbackResistance = 10;
    [SerializeField]private int _maxHealth = 10;
    [SerializeField]private int _damage = 1;
    [SerializeField, Range(0f, 4f)] private float _knockbackForce = 1.1f;

    //properties
    public float Speed => _speed;
    public float StunDuration => _stunDuration;
    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public float KnockbackForce => _knockbackForce;
    public int StunResistance => _stunResistance;
    public int KnockbackResistance => _knockbackResistance;
}
