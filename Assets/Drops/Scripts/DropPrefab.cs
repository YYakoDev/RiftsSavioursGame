using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class DropPrefab : MonoBehaviour
{
    SpriteRenderer _renderer;
    Drop _drop;

    Coroutine _pickUpRoutine;

    PickUpsController _pickUpsController;
    Transform _target;
    [SerializeField]float _pickupVelocity;

    private void Awake() {
        gameObject.CheckComponent<SpriteRenderer>(ref _renderer);
        //gameObject.CheckComponent<Animator>(ref _animator);
    }

    public void Initialize(Drop drop)
    {
        _drop = drop;
        _renderer.sprite = _drop.Sprite;
        //_animator.runtimeAnimatorController = _drop.AnimatorOverride;
    }

    private void FixedUpdate() {
        if(_target == null)return;
        if(_pickUpRoutine != null)return;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _pickupVelocity * Time.fixedDeltaTime);
        if(Vector2.Distance(transform.position, _target.position) < 0.4f)
        {
            _pickUpRoutine = StartCoroutine(DestroyAfterTime(0.1f));
        }
    }

    public void PickUp(PickUpsController pickUpsController)
    {
        _pickUpsController = pickUpsController;
        _target = _pickUpsController.transform;
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _drop.OnPickUp(_pickUpsController);
        Destroy(gameObject,0.01f);
        //_pickUpRoutine = null;
    }
}
