using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class DropPrefab : MonoBehaviour
{
    SpriteRenderer _renderer;
    PickUpsController _pickUpsController;
    Light2D _light;
    Drop _drop;
    Transform _target;
    [SerializeField]TrailRenderer _trail;
    [SerializeField]float _pickupVelocity;
    Timer _timer;
    bool _moveToTarget = false;
    bool _pickedUp = false;


    //SFX
    [Header("Sounds")]
    [SerializeField]AudioClip _pickupSoundDefault;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        thisGO.CheckComponent<Light2D>(ref _light);
        _timer = new(0.3f);
        _timer.Stop();
        _timer.onStart += MoveFromTarget;
        _timer.onEnd += MoveToTarget;
    }

    private void OnEnable() {
        _pickedUp = false;
        _trail.emitting = false;
    }

    public void Initialize(Drop drop)
    {
        _drop = drop;
        _renderer.sprite = _drop.Sprite;
        _light.intensity = _drop.LightIntensity;
        _timer.Start();
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
        _trail.emitting = true;
        _moveToTarget = true;
    }

    void MoveFromTarget()
    {
        _moveToTarget = false;
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

        _pickUpsController.PlayAudioClip(GetPickUpSound());

        _drop.OnPickUp(_pickUpsController);
        gameObject.SetActive(false);
    }

    AudioClip GetPickUpSound()
    {
        var pickupSounds = _drop.PickUpSounds;
        if(pickupSounds == null || pickupSounds.Length == 0) return _pickupSoundDefault;
        else return pickupSounds[Random.Range(0, pickupSounds.Length)];
    }

    private void OnDisable() {
        _target = null;
    }

    private void OnDestroy() 
    {
        _timer.onStart -= MoveFromTarget;
        _timer.onEnd -= MoveToTarget;
    }
}
