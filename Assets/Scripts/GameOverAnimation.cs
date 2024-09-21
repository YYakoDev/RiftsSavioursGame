using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverAnimation : MonoBehaviour
{
    [SerializeField] bool _debugPlayAnimationAtStart;
    bool _playGameOver;
    
    [Header("References")]
    [SerializeField] Canvas _canvas;
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] Camera _mainCamera;
    [SerializeField] PlayerInputController _inputController;
    [SerializeField] GameObject _visualsParent;
    [SerializeField] RectTransform _redScreenRect;
    [SerializeField] MenuController _menuController;
    Image _redScreenImg;
    [SerializeField] RectTransform _playerImageRect;
    Image _playerImg;
    [SerializeField] RectTransform _gameOverTextRect;
    [SerializeField] Animator _gameOverTxtAnimatorComponent;
    Image _gameOverTextImg;
    [SerializeField] RectTransform[] _buttons;
    TweenAnimator _redScreenAnimator;
    TweenAnimator _playerAnimator;
    TweenAnimator _GOTextAnimator;
    TweenAnimator[] _buttonAnimators;

    [Header("Game Slow Freeze Animation")]
    [SerializeField]float _slowFreezeTime = 2f;
    Timer _gameFreezeTimer;

    [Header("Player Image Animation")]
    [SerializeField] Color _playerImgEndColor;
    [SerializeField] float _playerAnimDuration = 1f;
    SpriteRenderer _playerRenderer;
    Sprite _playerSprite;

    [Header("Red Screen Animation")]

    [SerializeField] float _redScreenFadeDuration = 0.56f;
    Color _redScreenInitialColor;
    [SerializeField] float _redScreenColorChangeDuration = 0.2f;
    [SerializeField] Color[] _redScreenBlinkColors;

    [Header("Game Over Text Animation")]
    [SerializeField] Color _GOverTxtInitialColor;
    [SerializeField] Color _GOverTxtEndColor;
    [SerializeField] float _GOverTxtMovementDuration = 1f;
    [SerializeField] string _GOverTxtAnimName = "Animation";
    [SerializeField] int _GOverMoveIterations = 5;
    [SerializeField] float _GOVerTxtColorChangeDuration = 1f;
    [SerializeField] Vector2 _GOverTxtInitialPos;
    Vector2 _GOverTxtEndPos;

    [Header("BUttons Animation")]
    [SerializeField] Vector2 _buttonsInitialPos;
    [SerializeField] float _buttonsAnimDuration = 1f;
    Vector2[] _buttonsEndPos;


    [Header("Audio Stuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _gameOverSound;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _gameFreezeTimer = new(_slowFreezeTime, useUnscaledTime: true);
        _gameFreezeTimer.onEnd += GameOver;
        _gameFreezeTimer.Start();

        if(Application.installMode != ApplicationInstallMode.Editor) _debugPlayAnimationAtStart = false;
    }

    private void Start()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        TimeScaleManager.ForceTimeScale(1f);
        _playerStats.onStatsChange += CheckHealth;
        _redScreenImg = _redScreenRect.GetComponent<Image>();
        _playerImg = _playerImageRect.GetComponent<Image>();

        _redScreenInitialColor = _redScreenImg.color;
        _redScreenInitialColor.a = 1;
        _visualsParent.SetActive(_debugPlayAnimationAtStart);

        _buttonsEndPos = new Vector2[_buttons.Length];
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttonsEndPos[i] = _buttons[i].localPosition;
        }
        CheckAnimators();
        SetInitialStates();
        _playerImageRect.gameObject.SetActive(false);
        _playGameOver = _debugPlayAnimationAtStart;
    }

    void CheckHealth()
    {
        if (_playerStats.CurrentHealth <= 0 && !_playGameOver)
        {
            Debug.Log("<b>Game Over :( </b>");
            _playGameOver = true;
            //YYInputManager.StopInput();
            //GameOver();
            _inputController.ChangeInputToUI();
            PauseMenuManager.DisablePauseBehaviour(true);
            _menuController.SwitchCurrentMenu(gameObject);
        }
    }

    private void Update() {
        if(!_playGameOver) return;
        _gameFreezeTimer.UpdateTime();
        float percent = 1f - (_gameFreezeTimer.CurrentTime / _slowFreezeTime);
        if(percent >= 1.05f || percent == 0) return;
        float result = Mathf.Lerp(1, 0, percent);
        TimeScaleManager.SetTimeScale(result);

    }

    void GameOver()
    {
        _visualsParent.SetActive(true);
        CheckAnimators();
        TimeScaleManager.ForceTimeScale(0f);
        SetInitialStates();
        PlayAnimations();
        _inputController.ChangeInputToUI();
    }

    void CheckAnimators()
    {
        //RedScreen Background
        GetAnimatorAndChangeTimeScale(_redScreenRect, ref _redScreenAnimator);
        //PlayerIMG
        if (_playerRenderer == null)
        {
            _playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SpriteRenderer>();
        }
        _playerSprite = _playerRenderer.sprite;
        GetAnimatorAndChangeTimeScale(_playerImageRect, ref _playerAnimator);
        //Game Over Text (GOText)
        GetAnimatorAndChangeTimeScale(_gameOverTextRect, ref _GOTextAnimator);
        _buttonAnimators = new TweenAnimator[_buttons.Length];
        for (int i = 0; i < _buttonAnimators.Length; i++)
        {
            GetAnimatorAndChangeTimeScale(_buttons[i], ref _buttonAnimators[i]);
        }
    }

    void GetAnimatorAndChangeTimeScale(RectTransform rect, ref TweenAnimator animator)
    {
        if(animator == null) animator = rect.CheckOrAddComponent<TweenAnimator>();
        animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
    }

    void SetInitialStates()
    {
        if (_redScreenImg == null) _redScreenImg = _redScreenRect.GetComponent<Image>();
        if (_playerImg == null) _playerImg = _playerImageRect.GetComponent<Image>();
        if (_gameOverTextImg == null) _gameOverTextImg = _gameOverTextRect.GetComponent<Image>();

        //PLAYER UI SPRITE
        _playerImg.sprite = _playerSprite;
        _playerImg.color = Color.white;
        Vector3 flippedScale = _playerImageRect.localScale;
        flippedScale.x = _playerRenderer.transform.localScale.x;
        _playerImageRect.localScale = flippedScale;
        _playerImageRect.gameObject.SetActive(true);
        //BG STUFF
        Color newColor =_redScreenInitialColor;
        newColor.a = 0;
        _redScreenImg.color = newColor;

        //GAME OVER LOGO
        Color newTextColor = _gameOverTextImg.color;
        newTextColor.a = 0;
        _gameOverTextImg.color = newTextColor;
        _gameOverTextRect.localPosition = _GOverTxtInitialPos;
        _gameOverTextRect.gameObject.SetActive(false);
        _GOverTxtEndPos = new Vector2(_playerImageRect.localPosition.x, _playerImageRect.localPosition.y + 200f);

        //GAME OVER BUTTONS
        foreach(RectTransform btn in _buttons)
        {
            btn.localPosition = _buttonsInitialPos;
        }
    }

    void PlayAnimations()
    {
        foreach(TweenAnimator animator in _buttonAnimators) animator.Clear();
        _playerAnimator.Clear();
        _redScreenAnimator.Clear();
        _GOTextAnimator.Clear();

        _playerAnimator.TweenImageColor(_playerImageRect, _playerImgEndColor, _playerAnimDuration);
        _redScreenAnimator.TweenImageOpacity(_redScreenRect, 255, _redScreenFadeDuration);
        for (int i = 0; i < _redScreenBlinkColors.Length; i++)
        {
            var initialColor = (i == 0) ?  _redScreenInitialColor : _redScreenBlinkColors[i-1];
            _redScreenAnimator.TweenImageColor
            (_redScreenRect, initialColor, _redScreenBlinkColors[i], _redScreenColorChangeDuration);
        }
        //this is a fake animation that is used to know when the color blinking anim has completed
        _redScreenAnimator.TweenImageOpacity(_redScreenRect, 255f, 0f, onComplete: PlayRestOfAnimations);
    }

    void PlayRestOfAnimations()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        float duration = 0.1f + _redScreenColorChangeDuration;
        _redScreenAnimator.TweenImageColor(_redScreenRect, _redScreenInitialColor,  _playerImgEndColor, duration,
        onComplete: () => 
        {
            _gameOverTextRect.gameObject.SetActive(true);
            _gameOverTxtAnimatorComponent.enabled = true;
        });
        _playerAnimator.TweenImageColor(_playerImageRect, _playerImgEndColor,  _redScreenInitialColor, duration);

        _playerAnimator.TweenMoveToBouncy(_gameOverTextRect, _GOverTxtEndPos, Vector3.right * 50f, _GOverTxtMovementDuration, 0, _GOverMoveIterations,
        onBounceComplete: () => 
        {
            _playerAnimator.MoveTo(_gameOverTextRect, _GOverTxtEndPos, _GOverTxtMovementDuration / (_GOverMoveIterations * 3f), CurveTypes.Linear);
        });


        //_GOTextAnimator.TweenImageOpacity(_gameOverTextRect, 255, _GOVerTxtColorChangeDuration);
        _GOTextAnimator.TweenImageColor(_gameOverTextRect, _GOverTxtInitialColor, _GOVerTxtColorChangeDuration, onComplete: () => 
        {
            _gameOverTxtAnimatorComponent.Play(_GOverTxtAnimName);
        });
        _GOTextAnimator.TweenImageColor(_gameOverTextRect, _GOverTxtInitialColor, _GOverTxtEndColor, _GOVerTxtColorChangeDuration,
        onComplete: () => 
        {
            ButtonAnimations();
        });

    }

    void ButtonAnimations()
    {
        EventSystem.current.SetSelectedGameObject(_buttons[0].gameObject);
        //here maybe you could add the game over stats and not just the buttons
        for (int i = 0; i < _buttons.Length; i++)
        {
            float duration = (_buttonsAnimDuration / (i+1)) + (_buttonsAnimDuration/3f) * i;
            _buttonAnimators[i].TweenMoveToBouncy(_buttons[i], _buttonsEndPos[i], Vector3.up * 100f, duration, 0, _GOverMoveIterations);
        }
    }

    public void Continue()
    {
        //here you reset player stats, or maybe fire an event called onGameReset
        //you need to restart the run with the same parameters chosen at the start like character & weapon selected
        //TimeScaleManager.ForceTimeScale(1f);
        _menuController.SwitchCurrentMenu(null);
        _inputController.ChangeInputToGameplay();
        PauseMenuManager.DisablePauseBehaviour(false);
        SceneManager.LoadScene("MainHub");

    }
    public void Restart()
    {
        _menuController.SwitchCurrentMenu(null);
        _inputController.ChangeInputToGameplay();
        PauseMenuManager.DisablePauseBehaviour(false);
        //TimeScaleManager.ForceTimeScale(1f);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        TimeScaleManager.ForceTimeScale(1f);
        SceneManager.LoadScene(0);
    }

    public void DebugPlayGameOver()
    {
        if(!_debugPlayAnimationAtStart) return;
        GameOver();
    }

    private void OnDrawGizmosSelected() {
        if(_canvas == null) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_GOverTxtInitialPos), Vector3.one * 30);
        //Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_GOverTxtEndPos), Vector3.one * 35);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_buttonsInitialPos), Vector3.one * 35);
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= CheckHealth;
        _gameFreezeTimer.onEnd -= GameOver;        
    }
}
