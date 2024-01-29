using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUILayout : MonoBehaviour
{
    [Header("Navigation Stuff")]
    [SerializeField] Vector2 _behindScale;
    
    [SerializeField]RectTransform[] _elements = new RectTransform[3];

    [SerializeField] Vector3[] _backPositions = new Vector3[2];
    Vector3[] _positions;
    float _inputDirection;
    int _currentIndex;
    [SerializeField]float _switchCooldown = 0.5f;
    float _nextSwitchTime = 0f;

    [Header("Animation Stuff")]
    [SerializeField] float _movementAnimDuration = 0.5f;
    [SerializeField] float _scaleAnimDuration = 0.5f;
    TweenAnimatorMultiple _animator;



    private int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            _currentIndex = value;
            if(value < 0) _currentIndex = _elements.Length - 1;
            if(value >= _elements.Length) _currentIndex = 0;
        }
    }


    private void Awake() {
        _animator = this.CheckOrAddComponent<TweenAnimatorMultiple>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _positions = new Vector3[3]
        {
            _backPositions[0],
            Vector3.zero,
            _backPositions[1]
        };
        for (int i = 0; i < _elements.Length; i++)
        {
            var element = _elements[i];
            element.localPosition = _positions[i];
            element.localScale = _behindScale;
            if(i == 1) element.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _inputDirection = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate() {
        if(_nextSwitchTime >= Time.time) return;
        if(_inputDirection != 0) SwitchUIElement();
    }

    void SwitchUIElement()
    {
        _nextSwitchTime = _switchCooldown + Time.time;
        if(_inputDirection > 0.1f) CurrentIndex++;
        else CurrentIndex--;
        IncreaseCooldown(_movementAnimDuration + 0.1f);
        for (int i = 0; i < _elements.Length; i++)
        {
            var element = _elements[i];
            var position = _positions[CurrentIndex];
            Vector3 scale = (_currentIndex == 1) ? Vector3.one : _behindScale;
            PlayMovementAnimation(element, position);
            PlayScaleAnimation(element, scale);
            CurrentIndex++;
        }
    }

    void PlayMovementAnimation(RectTransform element, Vector3 endPos)
    {
        _animator?.TweenMoveToBouncy(element, endPos, Vector3.right * 5f, _movementAnimDuration, 0, 3);
    }
    void PlayScaleAnimation(RectTransform element, Vector3 endScale)
    {
        _animator?.Scale(element, endScale, _scaleAnimDuration);
    }

    void IncreaseCooldown(float time) => _nextSwitchTime = Time.time + time;

    private void OnValidate() {
        if(_elements.Length > 3) System.Array.Resize<RectTransform>(ref _elements, 3);
        if(_backPositions.Length > 2) System.Array.Resize<Vector3>(ref _backPositions, 2);
    }
}
