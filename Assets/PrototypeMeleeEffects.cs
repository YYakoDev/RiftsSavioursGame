using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeMeleeEffects : MonoBehaviour
{
    [SerializeField] PlayerMovement _movement;
    [SerializeField] WeaponAiming _aiming;
    [SerializeField] WeaponManager _weaponManager;
    [SerializeField] PlayerAnimationController _playerAnimator;
    KeyInput _attackButton, _dashButton;
    bool _holding;
    Timer _atkTimer, _holdTimer;
    AnimationCurve _aimCurve;
    float _holdDuration = 0.7f, _elapsedTime, _initialAimSpeed; //get the heavyatktime threshold for the hold duration

    private void Awake() {
        _atkTimer = new(0.4f);
        _atkTimer.Stop();
        _atkTimer.onEnd += ReturnPlayerFreedom;
        //_holdTimer = new(0.2f); //this should be more than half the medium attack threshold
        _holdTimer = new(0.7f + 0.05f); //this should be more than half the medium attack threshold
        _holdTimer.Stop();
        _holdTimer.onEnd += ReleaseHeavyAttack;
    }

    // Start is called before the first frame update
    void Start()
    {
        _aimCurve = TweenCurveLibrary.EaseInCirc;
        _initialAimSpeed = _aiming.AimSmoothing;
        _dashButton = YYInputManager.GetKey(KeyInputTypes.Dash);
        _attackButton = YYInputManager.GetKey(KeyInputTypes.Attack);
        _attackButton.OnKeyPressed += Hold;
        _attackButton.OnKeyUp += Release;
    }

    // Update is called once per frame
    void Update()
    {
        _atkTimer.UpdateTime();
        _holdTimer.UpdateTime();
        if(_holding)
        {
            //_aiming.LockFlip(Time.deltaTime * 2f);
            _elapsedTime += Time.deltaTime;
            var percent = _elapsedTime / _holdDuration;
            if(percent <= 1.01f)
            {
                _aiming.AimSmoothing = Mathf.Lerp(_initialAimSpeed, _initialAimSpeed / 4f, _aimCurve.Evaluate(percent));
            }
        }else if(_movement.Movement.sqrMagnitude > 0.1f)
        {
            _aiming.AimSmoothing = _initialAimSpeed / 1.65f;
        }else
        {
            if(_aiming.AimSmoothing < _initialAimSpeed)
            {
                _aiming.AimSmoothing = Mathf.Lerp(_aiming.AimSmoothing, _initialAimSpeed, Time.deltaTime * 5f);
            }
        }

        
    }

    void Hold()
    {
        _holdTimer.Start();
        _holding = true;
        //you could disable the dash a few miliseconds after the attack holding, so the player can correct his course

    }

    void ReleaseHeavyAttack()
    {
        Release();
        //do more stuff?

        //_playerAnimator.PlayStated(PlayerAnimationsNames.ChargingAttack);
        //_playerAnimator.LockAnimator();
    }

    void Release() //the release is now caused not only when button is up but when the time for heavy atk has been reached and not a second longer
    {
        if(!_holding) return;
        _holding = false;
        _holdTimer.Stop();
        //_aiming.LockFlip(0f);
        //_playerAnimator.UnlockAnimator();
        _elapsedTime = 0f;
        //_atkTimer.ChangeTime(_weaponManager.) //get the atk duration of the current weapon
        _atkTimer.Start();
        _dashButton.SetActive(false);
    }

    void ReturnPlayerFreedom()
    {
        _dashButton.SetActive(true);
    }

    private void OnDestroy() {
        _attackButton.OnKeyPressed -= Hold;
        _attackButton.OnKeyUp -= Release;
        _atkTimer.onEnd -= ReturnPlayerFreedom;
        _holdTimer.onEnd -= ReleaseHeavyAttack;
    }
}
