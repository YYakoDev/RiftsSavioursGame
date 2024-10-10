using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatterFX : MonoBehaviour
{
    [SerializeField]Animator _animator;
    [SerializeField] SpriteRenderer _renderer;
    Color _initialColor;
    Color _endColor = new Color(0.25f, 0, 0, 0f);
    Vector2 _offset = new Vector2(-0.51f, 0.1f);
    float _coagulationTime = 10f;
    float _elapsedTime = 0f;

    private void Awake() {
        gameObject.SetActive(_animator != null || _renderer != null);
        _initialColor = _renderer.color;
        
    }
    private void OnEnable() {
        _renderer.color = _initialColor;
        _animator.enabled = true;
    }

    public void Set(SOBloodFX bloodData)
    {
        _initialColor = bloodData.InitialColor;
        //_renderer.enabled = false;
        _animator.runtimeAnimatorController = null;
        _animator.runtimeAnimatorController = bloodData.Animator;
        _elapsedTime = 0f;
        _coagulationTime = bloodData.CoagulationTime + 5f;
        transform.localScale = bloodData.Size;
        //_renderer.enabled = true;
    }

    private void Update() {
        if(_elapsedTime >= _coagulationTime)
        {
            gameObject.SetActive(false);
            return;
        }
        _elapsedTime += Time.deltaTime;
        float percent = _elapsedTime / _coagulationTime;
        _renderer.color = Color.Lerp(_initialColor, _endColor, percent);
        if(_elapsedTime >= _coagulationTime) _animator.enabled = false;
    }

    public void Flip(Vector3 playerPos)
    {
        Transform obj = transform;
        Vector3 flippedScale = obj.localScale;
        Vector3 direction = obj.position - playerPos;
        direction.Normalize();
        flippedScale.x *= (direction.x > 0) ? -1 : 1;
        obj.localScale = flippedScale;
        obj.position += (Vector3)(_offset * flippedScale.x);
    }
    
}
