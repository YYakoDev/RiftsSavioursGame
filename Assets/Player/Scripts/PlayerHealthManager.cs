using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WhiteBlinkEffect))]
public class PlayerHealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]PlayerManager _player;
    WhiteBlinkEffect _blinkFX;
    

    private void Awake()
    {   
        gameObject.CheckComponent<PlayerManager>(ref _player);
        gameObject.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetHealth()
    {
        _player.Stats.CurrentHealth = _player.Stats.MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(_player.Stats.CurrentHealth <= 0)
        {
            return;
        }
        _player.Stats.CurrentHealth -= damage;
        _blinkFX.Play();
        if(_player.Stats.CurrentHealth <= 0)
        {
            _blinkFX.Stop();
            Die();
        }
    }
    public void Die()
    {
        //Debug.Log("Player is dead lmao");
    }

}
