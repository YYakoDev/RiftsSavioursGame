using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbackeable
{
    Transform _ownTransform;
    Rigidbody2D _rb;
    Vector2 _emitterPos;
    float _force;
    Timer _knockbackTimer;
    int _knockbackHits = 0;
    bool _enabled = false;
    bool _alwaysApply = false;

    public bool Enabled => _enabled;

    public Knockbackeable(Transform ownTransform, Rigidbody2D rb, bool alwaysApplyKnockback = false)
    {
        _ownTransform = ownTransform;
        _rb = rb;
        _alwaysApply = alwaysApplyKnockback;
        _knockbackTimer = new(0.13f + Random.Range(0.01f, 0.1f), false);
        _knockbackTimer.onReset += StopKnockback;
    }

    public void SetKnockbackData(Vector3 emitterPos, float force)
    {
        _emitterPos = emitterPos;
        _force = force * 2;
        if(_alwaysApply)
        {
            EnableKnockback();
            return;
        }

        _knockbackHits++;
        if(_knockbackHits >= 0)
        {
            if(_knockbackHits > 3)
            {
                _knockbackHits = -2;
                _enabled = false;
            }else EnableKnockback();
        }
    }

    public void ApplyKnockback()
    {
        if(!_enabled) return;
        _knockbackTimer.UpdateTime();
        //Debug.Log("Applying Knockback!");
        Vector2 currentPos = _ownTransform.position;
        Vector2 direction = currentPos - _emitterPos;
        _rb.MovePosition(currentPos + direction.normalized * (_force * Time.fixedDeltaTime));
    }

    public void StopKnockback()
    {
        //Debug.Log("Stopping Knockback!");
        _enabled = false;
    }

    void EnableKnockback()
    {
        _enabled = true;
        _knockbackTimer.Start();
    }
}
