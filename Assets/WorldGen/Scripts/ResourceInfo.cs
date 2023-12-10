using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceInfo
{
    [SerializeField]float _scaleFactor = 1;
    [Header("Sprite Stuff")]
    [SerializeField]Sprite[] _sprites;
    [SerializeField]float _spriteOrderOffset;

    [Header("Health")]
    [SerializeField]int _maxHealth;
    [SerializeField]float _dissapearingTime;

    [Header("Resource Properties")]
    [SerializeField]ResourcesTypes _type;
    [SerializeField]Drop _resourceDrop;

    [Header("Collider Info")]
    [SerializeField] bool _isTrigger;
    [SerializeField] Vector2 _posOffset;
    [SerializeField] float _radius;

    [Header("Audio")]
    [SerializeField]AudioClip[] _hitSFXs;
    [SerializeField] AudioClip _breakSFX;

    [Header("Animations")]
    [SerializeField] AnimatorOverrideController _animOverrider;
    [SerializeField] bool _activeAnimatorOnStart;

    //public accessors

    public float ScaleFactor => _scaleFactor;

    public Sprite Sprite => _sprites[Random.Range(0, _sprites.Length)];
    public float SpriteOrderOffset => _spriteOrderOffset;

    public ResourcesTypes Type => _type;
    public Drop ResourceDrop => _resourceDrop;

    public int MaxHealth => _maxHealth;
    public float DissapearingTime => _dissapearingTime;

    public bool IsTrigger => _isTrigger;
    public Vector2 ColliderPosOffset => _posOffset;
    public float Radius => _radius;

    public AudioClip[] HitSFXs => _hitSFXs;
    public AudioClip BreakSFX => _breakSFX;

    public AnimatorOverrideController AnimOverrider => _animOverrider;
    public bool ActiveAnimatorOnStart => _activeAnimatorOnStart;
}
