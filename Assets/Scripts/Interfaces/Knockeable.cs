using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbackeable
{
    Transform _ownTransform;
    Rigidbody2D _rb;
    Transform _emitterPos;
    float _force;
    Timer _knockbackTimer;
    bool _enabled = false;
    bool _stopApplying = false;

    public bool Enabled => _enabled;

    public Knockbackeable(Transform ownTransform, Rigidbody2D rb, bool alwaysApplyKnockback = false)
    {
        _ownTransform = ownTransform;
        _rb = rb;
        _knockbackTimer = new(0.13f + Random.Range(0.01f, 0.1f), false);
        _knockbackTimer.onEnd += StopKnockback;
        _enabled = false;
        _stopApplying = false;
    }

    public void SetKnockbackData(Transform emitterPos, float force)
    {
        if(_stopApplying) return;
        _emitterPos = emitterPos;
        _force = force * 2;
        _enabled = true;
        _knockbackTimer.Start();
    }
    public void SetKnockbackData(Transform emitterPos, float force, float duration)
    {
        if(_stopApplying) return;
        _emitterPos = emitterPos;
        _force = force * 2;
        _knockbackTimer.ChangeTime(duration);
        _enabled = true;
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
    }

    public void StopApplying() => _stopApplying = true;
}
