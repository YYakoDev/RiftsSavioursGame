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
    [SerializeField] TweenAnimator _redScreenAnimator;
    [SerializeField] TweenAnimator _playerAnimator;

    [Header("Player Image Animation")]
    [SerializeField] Color _endColor;
    [SerializeField] float _playerAnimDuration = 1f;
    SpriteRenderer _playerRenderer;
    Sprite _playerSprite;

    [Header("Red Screen Animation")]
    [SerializeField, Range(0, 255)] int _endOpacity = 255;
    [SerializeField] float _redScreenAnimDuration = 1f;
    
    
    private void Start() {
        if(_mainCamera == null) _mainCamera = Camera.main;

        _playerStats.onStatsChange += CheckHealth;

        if(_debugPlayAnimationAtStart) GameOver();
    }
    
    void CheckHealth()
    {
        if(_playerStats.CurrentHealth <= 0)
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
        if(_redScreenAnimator == null)
        {
            _redScreenAnimator = _redScreen.CheckOrAddComponent<TweenAnimator>();
            _redScreenAnimator.ChangeTimeScalingUsage(TweenAnimator.TimeUsage.UnscaledTime);
        }
    }

    void CheckPlayerSprite()
    {
        if(_playerRenderer == null)
        {
            _playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SpriteRenderer>();
        }
        _playerSprite = _playerRenderer.sprite;
        if(_playerAnimator == null)
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
        Color color = _redScreen.color;
        color.a = 0;
        _redScreen.color = color;

        _playerAnimator.TweenImageColor(_playerImage.rectTransform, _endColor, _playerAnimDuration, CurveTypes.EaseInOut);
        _redScreenAnimator.TweenImageOpacity(_redScreen.rectTransform, _endOpacity, _redScreenAnimDuration, CurveTypes.EaseInOut);
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
