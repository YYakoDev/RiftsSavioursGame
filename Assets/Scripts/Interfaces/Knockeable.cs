using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Knockbackeable
{
    Transform _ownTransform;
    Rigidbody2D _rb;
    Transform _emitterTransform;
    AnimationCurve _curve;
    Vector3 _emitterPosition; //alternative way if you dont want to use a transform
    Vector2 _newKnockbackDir = Vector2.zero, _firstDir = Vector2.zero;
    float _force, _currentDuration;
    int _resistance;
    Timer _knockbackTimer;
    bool _enabled = false;
    bool _stopApplying = false;
    bool _ignoreResistance = false; 
    public event Action<bool> OnKnockbackChange;

    private float Force
    {
        get => _force;
        set
        {
            _force = (_ignoreResistance) ? value : value - (value * _resistance) / 100f;
        }
    }

    public Knockbackeable(Transform ownTransform, Rigidbody2D rb, Action<bool> onKnockbackChange, int knockbackResistance)
    {
        _ownTransform = ownTransform;
        _rb = rb;
        _knockbackTimer = new(0.13f + Random.Range(0.01f, 0.1f), false);
        _knockbackTimer.Stop();
        _knockbackTimer.onEnd += StopKnockback;
        _enabled = false;
        _stopApplying = false;
        _resistance = knockbackResistance;
        OnKnockbackChange = onKnockbackChange;
    }

    public void ReInitialize(Transform ownTransform, Rigidbody2D rb, Action<bool> onKnockbackChange, int knockbackResistance)
    {
        _ownTransform = ownTransform;
        _rb = rb;
        _enabled = false;
        _stopApplying = false;
        _knockbackTimer.Stop();
        _resistance = knockbackResistance;
        OnKnockbackChange = onKnockbackChange;
    }

    public void SetKnockbackData(Transform emitterTransform, float force, float duration = 0.13f, bool ignoreResistance = false, float forceMultiplier = 2f)
    {
        SetLogic(emitterTransform, Vector3.zero, force, duration, ignoreResistance, forceMultiplier);
    }
    public void SetKnockbackData(Vector3 emitterPos, float force, float duration = 0.13f, bool ignoreResistance = false, float forceMultiplier = 2f)
    {
        SetLogic(null, emitterPos, force, duration, ignoreResistance, forceMultiplier);
        _firstDir = _ownTransform.position - _emitterPosition;
    }
    void SetLogic(Transform emitterTransform, Vector3 emitterPos,  float force, float duration, bool ignoreResistance, float forceMultiplier)
    {
        if(_stopApplying) return;
        if(force == 0 || duration <= 0) return;
        _ignoreResistance = ignoreResistance;
        if(_enabled)
        {
            if(_newKnockbackDir != Vector2.zero)
            {
                _ignoreResistance = false;
                return;
            }
            if(_emitterTransform != null) _newKnockbackDir = _emitterTransform.position;
            else _newKnockbackDir = emitterPos;
            _knockbackTimer.ChangeTime(_knockbackTimer.CurrentTime + duration / 1.5f + Random.Range(0.05f, 0.25f));
            Force += force / 1.25f;
            return;
        }
        _emitterTransform = emitterTransform;
        _emitterPosition = emitterPos;
        Force = force * forceMultiplier;

        _enabled = true;
        _currentDuration = duration + Random.Range(-0.02f, 0.05f);
        _knockbackTimer.ChangeTime(_currentDuration);
        _knockbackTimer.Start();
        OnKnockbackChange?.Invoke(_enabled);
        _rb.velocity = Vector2.zero;
    }

    public void ApplyKnockback()
    {
        if(!_enabled) return;
        _knockbackTimer.UpdateTime();
        //Debug.Log("Applying Knockback!");
        Vector2 currentPos = _ownTransform.position;
        if(_emitterTransform != null)
        {
            _firstDir = currentPos - (Vector2)_emitterTransform.position;
        }
        Vector2 secondDirection = currentPos - _newKnockbackDir;
        var direction = Vector2.zero;
        if(secondDirection == currentPos) direction = _firstDir.normalized;
        else
        {
            //Debug.Log("Applying second knockback");
            direction = _firstDir.normalized + secondDirection.normalized;
        }

        var percent = 1f - _knockbackTimer.CurrentTime / _currentDuration;
        var force = Mathf.Lerp(Force / 3f, Force * 1.33f, TweenCurveLibrary.EaseInOutCurve.Evaluate(percent));

        _rb.MovePosition(currentPos + direction.normalized * (force * Time.fixedDeltaTime));
    }

    public void StopKnockback()
    {
        //Debug.Log("Stopping Knockback!");
        _enabled = false;
        _stopApplying = false;
        _newKnockbackDir = Vector2.zero;
        OnKnockbackChange?.Invoke(_enabled);
    }

    public void StopOtherKnockbacks() => _stopApplying = true;

    ~Knockbackeable()
    {
        _knockbackTimer.onEnd -= StopKnockback;
    }
}
