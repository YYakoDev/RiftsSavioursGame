using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Resource : MonoBehaviour, IResources, IComparable, IMaskeable
{
    //References
    [SerializeField]Animator _animator;
    [SerializeField]SpriteRenderer _renderer;
    SortingOrderController _sortOrderController;
    [SerializeField]CircleCollider2D _coll;
    [SerializeField]AudioSource _audio;
    [SerializeField]Dropper _dropper;
    [SerializeField]WhiteBlinkEffect _blinkFX;

    bool _initialized = false;

    //Resource Properties
    private ResourcesTypes _type;
    int _maxHealth = 3, _currentHealth;
    float _disappearingTime = 1f;
    bool _isBroken = false;

    //Audio Stuff
    AudioClip[] _hitSFXs;
    AudioClip _breakSFX;
    AudioClip HitSFX => _hitSFXs[Random.Range(0, _hitSFXs.Length)];

    //Animation Stuff
    //bool _activeAnimatorOnStart = false;
    private readonly int BreakingAnim = Animator.StringToHash("Breaking");
    private readonly int OnHitAnim = Animator.StringToHash("OnHit");


    //properties

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public ResourcesTypes ResourceType => _type;
    public Vector3 ResourcePosition => _coll.bounds.center;
    public SpriteRenderer MaskeableRenderer => _renderer;
    public bool IsBroken => _isBroken;

    public void SetResourceInfo(ResourceInfo info)
    {
        if(!_initialized) Initialize();
        transform.localScale = Vector3.one * info.ScaleFactor;
        _renderer.sprite = null;
        _sortOrderController.ChangeOffset(info.SpriteOrderOffset);
        
        _maxHealth = info.MaxHealth; 
        _currentHealth = _maxHealth;
        _disappearingTime = info.DissapearingTime;

        _type = info.Type;
        _dropper.Clear();
        foreach(Drop drop in info.ResourceDrops)
        {
            _dropper.AddDrop(drop);
        }

        _coll.offset = info.ColliderPosOffset;
        _coll.radius = info.Radius;
        _coll.isTrigger = info.IsTrigger;

        _hitSFXs = info.HitSFXs;
        _breakSFX = info.BreakSFX;

        _animator.enabled = false;
        _animator.runtimeAnimatorController = null;
        _renderer.enabled = false;
        _renderer.sprite = info.Sprite;
        _renderer.enabled = true;
        _animator.runtimeAnimatorController = info.AnimOverrider;
        _animator.enabled = info.ActiveAnimatorOnStart;

    }

    void Initialize()
    {
        _initialized = true;

        GameObject thisGo = gameObject;
        thisGo.CheckComponent<WhiteBlinkEffect>(ref _blinkFX);
        thisGo.CheckComponent<CircleCollider2D>(ref _coll);
        thisGo.CheckComponent<AudioSource>(ref _audio); 
        thisGo.CheckComponent<Dropper>(ref _dropper);
        if(_renderer == null) _renderer = GetComponentInChildren<SpriteRenderer>();
        if(_animator == null) _animator = GetComponentInChildren<Animator>();
        _sortOrderController = new(transform, _renderer);

        _audio.playOnAwake = false;
        _animator.keepAnimatorControllerStateOnDisable = true;
    }

    private void Awake() {
        
        if(!_initialized) Initialize();

    }

    private void OnEnable()
    {
        if(!_initialized) return;
        if(_currentHealth > 0) _coll.enabled = true;
    }

    void Interact(int damage)
    {
        if(_currentHealth <= 0)
        {
            _coll.enabled = false;
            return;
        }

        _currentHealth -= damage;
        _blinkFX.Play();
        
        _animator.enabled = true;
        _animator.Play(OnHitAnim);
        if(HitSFX != null)
        {
            _audio.Stop();
            _audio.PlayWithVaryingPitch(HitSFX);
        }

        if(_currentHealth <= 0)
        {
            _blinkFX.Stop();
            DestroyResource();
        }
    }
    void DestroyResource()
    {
        _coll.enabled = false;

        _animator.enabled = true;
        _animator?.Play(BreakingAnim);   

        
        if(_breakSFX != null)
        {
            _audio.Stop();
            _audio.PlayWithVaryingPitch(_breakSFX);
        }

        _dropper.Drop();
        _isBroken = true;
        //Destroy(gameObject, _disappearingTime);
    }

    public int CompareTo(object obj)
    {
        Resource resource = obj as Resource;
        return _currentHealth.CompareTo(resource._currentHealth);
    }

    public void TakeDamage(int damage)
    {
        Interact(damage);
    }

    public void Die()
    {
        DestroyResource();
    }
}
