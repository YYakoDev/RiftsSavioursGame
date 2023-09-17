using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIStats)),
RequireComponent(typeof(EnemyHealthManager)),
RequireComponent(typeof(EnemyAnimations)),
RequireComponent(typeof(Rigidbody2D))]
public class EnemyBrain : MonoBehaviour
{
    [SerializeField]EnemySignature _signature;

    [Header("References")]
    [SerializeField]Rigidbody2D _rb;
    [SerializeField]SpriteRenderer _renderer;
    Sprite _intialSprite = null;
    [SerializeField]AIStats _aiData;
    [SerializeField]EnemyHealthManager _healthManager;
    [SerializeField]EnemyAnimations _animation;
    [SerializeField]Collider2D[] _colliders;

    //properties
    public EnemySignature Signature => _signature;
    public Rigidbody2D Rigidbody => _rb;
    public SpriteRenderer Renderer => _renderer;
    public AIStats Stats => _aiData;
    public EnemyHealthManager HealthManager => _healthManager;
    public EnemyAnimations Animation => _animation;

    private void Awake()
    {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<Rigidbody2D>(ref _rb);
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        //saving the initialsprite in case the original sprite gets overriden by an empty animation or stuff like that
        if(_intialSprite == null) _intialSprite = _renderer.sprite;

        thisGO.CheckComponent<AIStats>(ref _aiData);
        thisGO.CheckComponent<EnemyHealthManager>(ref _healthManager);
        thisGO.CheckComponent<EnemyAnimations>(ref _animation);
        if(_colliders == null || _colliders.Length <= 0) _colliders = thisGO.GetComponents<Collider2D>();
        
    }

    public bool SignatureMatch(EnemySignature signature)
    {
        return signature == _signature;
    }

    #region Components State and enable & disable events
    void SetComponentsToActive(bool state)
    {
        foreach(Collider2D coll in _colliders)
        {
            coll.enabled = state;
        }
        _healthManager.enabled = state;
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
        _healthManager.onDeath += DisableComponents;
    }
    private void OnDisable() {
        _healthManager.onDeath -= DisableComponents;
        _renderer.enabled = false;
        _renderer.sprite = _intialSprite;
    }
    #endregion



   
    
}
