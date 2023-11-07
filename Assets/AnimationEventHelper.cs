using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHelper : MonoBehaviour
{
    Animator _animator;
    private void Awake() {
        _animator = GetComponent<Animator>();
    }
    public void DisableAnimator()
    {
        if(_animator == null) return;
        _animator.enabled = false;
    }
}
