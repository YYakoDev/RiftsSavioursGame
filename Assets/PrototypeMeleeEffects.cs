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
    Timer _atkTimer;
    AnimationCurve _aimCurve;
    float _holdDuration = 0.7f, _elapsedTime, _initialAimSpeed; //get the heavyatktime threshold for the hold duration

    private void Awake() {
        _atkTimer = new(0.25f);
        _atkTimer.Stop();
        _atkTimer.onEnd += ReturnPlayerFreedom;
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
        if(_holding)
        {
            _aiming.LockFlip(Time.deltaTime * 2f);
            _elapsedTime += Time.deltaTime;
            var percent = _elapsedTime / _holdDuration;
            if(percent <= 1.01f)
            {
                _aiming.AimSmoothing = Mathf.Lerp(_initialAimSpeed, 0f, _aimCurve.Evaluate(percent));
            }
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
        _holding = true;
        _playerAnimator.PlayStated(PlayerAnimationsNames.ChargingAttack);
        _playerAnimator.LockAnimator();
        //you could disable the dash a few miliseconds after the attack holding, so the player can correct his course

    }

    void Release()
    {
        _playerAnimator.UnlockAnimator();
        _playerAnimator.PlayStated(PlayerAnimationsNames.Attack, 0.4f);
        _elapsedTime = 0f;
        _holding = false;
        //_atkTimer.ChangeTime(_weaponManager.) //get the atk duration of the current weapon
        _atkTimer.ChangeTime(0.4f);
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
        _atkTimer.onEnd += ReturnPlayerFreedom;
    }
}
