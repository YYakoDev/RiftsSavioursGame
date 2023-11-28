using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/TripleComboMeleeWeapon")]
public class TripleComboMeleeWeapon : MeleeWeapon
{
    [SerializeField]float _attackSpeed = 1; // this value should change when evaluating stats
    const int ComboAttacks = 3;
    Animator _weaponInstanceAnimator;
    Timer _waitForInputTimer;
    Timer _waitForRemainingDuration;
    const float TimeOffset = 0.12f;
    float _comboCooldown;
    bool _checkForComboInput = false;
    bool _inputDetected = false;
    int _currentComboIndex = 0;
    private string[] _animationNames = new string[ComboAttacks]
    {
        "Attack",
        "Attack2",
        "Attack3"
    };
    private int[] _animationsHash = new int[ComboAttacks];
    private float[] _atkDurations = new float[ComboAttacks];

    public override void Initialize(WeaponManager weaponManager, Transform prefabTransform)
    {
        base.Initialize(weaponManager, prefabTransform);
        _weaponInstanceAnimator = prefabTransform.GetComponent<Animator>();
        _weaponInstanceAnimator.speed = _attackSpeed;

        for (int i = 0; i < ComboAttacks; i++)
        {
            _animationsHash[i] = Animator.StringToHash(_animationNames[i]);
            _atkDurations[i] = GetAnimationDuration(_animationNames[i]);
        }
        int soundsCount = _weaponSounds.Length;
        if(soundsCount < ComboAttacks)
        {
            Array.Resize<AudioClip>(ref _weaponSounds, ComboAttacks);
            int soundsLeft = ComboAttacks - soundsCount;
            for (int i = soundsCount; i < ComboAttacks; i++)
            {
                _weaponSounds[i] = _weaponSounds[0];
            }
        }

        _currentComboIndex = 0;
        
        _waitForInputTimer = new(_atkDurations[0] + TimeOffset, false, false);
        _waitForInputTimer.onStart += StartInputCheck;
        _waitForInputTimer.onEnd += StopInputCheck;
        _waitForInputTimer.Stop();

        _waitForRemainingDuration = new(1f, false, false);
        _waitForRemainingDuration.onEnd += SetNextAttack;
        _waitForRemainingDuration.Stop();

        OnComboIndexChange(_currentComboIndex);

        _checkForComboInput = false;
        _inputDetected = false;
    }
    public override void InputLogic()
    {
        _atkExecutionTimer.UpdateTime();
        if(Time.time < _nextAttackTime) return;

        if(_inputDetected)
        {
            _waitForRemainingDuration.UpdateTime();
            return;
        }else if(_checkForComboInput)
        {
            if(Input.GetButtonDown("Attack") && !_inputDetected)
            {
                _inputDetected = true;
                _weaponInstanceAnimator.speed = _attackSpeed * 1.25f;
                _attackDuration /= 1.25f;
                _waitForRemainingDuration.ChangeTime(_waitForInputTimer.CurrentTime / 1.25f - (TimeOffset / 5f));
                _waitForRemainingDuration.Start();
                return;
            }
            _waitForInputTimer.UpdateTime();
            return;
        }
        _waitForInputTimer.UpdateTime();
        if(Input.GetButtonDown("Attack"))
        {
            Attack();
        }
    }
    protected override void Attack()
    {
        InvokeOnAttack();
        //Debug.Log("<b> Attacking! </b>");
        _waitForInputTimer.ResetTime();
        _waitForInputTimer.Start();
        Collider2D[] hittedEnemies =  Physics2D.OverlapCircleAll(_weaponPrefabTransform.position, _attackRange, _enemyLayer);
        if(hittedEnemies.Length == 0) return;

        _hittedEnemiesGO.Clear();
        for(int i = 0; i < hittedEnemies.Length; i++)
        {
            if(_hittedEnemiesGO.Contains(hittedEnemies[i].gameObject)) continue;
            _hittedEnemiesGO.Add(hittedEnemies[i].gameObject);
        }
        _atkExecutionTimer.ResetTime();
        _atkExecutionTimer.Start();
    }

    void SetNextAttack()
    {
        Debug.Log("Setting Next Attack");
        _currentComboIndex++;
        OnComboIndexChange(_currentComboIndex);
        _inputDetected = false;
        _waitForRemainingDuration.Stop();
        Attack();
        if(_currentComboIndex >= 2)
        {
            ResetCombo();
            return;
        }
    }

    void ResetCombo()
    {
        //Debug.Log("<b> Resetting Combo </b>");
        _comboCooldown = TimeOffset + (_attackCooldown / _attackSpeed) * ((_currentComboIndex + 1) / 3);
        _nextAttackTime = Time.time + _comboCooldown; //here you should check the combo index at which the attacked stopped and change the cooldown based on that
        _currentComboIndex = 0;
        OnComboIndexChange(_currentComboIndex);
        _inputDetected = false;
        _checkForComboInput = false;
        _waitForInputTimer.Stop();

        _weaponInstanceAnimator.speed = _attackSpeed;
    }

    void OnComboIndexChange(int newIndex)
    {
        _attackDuration = _atkDurations[_currentComboIndex];
        _currentAnim = _animationsHash[_currentComboIndex];
        _attackSound = _weaponSounds[_currentComboIndex];
        _waitForInputTimer.ChangeTime(_attackDuration + TimeOffset);
    }
    void StartInputCheck()
    {
        //Debug.Log("Starting Input Check");
        _checkForComboInput = true;
    }
    void StopInputCheck()
    {
        _checkForComboInput = false;
        _waitForInputTimer.Stop();
        ResetCombo();

    }

    protected override void EvaluateStats(SOPlayerAttackStats attackStats)
    {
        base.EvaluateStats(attackStats);
    }

    public override void DrawGizmos()
    {
        base.DrawGizmos();
    }
}
