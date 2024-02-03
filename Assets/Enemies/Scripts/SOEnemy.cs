using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/.SOENEMY")]
public class SOEnemy : ScriptableObject
{
	[Header("Sprite Data")]
    [SerializeField] Sprite _sprite;
    [SerializeField] AnimatorOverrideController _animator;

	[Header("Stats")]
    [SerializeField] AIStats _stats;

    [Header("Drops")]
    [SerializeField] Drop[] _drops;

    [Header("Collider Stuff")]
    [SerializeField] bool _isBoxTrigger = true;
    [SerializeField] Vector2 _boxColliderSize = Vector2.one/2f;
    [SerializeField] bool _isCircleTrigger = false;
    [SerializeField] Vector2 _circleColliderOffset;
    [SerializeField] float _circleColliderRadius = 0.3f;
    [SerializeField] bool _enableEnemyDetector = true;
    [SerializeField] float _enemyDetectionRadius = 0.7f;


    [Header("Behaviours")]
    [SerializeField] SOEnemyMovementBehaviour _movementBehaviour;
    [SerializeField] SOEnemyAttackBehaviour _attackBehaviour;
    [SerializeField] SOEnemyBehaviour _deathBehaviour;
	
	[Header("Fxs & Sfxs")]
    [SerializeField] BloodSplatterFX _bloodSplatter;
    [SerializeField] bool _hasShadow = true;
    [SerializeField] Vector3 _shadowPosition = Vector3.zero;
    [SerializeField] Vector3 _shadowSize = Vector3.one;
    [SerializeField] AudioClip[] _moveSFXs, _onHitSFXs, _attackSFXs, _deathSFXs;

    // properties
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController Animator => _animator;

    public AIStats Stats => _stats;

    public Drop[] Drops => _drops;

    public bool IsBoxTrigger => _isBoxTrigger;
    public Vector2 BoxColliderSize => _boxColliderSize;
    public bool IsCircleTrigger => _isCircleTrigger;
    public Vector2 CircleColliderOffset => _circleColliderOffset;
    public float CircleColliderRadius => _circleColliderRadius;
    public bool EnableEnemyDetector => _enableEnemyDetector;
    public float EnemyDetectionRadius => _enemyDetectionRadius;

    public SOEnemyMovementBehaviour MovementBehaviour => _movementBehaviour;
	public SOEnemyAttackBehaviour AttackBehaviour => _attackBehaviour;
	public SOEnemyBehaviour DeathBehaviour => _deathBehaviour;


    public BloodSplatterFX BloodFX => _bloodSplatter;

    public bool HasShadow => _hasShadow;
    public Vector3 ShadowOffset => _shadowPosition;
    public Vector3 ShadowSize => _shadowSize;

    public AudioClip[] MoveSFXs => _moveSFXs;
    public AudioClip[] OnHitSFXs => _onHitSFXs;
    public AudioClip[] AttackSFXs => _attackSFXs;
    public AudioClip[] DeathSFXs => _deathSFXs;
}