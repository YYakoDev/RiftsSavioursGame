using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WhiteBlinkEffect))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Dropper))]
public class Resource : MonoBehaviour, IResources, IComparable, IMaskeable
{
    Animator _animator;
    SpriteRenderer _renderer;
    Collider2D _coll;
    AudioSource _audio;
    Dropper _dropper;
    WhiteBlinkEffect _blinkFX;

    [Header("Stats")]
    [SerializeField]private ResourcesTypes type;
    [SerializeField]int _maxHealth = 3;
    [SerializeField]float _disappearingTime = 1f;
    int _health;

    [Header("AudioStuff")]
    [SerializeField]AudioClip _hitSFX;
    [SerializeField]AudioClip _breakSFX;

    [SerializeField]bool _activeAnimatorOnStart = false;

    //properties

    public ResourcesTypes ResourceType => type;
    public Vector3 ResourcePosition => transform.position;
    public SpriteRenderer MaskeableRenderer => _renderer;


    private void Awake() {
        GameObject thisGo = gameObject;
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        thisGo.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);
        thisGo.CheckComponent<Collider2D>(ref _coll);
        thisGo.CheckComponent<AudioSource>(ref _audio); 
        thisGo.CheckComponent<Dropper>(ref _dropper);

        _animator.enabled = _activeAnimatorOnStart;
        _audio.playOnAwake = false;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
    }
    public void Interact(int damage)
    {
        if(_health <=0)return;

        _health -= damage;
        _blinkFX.Play();

        if(_hitSFX != null)
        {
            _audio.Stop();
            _audio.PlayWithVaryingPitch(_hitSFX);
        }

        if(_health <= 0)
        {
            _blinkFX.Stop();
            DestroyResource();
        }
    }
    public void DestroyResource()
    {
        _coll.enabled = false;

        _animator.enabled = true;
        _animator?.Play("Death");   

        _dropper.Drop();

        Destroy(gameObject, _disappearingTime);
    }

    public int CompareTo(object obj)
    {
        Resource resource = obj as Resource;
        return _health.CompareTo(resource._health);
    }
}
