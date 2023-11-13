using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dropper))]
[RequireComponent(typeof(WhiteBlinkEffect))]
public class EnemyHealthManager : MonoBehaviour, IDamageable
{
    [SerializeField]EnemyBrain _brain;
    Dropper _dropper;
    WhiteBlinkEffect _blinkFX;
    private int _health;
    [SerializeField]private float _deathDuration = 0.35f;
    WaitForSeconds _deathDurationWait;
    public event Action onDeath;

    //SFX STUFF
    [SerializeField]AudioClip _onHitSFX, _onDeathSFX;

    private void Awake()
    {   
        gameObject.CheckComponent<EnemyBrain>(ref _brain);
        gameObject.CheckComponent<Dropper>(ref _dropper);
        gameObject.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);

        _deathDurationWait = new(_deathDuration);
    }

    private void OnEnable() 
    {
        StopAllCoroutines();  
        SetHealth();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHealth();
    }

    void SetHealth()
    {
        if(_brain == null)return;
        _health = _brain.Stats.MaxHealth;
    }
    public void TakeDamage(int damage)
    {
        if(_health <= 0)return;
        
       _health -= damage;
       _blinkFX.Play();
       _brain.Audio.PlayWithVaryingPitch(_onHitSFX);
       if(_health <= 0)
       {
            _blinkFX.Stop();
            Die();
       }
    }
    public void Die()
    {
        StartCoroutine(DeactivateObject());
        _brain.Audio.PlayWithVaryingPitch(_onDeathSFX);
        _brain.Animation.PlayDeath();
        _dropper.Drop();
        onDeath?.Invoke();
    }

    IEnumerator DeactivateObject()
    {
        yield return _deathDurationWait;
        gameObject.SetActive(false);
    }


}
