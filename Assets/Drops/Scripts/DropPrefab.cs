using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class DropPrefab : MonoBehaviour
{
    SpriteRenderer _renderer;
    Drop _drop;
    PickUpsController _pickUpsController;
    Transform _target;
    [SerializeField]float _pickupVelocity;
    Timer _timer;
    bool _moveToTarget = false;
    bool _pickedUp = false;

    //SFX
    AudioSource _audio;
    [SerializeField]AudioClip _pickupSound;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        thisGO.CheckComponent<AudioSource>(ref _audio);

        _timer = new(0.3f);
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

        _timer.UpdateTime();
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
        _timer.Stop();
        _moveToTarget = true;
    }

    void MoveFromTarget()
    {
        _moveToTarget = false;
        _timer.Start();
        Debug.Log("Moving from target");
    }


    public void PickUp(PickUpsController pickUpsController)
    {
        if(_target != null)return;
        _pickUpsController = pickUpsController;
        _target = _pickUpsController.transform;
    }
    void PickUpAndDestroy()
    {
        _pickedUp = true;
        _audio.PlayWithVaryingPitch(_pickupSound);
        _drop.OnPickUp(_pickUpsController);
        Destroy(gameObject,_pickupSound.length);
    }

    private void OnDisable() {
        _timer.onTimerStart -= MoveFromTarget;
        _timer.onReset -= MoveToTarget;
    }
}
