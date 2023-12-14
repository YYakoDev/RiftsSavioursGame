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
    [SerializeField] Image _redScreen;
    [SerializeField] Image _playerImage;
    TweenAnimator _redScreenAnimator;
    TweenAnimator _playerAnimator;


    [Header("Player Image Animation")]
    [SerializeField] Color _endColor;
    [SerializeField] float _playerAnimDuration = 1f;
    SpriteRenderer _playerRenderer;
    Sprite _playerSprite;

    [Header("Red Screen Animation")]

    [SerializeField] float _redScreenFadeDuration = 0.56f;
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

        if (_debugPlayAnimationAtStart) GameOver();
    }

    void CheckHealth()
    {
        if (_playerStats.CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        CheckRedScreen();
        CheckPlayerSprite();
        TimeScaleManager.ForceTimeScale(0f);
        PlayAnimations();

    }

    void CheckRedScreen()
    {
        if (_redScreenAnimator == null)
        {
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

        RectTransform playerImgRect = _playerImage.rectTransform;
        _playerImage.sprite = _playerSprite;
        _playerImage.color = Color.white;
        Vector3 flippedScale = playerImgRect.localScale;
        flippedScale.x = _playerRenderer.transform.localScale.x;
        playerImgRect.localScale = flippedScale;

        RectTransform redScreenRect = _redScreen.rectTransform;
        Color newColor = _redScreen.color;
        newColor.a = 0;
        _redScreen.color = newColor;



        //_playerAnimator.TweenImageColor(playerImgRect, _endColor, _playerAnimDuration);
        _redScreenAnimator.TweenImageOpacity(redScreenRect, 255, _redScreenFadeDuration);
        foreach (Color color in _redScreenBlinkColors)
        {
            _redScreenAnimator.TweenImageColor(redScreenRect, color, _redScreenColorChangeDuration, CurveTypes.EaseInOut);
        }
        //this is a fake animation that is used to know when the color blinking anim has completed
        _redScreenAnimator.TweenImageOpacity(redScreenRect, 255, 0, onComplete: PlayButtonAnimations);
    }

    public void PlayButtonAnimations()
    {
        Debug.Log("Moving Buttons");
    }

    public void RestartGame()
    {
        //here you reset player stats, or maybe fire an event called onGameReset
        //you need to restart the run with the same parameters chosen at the start like character & weapon selected
        SceneManager.LoadScene("Game");

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
