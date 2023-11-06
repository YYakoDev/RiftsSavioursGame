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
    Timer _timer;
    bool _moveToTarget = false;
    bool _pickedUp = false;


    private void Awake() {
        gameObject.CheckComponent<SpriteRenderer>(ref _renderer);
        _timer = new(0.3f);
        //gameObject.CheckComponent<Animator>(ref _animator);
    }

    private void OnEnable() {
        _timer.onTimerStart += MoveFromTarget;
        _timer.onReset += MoveToTarget;
        _pickedUp = false;
    }

    public void Initialize(Drop drop)
    {
        _drop = drop;
        _renderer.sprite = _drop.Sprite;
        //_animator.runtimeAnimatorController = _drop.AnimatorOverride;
    }

    private void Update() {
        if(_target == null)return;

        _timer.TimerUpdate();
    }

    private void FixedUpdate() {
        if(_target == null)return;
        if(_pickedUp)return;


        //when the drop is picked up the fixed update starts working
        //execute the coroutine here and handle all the logic inside there
        //you may need to cut and paste the distance check to the coroutine
        //_pickUpRoutine = StartCoroutine(DestroyAfterTime(_target.position));


        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = _target.position;
        Vector3 directionFromTarget = currentPosition - targetPosition;
        directionFromTarget.Normalize();

        if(_moveToTarget) transform.position = Vector3.MoveTowards(currentPosition, targetPosition, _pickupVelocity *2.3f * Time.fixedDeltaTime);
        else transform.position = Vector3.MoveTowards(currentPosition, currentPosition + directionFromTarget, _pickupVelocity * Time.fixedDeltaTime);

        if(Vector2.Distance(currentPosition, targetPosition) < 0.2f)
        {
            PickUpAndDestroy();
        }
    }

    
    void MoveToTarget()
    {
        _timer.SetTimerActive(false);
        _moveToTarget = true;
    }

    void MoveFromTarget()
    {
        _moveToTarget = false;
    }


    public void PickUp(PickUpsController pickUpsController)
    {
        _pickUpsController = pickUpsController;
        _target = _pickUpsController.transform;
    }

    /*IEnumerator DestroyAfterTime(Vector3 targetPos)
    {
        Vector3 currentPosition = transform.position;
        Vector3 positionFromTarget = currentPosition - targetPos;
        positionFromTarget.Normalize();
        float timeGoingAway = 0.2f;
        while(timeGoingAway >= 0)
        {
            Debug.Log("Going Away");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + positionFromTarget, _pickupVelocity * 2 * Time.fixedDeltaTime);
            timeGoingAway -= Time.fixedDeltaTime;
            yield return null;
        }
        while(Vector2.Distance(transform.position, targetPos) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, _pickupVelocity * 2 * Time.fixedDeltaTime);
            yield return null;
        }
        Debug.Log("Destroying pickup");
        _drop.OnPickUp(_pickUpsController);
        Destroy(gameObject,0.01f);
        //_pickUpRoutine = null;
    }*/

    void PickUpAndDestroy()
    {
        _pickedUp = true;
        _drop.OnPickUp(_pickUpsController);
        Destroy(gameObject,0.01f);
    }

    private void OnDisable() {
        _timer.onTimerStart -= MoveFromTarget;
        _timer.onReset -= MoveToTarget;
    }
}
