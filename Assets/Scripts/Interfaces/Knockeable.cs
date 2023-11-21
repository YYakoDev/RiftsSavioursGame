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

    public Knockbackeable(Transform ownTransform, Rigidbody2D rb)
    {
        _ownTransform = ownTransform;
        _rb = rb;
        _knockbackTimer = new(0.25f, false);
        _knockbackTimer.onReset += StopKnockback;
    }

    public void SetKnockbackData(Vector3 emitterPos, float force)
    {
        _emitterPos = emitterPos;
        _force = force;
        
        _knockbackTimer.Start();
    }

    public void ApplyKnockback()
    {
        if(_emitterPos.sqrMagnitude < 0.1f) return;

        _knockbackTimer.UpdateTime();

        Debug.Log("Applying Knockback!");
        //_stopMovement = true;

        Vector2 currentPos = _ownTransform.position;
        Vector2 direction = _emitterPos - currentPos;
        //_enemy.Rigidbody.MovePosition(currentPos + direction.normalized * (_enemy.Stats.Speed * Time.fixedDeltaTime));
    }

    public void StopKnockback()
    {
        Debug.Log("Stopping Knockback!");
        _emitterPos = Vector2.zero;
    }

    /*Vector2 _knockbackDirection = Vector2.zero;
    bool _enabled = true;
    public bool Enabled { get => _enabled; set => _enabled = value; }*/

    /*public void ApplyForce(Vector2 emitterPosition, float force)
    {
        if(!_enabled) return;
        _knockbackDirection = (Vector2)_ownTransform.position - emitterPosition;
        _knockbackDirection.Normalize();
       
        force = Mathf.Clamp(force, 0, 2.25f);

        _ownTransform.position += (Vector3)_knockbackDirection * force;

    }*/

}
