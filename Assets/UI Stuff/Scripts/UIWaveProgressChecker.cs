using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UIWaveProgressChecker : MonoBehaviour
{
    [SerializeField] WaveSystem _waveSys;
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    [SerializeField] DifficultyScaler _difficultyScaler;
    [SerializeField] DifficultyTiers _difficultyTiers;
    [SerializeField]Slider _slider;
    [SerializeField] Image _fillImg, _barIcon;
    [SerializeField] Sprite _restImg;
    [SerializeField] TextMeshProUGUI _waveCountText, _difficultyLevelText, _timeText;
    SOEnemyWave _currentWave;
    RectTransform _sliderRect;
    int _waveCount = 0, _playerWaveKillCount;
    TweenAnimatorMultiple _animator;

    [Header("Animation")]
    [SerializeField] float _animDuration = 0.5f;
    [SerializeField] Vector3 _endScale = Vector3.one, _scaleOffset;
    float _timeUpdates = 0.9f, _nextTimeUpdate;

    private void Awake() {
        _sliderRect = _slider.GetComponent<RectTransform>();
        _animator = GetComponent<TweenAnimatorMultiple>();
    }

    private void Start() {
        EnemyBrain.OnEnemyDeath += UpdateKillCount;
        _waveSys.OnWaveChange += NextWaveCheck;
        _difficultyScaler.OnDifficultyIncrease += UpdateDifficulty;
        _waveCount = 0;
        _slider.wholeNumbers = false;
        _waveCountText.SetText($"CURRENT WAVE: {_waveCount}");
        UpdateDifficulty();
    }

    void NextWaveCheck(SOEnemyWave wave)
    {
        Debug.Log("New wave!");
        _currentWave = wave;
        _playerWaveKillCount = 0;
        AdvanceWaveCount();
        SetEnemyWaveSlider();
        /*else if(state.GetType() == typeof(RestState))
        {
            var RestState = (RestState)state;
            _currentWaveDuration = RestState.CountdownTime;
            _barIcon.sprite = _restImg;
            _waveCountText.SetText("RESTING");
        }*/
    }

    void UpdateKillCount()
    {
        _playerWaveKillCount = _waveSpawner.PlayerKills;
        _slider.value = _waveSpawner.MaxEnemiesToKill - _playerWaveKillCount;
    }

    void AdvanceWaveCount()
    {
        _waveCount++;
        _waveCountText.SetText($"CURRENT WAVE: {_waveCount}");
    }

    void SetEnemyWaveSlider()
    {
        _slider.maxValue = _waveSpawner.MaxEnemiesToKill;
        _slider.value = _waveSpawner.MaxEnemiesToKill;
        PlayAnimation();
    }

    void UpdateDifficulty()
    {
        //maybe play a little sound?
        _barIcon.sprite = _difficultyTiers.CurrentTier.Icon;
        _animator.TweenScaleBouncy(_barIcon.rectTransform, _endScale, _scaleOffset, _animDuration / 2f, 0, 6, CurveTypes.EaseOutCirc);
        _difficultyLevelText.SetText(_difficultyTiers.CurrentTier.Name);
    }

    void PlayAnimation() => _animator.TweenScaleBouncy(_sliderRect, _endScale, _scaleOffset, _animDuration, 0, 4);
    

    private void Update() {
        
        if(_nextTimeUpdate > Time.time) return;
        var minutes = Mathf.FloorToInt(GameTime.RunTime/60);
        var seconds = Mathf.FloorToInt(GameTime.RunTime % 60);
        var time = string.Format("{0:00}:{1:00}",minutes,seconds);
        _timeText.SetText(time);
        _nextTimeUpdate = Time.time + _timeUpdates;

        //if(!_waveSys.Enabled) return;
    }
    private void OnDestroy() {
        EnemyBrain.OnEnemyDeath -= UpdateKillCount;
        _waveSys.OnWaveChange -= NextWaveCheck;
        _difficultyScaler.OnDifficultyIncrease -= UpdateDifficulty;
    }
}

