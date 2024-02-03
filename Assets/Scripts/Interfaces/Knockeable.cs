using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Knockbackeable
{
    Transform _ownTransform;
    Rigidbody2D _rb;
    Transform _emitterTransform;
    Vector3 _emitterPosition; //alternative way if you dont want to use a transform
    Vector2 _newKnockbackDir = Vector2.zero;
    float _force;
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
            _knockbackTimer.ChangeTime(_knockbackTimer.CurrentTime + duration / 1.5f + Random.Range(0.01f, 0.07f));
            Force += force / 1.25f;
            return;
        }
        _emitterTransform = emitterTransform;
        _emitterPosition = emitterPos;
        Force = force * forceMultiplier;

        _enabled = true;
        _knockbackTimer.ChangeTime(duration + Random.Range(0.01f, 0.1f));
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
        Vector2 firstDirection = (_emitterTransform != null) ?
        currentPos - (Vector2)_emitterTransform.position :
        currentPos - (Vector2)_emitterPosition;
        Vector2 secondDirection = currentPos - _newKnockbackDir;
        Vector2 direction;
        if(secondDirection == currentPos) direction = firstDirection.normalized;
        else direction = firstDirection.normalized + secondDirection.normalized;

        _rb.MovePosition(currentPos + direction.normalized * (Force * Time.fixedDeltaTime));
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
