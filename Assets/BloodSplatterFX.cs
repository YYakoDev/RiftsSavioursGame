using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterFX : MonoBehaviour
{
    [SerializeField]Animator _Animator;
    [SerializeField]AnimatorOverrideController[] _SplatterAnimatorVariants;
    private void Awake() {
        gameObject.SetActive(_Animator != null);
        _Animator.runtimeAnimatorController = _SplatterAnimatorVariants[Random.Range(0, _SplatterAnimatorVariants.Length)];
    }
}
