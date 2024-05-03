using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class InteractableHotkey : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField]TextMeshPro _text;
    [SerializeField] Light2D _light;
    GameObject _object;
    Transform _transform;
    TweenAnimatorMultiple _animator;
    [SerializeField] float _fadeIn = 0.5f, _fadeOut = 0.2f, _lightIntensity = 1f, _shakeStrength = 5f, _shakeDuration = 0.2f;
    public TextMeshPro Text => _text;
    public GameObject Self => _object;
    private void Awake() {
        if(_text == null) _text = GetComponentInChildren<TextMeshPro>();
        if(_renderer == null) _renderer = GetComponent<SpriteRenderer>();
        if(_light == null) _light = GetComponentInChildren<Light2D>();
        _animator = GetComponent<TweenAnimatorMultiple>();
        _object = gameObject;
        _transform = transform;

        Color spriteColor = _renderer.color;
        spriteColor.a = 0f;
        _renderer.color = spriteColor;
        Color textColor = _text.color;
        textColor.a = 0f;
        _text.color = textColor;
        _light.intensity = 0f;
    }

    public void ChangeState(bool value)
    {
        if(value)Self.SetActive(value);
        _animator.Clear();
        float opacityValue = (value) ? 255f : 0f;
        float duration = (value) ? _fadeIn : _fadeOut;
        float lightIntensity = (value) ? _lightIntensity : 0f;
        Color spriteColor = _renderer.color;
        spriteColor.a = opacityValue / 255f;
        _animator.TweenSpriteColor(_renderer, spriteColor, duration, onComplete: () 
        => 
        {
            if(!value) Self.SetActive(value);
        });
        _animator.TweenTextOpacity(_text, opacityValue, duration);
        _animator.TweenLightIntensity(_light, lightIntensity, duration);

    }

    public void ShakeAnim(System.Action onComplete = null)
    {
        Vector3 startPos = _transform.localPosition;
        Vector3 randomDir = new Vector3(0f, Random.Range(-0.5f, -0.2f), 0f);
        _animator.TweenMoveTransformBouncy(_transform, startPos + randomDir * _shakeStrength, randomDir * -_shakeStrength, _shakeDuration, 0, 5, onBounceComplete: () => 
        {
            _animator.TweenTransformMoveTo(_transform, startPos, _shakeDuration / 5f, onComplete: onComplete);
        });
    }

}
