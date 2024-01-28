using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIStats)),
RequireComponent(typeof(EnemyHealthManager)),
RequireComponent(typeof(EnemyAnimations)),
RequireComponent(typeof(Rigidbody2D)),
RequireComponent(typeof(AudioSource))]
public class EnemyBrain : MonoBehaviour
{
    [SerializeField]EnemySignature _signature;

    [Header("References")]
    [SerializeField]Rigidbody2D _rb;
    [SerializeField]SpriteRenderer _renderer;
    [SerializeField]AudioSource _audio;
    Sprite _intialSprite = null;
    [SerializeField]AIStats _aiData;
    [SerializeField]EnemyHealthManager _healthManager;
    [SerializeField]IEnemyMovement _movement;
    EnemyBaseMovement _movementLogic;
    [SerializeField]EnemyAnimations _animation;
    Collider2D[] _colliders;

    //properties
    public EnemySignature Signature => _signature;
    public Rigidbody2D Rigidbody => _rb;
    public SpriteRenderer Renderer => _renderer;
    public AudioSource Audio => _audio;
    public AIStats Stats => _aiData;
    public EnemyHealthManager HealthManager => _healthManager;
    public EnemyAnimations Animation => _animation;

    private void Awake()
    {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Rigidbody2D>(ref _rb);
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        //saving the initialsprite in case the original sprite gets overriden by an empty animation or stuff like that
        if(_intialSprite == null) _intialSprite = _renderer.sprite; //this will need to be refactor if you delete the signature match part of the wave spawner
        //this works because the prefab foreach enemy is based on the type of enemy but it would stop working when you create a base enemyprefab class
        thisGO.CheckComponent<AudioSource>(ref _audio);
        thisGO.CheckComponent<AIStats>(ref _aiData);
        thisGO.CheckComponent<EnemyHealthManager>(ref _healthManager);
        thisGO.CheckComponent<EnemyAnimations>(ref _animation);
        _movementLogic = new(transform, this, 0.25f);
        _movement = GetComponent<IEnemyMovement>(); // this component reference will be lost if the movement class change
        // ie if you spawn a flying enemy it will not have the same movement class as a regular one you will need to get the component again
        // a way to bypass this is assigning the reference in the inspector
        SetMovementLogic();

        if(_colliders == null || _colliders.Length <= 0) _colliders = thisGO.GetComponents<Collider2D>();
        _healthManager.onDeath += DisableComponents;
        
    }

    public bool SignatureMatch(EnemySignature signature)
    {
        return signature == _signature;
    }
    
    public void SetMovementLogic()
    {
        _movement.MovementLogic = _movementLogic;
    }

    #region Components State and enable & disable events
    void SetComponentsToActive(bool state)
    {
        foreach(Collider2D coll in _colliders)
        {
            coll.enabled = state;
        }
        _healthManager.enabled = state;
        _movementLogic.Enabled = state;
    }
    void EnableComponents()
    {
        SetComponentsToActive(true);
    }
    void DisableComponents()
    {
        SetComponentsToActive(false);
    }
    private void OnEnable() {
        EnableComponents();
        _renderer.enabled = true;
    }
    private void OnDisable() {
        
        _renderer.sprite = _intialSprite;
        _renderer.enabled = false;
        _renderer.sprite = _intialSprite;
    }

    private void OnDestroy() {
        _healthManager.onDeath -= DisableComponents;
    }
    #endregion



   
    
}
