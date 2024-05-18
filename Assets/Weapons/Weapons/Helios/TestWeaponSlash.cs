using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TestWeaponSlash : MonoBehaviour
{
    Animator _animator;
    int _animHash = Animator.StringToHash("Slash");
    private void Awake() {
        _animator = GetComponent<Animator>();
    }
    public void Play()
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).shortNameHash == _animHash)
        {
            _animator.Play("default");
            StartCoroutine(PlayAnimationAfterFrame(_animHash));
        }
        else
        {
            _animator.Play(_animHash);
        }
        
    }

    IEnumerator PlayAnimationAfterFrame(int hash)
    {
        yield return null;
        _animator.Play(hash);
    }

    public void SetAnimationSpeed(float speed)
    {
        _animator.speed = speed / 1.2f;
        if(speed < 1) _animator.speed = 1f;
    }
}
