using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Knockbackeable
{
    Transform _ownTransform;
    Rigidbody2D _rb;
    Transform _emitterPos;
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
        _knockbackTimer.onEnd += StopKnockback;
        _enabled = false;
        _stopApplying = false;
        _resistance = knockbackResistance;
        OnKnockbackChange = onKnockbackChange;
    }

    public void SetKnockbackData(Transform emitterPos, float force, float duration = 0.13f, bool ignoreResistance = false, float forceMultiplier = 2f)
    {
        if(_stopApplying) return;
        _ignoreResistance = ignoreResistance;
        if(_enabled)
        {
            if(_newKnockbackDir != Vector2.zero) return;
            _newKnockbackDir = _emitterPos.position;
            _knockbackTimer.ChangeTime(_knockbackTimer.CurrentTime + duration / 1.5f + Random.Range(0.01f, 0.07f));
            Force += force / 1.25f;
            return;
        }
        _emitterPos = emitterPos;
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
        Vector2 direction = currentPos - ((Vector2)_emitterPos.position + _newKnockbackDir);
        _rb.MovePosition(currentPos + direction.normalized * (Force * Time.fixedDeltaTime));
    }

    void StopKnockback()
    {
        //Debug.Log("Stopping Knockback!");
        _enabled = false;
        _stopApplying = false;
        _newKnockbackDir = Vector2.zero;
        OnKnockbackChange?.Invoke(_enabled);
    }

    public void StopOtherKnockbacks() => _stopApplying = true;
}
