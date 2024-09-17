using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class DropPrefab : MonoBehaviour
{
    [SerializeField]SpriteRenderer _renderer;
    [SerializeField] Rigidbody2D _rb;
    PickUpsController _pickUpsController;
    Light2D _light;
    Drop _drop;
    Transform _target;
    [SerializeField]TrailRenderer _trail;
    [SerializeField] CurveTypes _curveType = CurveTypes.EaseInCirc;
    [SerializeField]float _moveToTime = 0.2f;
    Timer _timer;
    bool _pickedUp = false;
    float _elapsedMoveToTime;
    Vector2 _startPosition, _endPosition, _awayPosition;
    AnimationCurve _curve;


    //SFX
    [Header("Sounds")]
    [SerializeField]AudioClip _pickupSoundDefault;

    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.CheckComponent<SpriteRenderer>(ref _renderer);
        thisGO.CheckComponent<Rigidbody2D>(ref _rb);
        thisGO.CheckComponent<Light2D>(ref _light);
        _timer = new(0.3f);
        _timer.Stop();
        _timer.onStart += MoveFromTarget;
        _timer.onEnd += MoveToTarget;
    }

    private void OnEnable() {
        _pickedUp = false;
        _trail.emitting = false;
        //_elapsedAwayTime = 0f;
        _elapsedMoveToTime = 0f;
        _curve = TweenCurveLibrary.GetCurve(_curveType);
    }

    public void Initialize(Drop drop)
    {
        _drop = drop;
        _renderer.sprite = _drop.Sprite;
        _light.intensity = _drop.LightIntensity;
        _timer.Start();
        _startPosition = transform.position;
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
        var endPos = _target.position;
        //var awayPos = _startPosition - _endPosition;

        _elapsedMoveToTime += Time.fixedDeltaTime;
        var percent = _elapsedMoveToTime / _moveToTime;
        var dir = Vector2.Lerp(_startPosition, endPos, _curve.Evaluate(percent));
        _rb.MovePosition(dir);
        if(percent >= 1.01f)
        {
            PickUpAndDestroy();
        }
    }

    
    void MoveToTarget()
    {
        _trail.emitting = true;
        //_moveToTarget = true;
    }

    void MoveFromTarget()
    {
        //_moveToTarget = false;
    }


    public void PickUp(PickUpsController pickUpsController)
    {
        if(_target != null)return;
        _pickUpsController = pickUpsController;
        _target = _pickUpsController.transform;
        _endPosition = _target.position;
        _awayPosition = _startPosition - _endPosition * 1.5f;
        if(Vector3.Distance(_startPosition, _target.position) < 1f) _elapsedMoveToTime = _moveToTime / 2f;
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
