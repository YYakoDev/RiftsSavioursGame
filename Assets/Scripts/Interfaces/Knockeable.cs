using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Knockbackeable
{
    Transform _ownTransform;
    Rigidbody2D _rb;
    Transform _emitterPos;
    float _force;
    Timer _knockbackTimer;
    bool _enabled = false;
    bool _stopApplying = false;
    public event Action<bool> OnKnockbackChange;

    public Knockbackeable(Transform ownTransform, Rigidbody2D rb, Action<bool> onKnockbackChange)
    {
        _ownTransform = ownTransform;
        _rb = rb;
        _knockbackTimer = new(0.13f + Random.Range(0.01f, 0.1f), false);
        _knockbackTimer.onEnd += StopKnockback;
        _enabled = false;
        _stopApplying = false;
        OnKnockbackChange = onKnockbackChange;
    }

    public void SetKnockbackData(Transform emitterPos, float force, float duration = 0.13f)
    {
        if(_stopApplying) return;
        _emitterPos = emitterPos;
        _force = force * 2;
        _enabled = true;
        _knockbackTimer.ChangeTime(duration + Random.Range(0.01f, 0.1f));
        _knockbackTimer.Start();
        OnKnockbackChange?.Invoke(_enabled);
    }

    public void ApplyKnockback()
    {
        if(!_enabled) return;
        _knockbackTimer.UpdateTime();
        //Debug.Log("Applying Knockback!");
        Vector2 currentPos = _ownTransform.position;
        Vector2 direction = currentPos - (Vector2)_emitterPos.position;
        _rb.MovePosition(currentPos + direction.normalized * (_force * Time.fixedDeltaTime));
    }

    void StopKnockback()
    {
        //Debug.Log("Stopping Knockback!");
        _enabled = false;
        _stopApplying = false;
        OnKnockbackChange?.Invoke(_enabled);
    }

    public void StopOtherKnockbacks() => _stopApplying = true;
}
