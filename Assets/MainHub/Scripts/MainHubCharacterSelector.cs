using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class MainHubCharacterSelector : MonoBehaviour
{
    [SerializeField] SOCharacterData[] _charactersList;
    EventSystem _eventSys;
    public event Action onMenuClose, onMenuOpen;
    bool _checkInput = false;
    float _inputCooldown = 0.15f;
    float _nextInputTime;
    private float _horizontalScroll;
    KeyInput _confirmKey;
    [SerializeField]GameObject _menuParent;
    [SerializeField]Image _characterIcon;
    [SerializeField]Button _confirmButton, _closeButton;
    WaitForSecondsRealtime _pauseEnablingWait;
    int _currentIndex = 0;
    int CurrentIndex
    {
        get => _currentIndex;
        set 
        {
            if(value >= _charactersList.Length) _currentIndex = 0;
            else if(value < 0) _currentIndex = _charactersList.Length - 1;
            else _currentIndex = value;
        }
    }

    //scroll animation
    TweenAnimatorMultiple _animator;
    [SerializeField]float _animDuration = 0.7f;
    [SerializeField] Vector3 _endScale = Vector3.one;
    [SerializeField] Vector3 _scaleAnimOffset = Vector3.up;
    [SerializeField] int _scaleBounces = 3;

    //audio stuff

    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        _menuParent.SetActive(false);
    }

    private void Start() {
        _confirmKey = YYInputManager.GetKey(KeyInputTypes.Interact);
        _pauseEnablingWait = new(0.1f);
        _eventSys = EventSystem.current;
        _confirmButton.AddEventListener(Confirm);

        _eventSys.SetSelectedGameObject(_confirmButton.gameObject);
        var character = _charactersList[CurrentIndex];
        _characterIcon.sprite = character.Sprite;

    }


    private void Update() {
        if(!_checkInput || _nextInputTime >= Time.time) return;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
        else 
        {
            _horizontalScroll = Input.GetAxisRaw("Horizontal");
            if(_horizontalScroll == 0f) return;
            int scrollDirection = (_horizontalScroll > 0.1f) ? 1 : -1;
            Scroll(scrollDirection);
        }
    }
    public void Open()
    {
        if(PlayerManager.SelectedChara != null)
        {
            _closeButton.gameObject.SetActive(true);
        }
        else
        {
            // this means you can only exit the menu once you have created a character!
            _closeButton.gameObject.SetActive(false);
        } 
        onMenuOpen?.Invoke();
        YYInputManager.StopInput();
        _menuParent.SetActive(true);
        _checkInput = true;
        PauseMenuManager.DisablePauseBehaviour(true);
        //play animations??
    }

    void Scroll(int dir)
    {
        _nextInputTime = Time.time + _inputCooldown;
        CurrentIndex += dir;
        _eventSys.SetSelectedGameObject(_confirmButton.gameObject);
        var character = _charactersList[CurrentIndex];
        _characterIcon.sprite = character.Sprite;
        PlayScrollAnimation();
        //play a sound maybe?
    }
    public void ScrollLeft() => Scroll(-1);
    public void ScrollRight() => Scroll(1);

    void SetSelectedCharacter() => PlayerManager.ChangeSelectedCharacter(_charactersList[CurrentIndex]);
    

    void PlayScrollAnimation()
    {
        _animator.Clear();
        Vector3 startingSize = _characterIcon.rectTransform.localScale;
        startingSize.x = 0;
        _characterIcon.rectTransform.localScale = startingSize;
        _animator.TweenScaleBouncy(_characterIcon.rectTransform, _endScale, _scaleAnimOffset, _animDuration, 0, _scaleBounces);
    }
    public void Close(bool skipAudio = false)
    {
        _checkInput = false;
        YYInputManager.ResumeInput();
        _menuParent.SetActive(false);
        onMenuClose?.Invoke();
        StartCoroutine(EnablePauseMenuBehaviour());
        //if(!skipAudio) //play the close sfx
    }

    public void Confirm()
    {
        SetSelectedCharacter();
        //play audio here
        Close(true);
    }

    IEnumerator EnablePauseMenuBehaviour()
    {
        yield return _pauseEnablingWait;
        PauseMenuManager.DisablePauseBehaviour(false);
    }
}
