using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[RequireComponent(typeof(TweenAnimatorMultiple))]
public class CharacterSelectionUI : MonoBehaviour
{
    bool _stop = false;
    [SerializeField] GameObject _startButton;
    [SerializeField] SOCharacterData[] _availableCharacters = new SOCharacterData[0];
    [SerializeField] Image _characterIcon;
    int _currentIndex = 0;
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
    [SerializeField] AudioClip _moveSFX, _chooseSFX;
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
        if(_stop || _nextInputTime >= Time.time) return;
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
        if(_startButton != null)EventSystem.current.SetSelectedGameObject(_startButton);
    }

    void SetSelectedCharacter()
    {
        var character = _availableCharacters[_currentIndex];
        PlayerManager.ChangeSelectedCharacter(character);
        PlayAnimation();
        _characterIcon.sprite = character.Sprite;
    }

    void PlayAnimation()
    {
        _animator.Clear();
        Vector3 startingSize = _characterIcon.rectTransform.localScale;
        startingSize.x = 0;
        _characterIcon.rectTransform.localScale = startingSize;
        _animator.TweenScaleBouncy(_characterIcon.rectTransform, _endScale, _scaleAnimOffset, _animDuration, 0, _scaleBounces);
    }


    public void ChooseCharacter()
    {
        _stop = true;
        SetSelectedCharacter();
        _characterIcon.enabled = false;
        _audio?.PlayWithVaryingPitch(_chooseSFX);
    }

    public void ResumeCharacterSelection()
    {
        _stop = false;
        _characterIcon.enabled = true;
    }
}
