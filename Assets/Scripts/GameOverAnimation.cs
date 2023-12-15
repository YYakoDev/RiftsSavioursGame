using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverAnimation : MonoBehaviour
{
    [SerializeField] bool _debugPlayAnimationAtStart;
    [Header("References")]
    [SerializeField] SOPlayerStats _playerStats;
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _visualsParent;
    [SerializeField] Image _redScreen;
    RectTransform _redScreenRect;
    [SerializeField] Image _playerImage;
    RectTransform _playerImgRect;
    [SerializeField] RectTransform _buttons;
    TweenAnimator _redScreenAnimator;
    TweenAnimator _playerAnimator;


    [Header("Player Image Animation")]
    [SerializeField] Color _endColor;
    [SerializeField] float _playerAnimDuration = 1f;
    SpriteRenderer _playerRenderer;
    Sprite _playerSprite;

    [Header("Red Screen Animation")]

    [SerializeField] float _redScreenFadeDuration = 0.56f;
    Color _redScreenInitialColor;
    [SerializeField] float _redScreenColorChangeDuration = 0.2f;
    [SerializeField] Color[] _redScreenBlinkColors;

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
        _redScreenInitialColor = _redScreen.color;
        _redScreenInitialColor.a = 1;

        _redScreenRect = _redScreen.rectTransform;
        _playerImgRect = _playerImage.rectTransform;

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
        CheckRedScreen();
        CheckPlayerSprite();
        TimeScaleManager.ForceTimeScale(0f);
        PlayAnimations();

    }

    void CheckRedScreen()
    {
        if(_redScreenAnimator == null)
        {
            Debug.Log("redscreenanimator is null");
            _redScreenAnimator = _redScreen.CheckOrAddComponent<TweenAnimator>();
            _redScreenAnimator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        }
    }

    void CheckPlayerSprite()
    {
        if (_playerRenderer == null)
        {
            _playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SpriteRenderer>();
        }
        _playerSprite = _playerRenderer.sprite;
        if (_playerAnimator == null)
        {
            _playerAnimator = _playerImage.CheckOrAddComponent<TweenAnimator>();
            _playerAnimator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        }
    }

    void PlayAnimations()
    {
        _playerAnimator.Clear();
        _redScreenAnimator.Clear();

        _playerImage.sprite = _playerSprite;
        _playerImage.color = Color.white;
        Vector3 flippedScale = _playerImgRect.localScale;
        flippedScale.x = _playerRenderer.transform.localScale.x;
        _playerImgRect.localScale = flippedScale;

        Color newColor =_redScreenInitialColor;
        newColor.a = 0;
        _redScreen.color = newColor;



        _playerAnimator.TweenImageColor(_playerImgRect, _endColor, _playerAnimDuration);
        _redScreenAnimator.TweenImageOpacity(_redScreenRect, 255, _redScreenFadeDuration, onComplete: () => 
        {
            for (int i = 0; i < _redScreenBlinkColors.Length; i++)
            {
                var initialColor = (i == 0) ?  _redScreenInitialColor : _redScreenBlinkColors[i-1];
                _redScreenAnimator.TweenImageColor
                (_redScreenRect, initialColor, _redScreenBlinkColors[i], _redScreenColorChangeDuration);
            }
        });
        //this is a fake animation that is used to know when the color blinking anim has completed
        _redScreenAnimator.TweenImageOpacity(_redScreenRect, 255, 0, onComplete: PlayRestOfAnimations);
    }

    void PlayRestOfAnimations()
    {
        float duration = 0.1f + _redScreenColorChangeDuration;
        _redScreenAnimator.TweenImageColor(_redScreenRect, _redScreenInitialColor,  _endColor, duration);
        _playerAnimator.TweenImageColor(_playerImgRect, _endColor,  _redScreenInitialColor, duration - 0.1f);
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

    private void OnDestroy() {
        _playerStats.onStatsChange += CheckHealth;        
    }
}
