using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimatorOnEnable : MonoBehaviour
{
    Animator _animator;
    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        if(_animator == null) enabled = false;
        _animator.enabled = true;
    }
}
