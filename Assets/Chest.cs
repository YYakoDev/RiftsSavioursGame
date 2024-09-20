using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Dropper))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(WhiteBlinkEffect))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
public class Chest : MonoBehaviour, IDamageable
{

    bool _tangible = false;
    SpriteRenderer _renderer;
    Animator _animator;
    Dropper _dropper;
    WhiteBlinkEffect _blinkFX;
    AudioSource _audio;
    [SerializeField] int _maxHealth = 10;
    int _currentHealth;
    [SerializeField] AudioClip _hitSfx, _deathSfx;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _dropper = GetComponent<Dropper>();
        _blinkFX = GetComponent<WhiteBlinkEffect>();
        _audio = GetComponent<AudioSource>();
        _currentHealth = _maxHealth;
    }

    private void Start() {
        _animator.Play("Closed");
    }
    public void MakeTangible()
    {
        _animator.Play("Closed");
        _renderer.color = Color.white;
        _tangible = true;
    }
    public void MakeIntangible()
    {
        Color color = Color.white;
        color.a = 0.5f;
        _renderer.color = color;
        _tangible = false;
        _animator.Play("Closed");
    }
    public void TakeDamage(int damage)
    {
        if(!_tangible) return;
        if(_currentHealth <= 0) return;
        _currentHealth -= damage;
        _blinkFX.Play();
        _audio.PlayWithVaryingPitch(_hitSfx);
        _animator.Play("Hit");
        if(_currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        _animator.Play("Open");
        _dropper.Drop();
        _blinkFX.Play(0.12f);
        _audio.PlayWithVaryingPitch(_deathSfx);
        MakeIntangible();
    }

    public void AddDropToChest(Drop drop)
    {
        _animator.Play("Shake");
        _currentHealth = _maxHealth;
        _dropper.AddDrop(drop);
    }
}
