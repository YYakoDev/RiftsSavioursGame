using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockbackeable
{
    Transform _ownTransform;
    Vector2 _knockbackDirection = Vector2.zero;
    bool _enabled = true;
    public bool Enabled { get => _enabled; set => _enabled = value; }
    public Knockbackeable(Transform ownTransform)
    {
        _ownTransform = ownTransform;
    }

    public void ApplyForce(Vector2 emitterPosition, float force)
    {
        if(!_enabled) return;
        _knockbackDirection = (Vector2)_ownTransform.position - emitterPosition;
        _knockbackDirection.Normalize();
       
        force = Mathf.Clamp(force, 0, 2.25f);

        _ownTransform.position += (Vector3)_knockbackDirection * force;

    }

}
