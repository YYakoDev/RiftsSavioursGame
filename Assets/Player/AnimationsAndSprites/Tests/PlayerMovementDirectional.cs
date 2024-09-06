using System;
using UnityEngine;

public class PlayerMovementDirectional : MonoBehaviour
{
    /*[SerializeField]Rigidbody2D _rigidbody;
    [SerializeField]Animator _animator;
    Vector2 _movement;
    [SerializeField]float _speed;
    float _lockedFacing = 0f;
    [SerializeField]Transform _weaponPrefab;
    SpriteRenderer _weaponSprite;
    public event Action<Vector2> OnMovement;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        YYInputManager.OnMovement += TryMovement;

    }
    private void Start() {
        _weaponSprite = _weaponPrefab.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(_lockedFacing > 0) _lockedFacing -= Time.deltaTime;
    }

    private void TryMovement(Vector2 movementInput)
    {
        _movement = movementInput;
        _movement.Normalize();
    }

    private void FixedUpdate() 
    {
        if(_movement.sqrMagnitude > 0.1f) Move();
        else PlayIddleAnimation();
    }

    void Move()
    {
        Vector2 direction = _movement * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition((Vector2)transform.position + direction);
        OnMovement?.Invoke(_movement);
        PlayMovementAnimation(_movement);
        if(_movement.y > 0.1f) _weaponSprite.sortingOrder = -1;
        else _weaponSprite.sortingOrder = 10;
    }

    void FlipOnAttack()
    {
        Vector2 directionToWeapon = _weaponPrefab.position - transform.position;
        directionToWeapon.Normalize();
        PlayMovementAnimation(directionToWeapon, 0.3f);
        if(directionToWeapon.y > 0.1f) _weaponSprite.sortingOrder = -1;
        else _weaponSprite.sortingOrder = 10;
    }

    void PlayMovementAnimation(Vector2 dir, float lockTime = 0f)
    {
        if(_lockedFacing > 0) return;
        _animator.SetFloat("MoveX", dir.x);
        _animator.SetFloat("MoveY", dir.y);
        _animator.Play("Move");

        _lockedFacing = lockTime;
    }

    void PlayIddleAnimation()
    {
        _animator.Play("Iddle");
    }

    private void OnDestroy() {
        YYInputManager.OnMovement -= TryMovement;
   }*/
}
