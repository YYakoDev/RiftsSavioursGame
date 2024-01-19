using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TestWeaponSlash : MonoBehaviour
{
    Animator _animator;
    private void Awake() {
        _animator = GetComponent<Animator>();
    }
    public void Play()
    {
        _animator.Play("Slash");
    }
    public void SetAnimationSpeed(float speed)
    {
        _animator.speed = speed / 1.5f;
        if(speed < 1) _animator.speed = 1f;
    }
}
