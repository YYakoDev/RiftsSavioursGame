using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterFX : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField] SpriteRenderer _renderer;
    Color _initialColor;
    Color _endColor = new Color(0.25f, 0, 0, 1);
    [SerializeField]AnimatorOverrideController[] _SplatterAnimatorVariants;
    [SerializeField] float _coagulationTime = 10f;
    float _elapsedTime = 0f;

    private void Awake() {
        gameObject.SetActive(_animator != null || _renderer != null);
        _animator.runtimeAnimatorController = _SplatterAnimatorVariants[Random.Range(0, _SplatterAnimatorVariants.Length)];
        _initialColor = _renderer.color;
    }
    private void OnEnable() {
        _renderer.color = _initialColor;
    }

    private void Update() {
        if(_elapsedTime >= _coagulationTime) return;
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _coagulationTime;
        _renderer.color = Color.Lerp(_initialColor, _endColor, percent);
    }
    
}
