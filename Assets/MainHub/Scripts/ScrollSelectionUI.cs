using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class ScrollSelectionUI<T> : MonoBehaviour
{
    protected enum ScrollType
    {
        Horizontal,
        Vertical
    }
    [SerializeField] ScrollType _scrollType = ScrollType.Horizontal;
    [SerializeField] protected T[] _selectableDatalist;
    protected EventSystem _eventSys;
    string _scrollInput;
    protected bool _initialized = false;

    public event Action onMenuClose, onMenuOpen;
    protected event Action<T> onConfirm;
    protected bool _checkInput = false;
    protected float _inputCooldown = 0.15f;
    float _nextInputTime;
    private float _horizontalScroll;
    [SerializeField] protected GameObject _menuParent;
    [SerializeField] protected Image _characterIcon;
    [SerializeField] protected Button _confirmButton, _closeButton;
    [SerializeField] protected ScaleOnSelected _leftArrow, _rightArrow;
    [SerializeField] InputActionReference _escapeInput;
    GameObject _confirmButtonObj;
    WaitForSecondsRealtime _pauseEnablingWait;
    int _currentIndex = 0;
    protected int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (value >= _selectableDatalist.Length) _currentIndex = 0;
            else if (value < 0) _currentIndex = _selectableDatalist.Length - 1;
            else _currentIndex = value;
        }
    }

    //scroll animation
    protected TweenAnimatorMultiple _animator;
    [SerializeField] float _animDuration = 0.7f;
    [SerializeField] Vector3 _endScale = Vector3.one;
    [SerializeField] Vector3 _scaleAnimOffset = Vector3.up;
    [SerializeField] int _scaleBounces = 3;

    //audio stuff
    AudioSource _audio;
    [SerializeField] AudioClip _scrollSFX, _closeSFX, _confirmSFX;

    private void Awake()
    {
        _animator = GetComponent<TweenAnimatorMultiple>();
        _animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        _menuParent.SetActive(false);
        _scrollInput = (_scrollType == ScrollType.Horizontal) ? "Horizontal" : "Vertical";
    }

    protected virtual void Initialize()
    {
        _pauseEnablingWait = new(0.1f);
        _eventSys = EventSystem.current;
        _confirmButton.AddEventListener(Confirm);
        _eventSys.SetSelectedGameObject(_confirmButton.gameObject);
        _confirmButtonObj = _confirmButton.gameObject;
        _initialized = true;
    }

    protected virtual void UpdateLogic()
    {
        if (!_checkInput || _nextInputTime >= Time.time) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
        else
        {
            _horizontalScroll = Input.GetAxisRaw(_scrollInput);
            if (_horizontalScroll == 0f) return;
            int scrollDirection = (_horizontalScroll > 0.1f) ? 1 : -1;
            Scroll(scrollDirection);
            ScaleArrows(scrollDirection == 1);
        }
    }
    
    private void Start()
    {
        if (!_initialized) Initialize();
        _escapeInput.action.performed += CloseWithInput;
    }


    private void Update()
    {
        UpdateLogic();
    }
    public virtual void Open()
    {
        onMenuOpen?.Invoke();
        YYInputManager.StopInput();
        _menuParent.SetActive(true);
        _checkInput = true;
        PauseMenuManager.DisablePauseBehaviour(true);
        //play animations??
    }

    protected virtual void Scroll(int dir)
    {
        _nextInputTime = Time.time + _inputCooldown;
        CurrentIndex += dir;
        //_eventSys.SetSelectedGameObject(_confirmButtonObj); //maybe this should be optional?
        PlayScrollAnimation();
        //play a sound maybe?
    }
    public virtual void ScrollLeft()
    {
        ScaleArrows(false);
        Scroll(-1);
    }
    public virtual void ScrollRight()
    {
        ScaleArrows(true);
        Scroll(1);
    }

    void ScaleArrows(bool right)
    {
        if(right)
        {
            _leftArrow.ScaleDown();
            _rightArrow.ScaleUp();
        }else
        {
            _leftArrow.ScaleUp();
            _rightArrow.ScaleDown();
        }
    }


    void PlayScrollAnimation()
    {
        _animator.Clear();
        Vector3 startingSize = _characterIcon.rectTransform.localScale;
        startingSize.x = 0;
        _characterIcon.rectTransform.localScale = startingSize;
        _animator.TweenScaleBouncy(_characterIcon.rectTransform, _endScale, _scaleAnimOffset, _animDuration, 0, _scaleBounces);
    }
    public virtual void Close(bool skipAudio = false)
    {
        _checkInput = false;
        YYInputManager.ResumeInput();
        _menuParent.SetActive(false);
        onMenuClose?.Invoke();
        StartCoroutine(EnablePauseMenuBehaviour());
        //if(!skipAudio) //play the close sfx
    }

    void CloseWithInput(InputAction.CallbackContext obj)
    {
        Close();
    }
    public void Confirm()
    {
        _leftArrow.ScaleDown();
        _rightArrow.ScaleDown();
        onConfirm?.Invoke(_selectableDatalist[CurrentIndex]);
        //play confirm audio here
        Close(true);
    }

    IEnumerator EnablePauseMenuBehaviour()
    {
        yield return _pauseEnablingWait;
        PauseMenuManager.DisablePauseBehaviour(false);
    }

    private void OnDestroy() {
        _escapeInput.action.performed -= CloseWithInput;
    }
}
