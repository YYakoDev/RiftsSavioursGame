using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UIWaveProgressChecker : MonoBehaviour
{
    [SerializeField]Slider _slider;
    [SerializeField] Image _fillImg, _barIcon;
    [SerializeField] Sprite _enemyWaveImg, _restImg;
    RectTransform _sliderRect;
    Timer _waveTimer;
    float _currentWaveDuration = 0f;
    bool _timerIsRunning = false;
    TweenAnimatorMultiple _animator;

    [Header("Animation")]
    [SerializeField] float _animDuration = 0.5f;
    [SerializeField] Vector3 _endScale = Vector3.one, _scaleOffset;

    private void Awake() {
        _sliderRect = _slider.GetComponent<RectTransform>();
        _waveTimer = new Timer(_currentWaveDuration);
        _waveTimer.Stop();
        _animator = GetComponent<TweenAnimatorMultiple>();
        GameStateManager.OnStateSwitch += StateSwitchCheck;
    }

    private void Start() {
        //_handleImg.sprite = _enemyWaveImg;
        //_fillImg.color = UIColors.GetColor(UIColor.Red);
        _slider.value = 0f;
        _slider.maxValue = 1f;
        _slider.wholeNumbers = false;
        StateSwitchCheck(GameStateManager.CurrentState);
    }

    void StateSwitchCheck(GameStateBase state)
    {
        
        if(state.GetType() == typeof(ConvergenceState))
        {
            var ConvergenceState = (ConvergenceState)state;
            _currentWaveDuration = ConvergenceState.CountdownTime;
            _barIcon.sprite = _enemyWaveImg;
        }
        else if(state.GetType() == typeof(RestState))
        {
            var RestState = (RestState)state;
            _currentWaveDuration = RestState.CountdownTime;
            _barIcon.sprite = _restImg;
        }
        SetEnemyWaveSlider(_currentWaveDuration);
    }
    void SetEnemyWaveSlider(float time)
    {
        PlayAnimation();
        //_handleImg.sprite = _enemyWaveImg;
        //_fillImg.color = UIColors.GetColor(UIColor.Red);
        SetNewTime(time);
    }

    void SetNewTime(float time)
    {
        Debug.Log("New wave progress timer:  " + time);
        _waveTimer.ChangeTime(time);
        _waveTimer.Start();
        _timerIsRunning = true;
    }

    void PlayAnimation() => _animator.TweenScaleBouncy(_sliderRect, _endScale, _scaleOffset, _animDuration, 0, 4);
    

    private void Update() {
        _waveTimer.UpdateTime();
        if(_timerIsRunning)
        {
            var percentage = _waveTimer.CurrentTime / _currentWaveDuration;
            _slider.value = percentage;
        }
    }
    private void OnDestroy() {
        GameStateManager.OnStateSwitch -= StateSwitchCheck;
    }
}

