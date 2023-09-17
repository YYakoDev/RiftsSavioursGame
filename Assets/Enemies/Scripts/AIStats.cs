using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]private bool _randomizeStats = false;
    [SerializeField]private float _speed = 3f;
    [SerializeField]private int _maxHealth = 5;
    [SerializeField]private int _damage = 1;

    //properties
    public float Speed => _speed;
    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
   

    private void Start() 
    {
        if(_randomizeStats)
        {
            _speed += Random.Range(-0.3f, 0.45f);
            //_maxHealth += Random.Range(0, 3);
        }
    }
}
