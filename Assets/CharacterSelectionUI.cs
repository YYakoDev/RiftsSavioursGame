using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(TweenAnimatorMultiple))]
public class CharacterSelectionUI : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] SOCharacterData[] _availableCharacters = new SOCharacterData[0];
    [SerializeField] Image _characterIcon;
    int _currentIndex = 0;
    public static SOCharacterData SelectedCharacter;
    float _inputCooldown = 0.25f;
    float _nextInputTime = 0;

    int CurrentIndex
    {
        get => _currentIndex;
        set 
        {
            if(value >= _availableCharacters.Length) _currentIndex = 0;
            else if(value < 0) _currentIndex = _availableCharacters.Length - 1;
            else _currentIndex = value;
        }
    }

    [Space][SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _moveSFX;
    [Space]
    TweenAnimatorMultiple _animator;
    [SerializeField]float _animDuration = 0.7f;
    [SerializeField] Vector3 _endScale = Vector3.one;
    [SerializeField] Vector3 _scaleAnimOffset = Vector3.up;
    [SerializeField] int _scaleBounces = 3;

    private void Awake() {
        _nextInputTime = Time.time + 0.2f;
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
    }

    private void Start() {
        SetSelectedCharacter();
    }

    private void Update() {
        if(_nextInputTime >= Time.time) return;
        float input = Input.GetAxisRaw("Horizontal");
        
        if(input != 0)
        {
            int move = (input > 0.1f) ? 1 : -1;
            MoveIndex(move);
        }
    }

    public void MoveIndex(int move)
    {
        _audio?.PlayWithVaryingPitch(_moveSFX);
        move = Mathf.Clamp(move, -1, 1);
        _nextInputTime = _inputCooldown + Time.time;
        CurrentIndex += move;
        SetSelectedCharacter();
        EventSystem.current.SetSelectedGameObject(_startButton);
    }

    void SetSelectedCharacter()
    {
        SelectedCharacter = _availableCharacters[_currentIndex];
        PlayAnimation();
        _characterIcon.sprite = SelectedCharacter.Sprite;
    }

    void PlayAnimation()
    {
        _animator.Clear();
        Vector3 startingSize = _characterIcon.rectTransform.localScale;
        startingSize.x = 0;
        _characterIcon.rectTransform.localScale = startingSize;
        _animator.TweenScaleBouncy(_characterIcon.rectTransform, _endScale, _scaleAnimOffset, _animDuration, 0, _scaleBounces);
    }



}
