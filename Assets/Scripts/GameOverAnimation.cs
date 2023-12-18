using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverAnimation : MonoBehaviour
{
    [SerializeField] bool _debugPlayAnimationAtStart;
    [Header("References")]
    [SerializeField] Canvas _canvas;
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _visualsParent;
    [SerializeField] RectTransform _redScreenRect;
    Image _redScreenImg;
    [SerializeField] RectTransform _playerImageRect;
    Image _playerImg;
    [SerializeField] RectTransform _gameOverTextRect;
    [SerializeField] Animator _gameOverTxtAnimatorComponent;
    Image _gameOverTextImg;
    [SerializeField] RectTransform _buttons;
    TweenAnimator _redScreenAnimator;
    TweenAnimator _playerAnimator;
    TweenAnimator _GOTextAnimator;

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
    [SerializeField]int _GOverMoveIterations = 5;
    [SerializeField] float _GOVerTxtColorChangeDuration = 1f;
    [SerializeField] Vector2 _GOverTxtInitialPos;
    Vector2 _GOverTxtEndPos;


    [Header("Audio Stuff")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _gameOverSound;
    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;

        _playerStats.onStatsChange += CheckHealth;
        _redScreenImg = _redScreenRect.GetComponent<Image>();
        _playerImg = _playerImageRect.GetComponent<Image>();

        _redScreenInitialColor = _redScreenImg.color;
        _redScreenInitialColor.a = 1;
        if (_debugPlayAnimationAtStart) GameOver();
    }

    void CheckHealth()
    {
        if (_playerStats.CurrentHealth <= 0)
        {
            Debug.Log("<b>Game Over :( </b>");
            GameOver();
        }
    }

    void GameOver()
    {
        _visualsParent.SetActive(true);
        CheckAnimators();
        TimeScaleManager.ForceTimeScale(0f);
        PlayAnimations();

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
    }

    void GetAnimatorAndChangeTimeScale(RectTransform rect, ref TweenAnimator animator)
    {
        if(animator == null) animator = rect.CheckOrAddComponent<TweenAnimator>();
        animator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
    }

    void PlayAnimations()
    {
        if(_redScreenImg == null) _redScreenImg = _redScreenRect.GetComponent<Image>();
        if(_playerImg == null) _playerImg = _playerImageRect.GetComponent<Image>();
        if(_gameOverTextImg == null) _gameOverTextImg = _gameOverTextRect.GetComponent<Image>();

        _playerAnimator.Clear();
        _redScreenAnimator.Clear();
        _GOTextAnimator.Clear();

        _playerImg.sprite = _playerSprite;
        _playerImg.color = Color.white;
        Vector3 flippedScale = _playerImageRect.localScale;
        flippedScale.x = _playerRenderer.transform.localScale.x;
        _playerImageRect.localScale = flippedScale;

        Color newColor =_redScreenInitialColor;
        newColor.a = 0;
        _redScreenImg.color = newColor;

        Color newTextColor = _gameOverTextImg.color;
        newTextColor.a = 0;
        _gameOverTextImg.color = newTextColor;
        _gameOverTextRect.localPosition = _GOverTxtInitialPos;
        _gameOverTextRect.gameObject.SetActive(false);
        _GOverTxtEndPos = new Vector2(_playerImageRect.localPosition.x, _playerImageRect.localPosition.y + 200f);

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
        float duration = 0.1f + _redScreenColorChangeDuration;
        _redScreenAnimator.TweenImageColor(_redScreenRect, _redScreenInitialColor,  _playerImgEndColor, duration,
        onComplete: () => 
        {
            _gameOverTextRect.gameObject.SetActive(true);
            _gameOverTxtAnimatorComponent.enabled = true;
        });
        _playerAnimator.TweenImageColor(_playerImageRect, _playerImgEndColor,  _redScreenInitialColor, duration);

        MoveGameOverText();
        
        //_GOTextAnimator.TweenImageOpacity(_gameOverTextRect, 255, _GOVerTxtColorChangeDuration);
        _GOTextAnimator.TweenImageColor(_gameOverTextRect, _GOverTxtInitialColor, _GOVerTxtColorChangeDuration, onComplete: () => 
        {
            _gameOverTxtAnimatorComponent.Play(_GOverTxtAnimName);
        });
        _GOTextAnimator.TweenImageColor(_gameOverTextRect, _GOverTxtInitialColor, _GOverTxtEndColor, _GOVerTxtColorChangeDuration);
    }

    void MoveGameOverText(int iteration = 0)
    {
        iteration++;

        if(iteration >= _GOverMoveIterations)
        {
            _playerAnimator.MoveTo(_gameOverTextRect, _GOverTxtEndPos, _GOverTxtMovementDuration / (iteration * 3f), CurveTypes.Linear);
            return;   
        }
        int sign = (iteration % 2 == 0) ? -1 : 1;
        Vector2 offset = Vector2.right * 20 * sign;
        float timeDivision = iteration * 2f;
        _playerAnimator.MoveTo(_gameOverTextRect, _GOverTxtEndPos + offset, _GOverTxtMovementDuration / timeDivision, onComplete: () => 
        {
            MoveGameOverText(iteration);
        });
    }

    void ButtonAnimations()
    {
        //here maybe you could add the game over stats and not just the buttons
        Debug.Log("Moving Buttons");
    }

    public void RestartGame()
    {
        //here you reset player stats, or maybe fire an event called onGameReset
        //you need to restart the run with the same parameters chosen at the start like character & weapon selected
        TimeScaleManager.ForceTimeScale(1f);
        SceneManager.LoadScene("Game");

    }

    public void QuitGame()
    {
        Application.Quit();
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
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_GOverTxtEndPos), Vector3.one * 35);
    }

    private void OnDestroy() {
        _playerStats.onStatsChange += CheckHealth;        
    }
}
