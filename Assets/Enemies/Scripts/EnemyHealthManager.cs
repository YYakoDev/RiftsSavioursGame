using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dropper))]
[RequireComponent(typeof(WhiteBlinkEffect))]
public class EnemyHealthManager : MonoBehaviour, IDamageable, ITargetPositionProvider
{
    EnemyBrain _brain;
    SOEnemyBehaviour _deathBehaviour;
    Dropper _dropper;
    WhiteBlinkEffect _blinkFX;
    private int _health;
    private float _deathDuration = 1.5f;
    WaitForSeconds _deathDurationWait;
    public event Action onDeath;
    Transform _player;

    //SFX STUFF
    public Transform TargetTransform { get => _player; set => _player = value; }

    private void Awake()
    {   
        gameObject.CheckComponent<EnemyBrain>(ref _brain);
        gameObject.CheckComponent<Dropper>(ref _dropper);
        gameObject.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);

        _deathDurationWait = new(_deathDuration);
        
    }
    public void Init(EnemyBrain brain, SOEnemyBehaviour deathBehaviour)
    {
        _brain = brain;
        _player = brain.TargetTransform;
        _deathBehaviour = deathBehaviour;
        SetHealth();
    }

    private void OnEnable() 
    {
        StopAllCoroutines();  
        SetHealth();
    }
    void SetHealth()
    {
        if(_brain == null || _brain.Stats == null)return;
        _health = _brain.Stats.MaxHealth;
    }
    public void TakeDamage(int damage)
    {
        if(_health <= 0)return;
        
        _health -= damage;
        _blinkFX.Play();
        SpawnBlood();
        _brain.PlaySound(_brain.GetOnHitSfx());
        if(_health <= 0)
        {
            //_blinkFX.Stop();
            Die();
        }
    }

    void SpawnBlood()
    {
        if(_brain.BloodFX == null) return;
        BloodSplatterFX blood = Instantiate(_brain.BloodFX, transform.position, Quaternion.identity);
        blood.Flip(_player.position);
    }

    public void Die()
    {
        StartCoroutine(DeactivateObject());
        _brain.PlaySound(_brain.GetOnDeathSfx());
        _brain.Animation.PlayDeath(_deathDuration);
        _dropper.Drop();
        _deathBehaviour?.Action();
        onDeath?.Invoke();
    }

    IEnumerator DeactivateObject()
    {
        yield return _deathDurationWait;
        gameObject.SetActive(false);
    }


}
