using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class LoadingScreenAnimations : MonoBehaviour
{
    TweenAnimatorMultiple _animator;
    [SerializeField] TextMeshProUGUI _loadingText;

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
    }

    private void Start() {
        _animator.TweenTextOpacity(_loadingText, 0, 255, loop: true);
    }
}
