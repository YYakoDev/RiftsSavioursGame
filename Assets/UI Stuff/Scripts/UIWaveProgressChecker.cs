using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class UIWaveProgressChecker : MonoBehaviour
{
    [SerializeField]World _currentWorld;
    Timer _restTimer;
    [SerializeField]Slider _slider;
    [SerializeField] Image _fillImg;
    [SerializeField] Sprite _enemyWaveImg, _restWaveImg;
    RectTransform _sliderRect;
    float _elapsedWaveTime = 0;
    TweenAnimatorMultiple _animator;

    float _currentWaveDuration = 0f;

    [Header("Animation")]
    [SerializeField] float _animDuration = 0.5f;
    [SerializeField] Vector3 _endScale = Vector3.one, _scaleOffset;

    private void Awake() {
        if(_currentWorld == null) gameObject.SetActive(false);
        _sliderRect = _slider.GetComponent<RectTransform>();
        _restTimer = new(0.1f);
        _restTimer.Stop();
        _restTimer.onEnd += SetEnemyWaveSlider;
        _animator = GetComponent<TweenAnimatorMultiple>();
        GameStateManager.OnRestStart += SetRestSlider;
        _currentWorld.OnWaveChange += SetCurrentWave;
        SetCurrentWave(_currentWorld.Waves[0]);
    }

    private void Start() {
        //_handleImg.sprite = _enemyWaveImg;
        _fillImg.color = UIColors.GetColor(UIColor.Red);
    }

    void SetRestSlider(float time)
    {
        _restTimer.ChangeTime(time);
        _restTimer.Start();
        PlayAnimation();
        //_handleImg.sprite = _restWaveImg;
        _fillImg.color = UIColors.GetColor(UIColor.Green);
        SetNewTime(time);
    }

    void SetEnemyWaveSlider()
    {
        PlayAnimation();
        //_handleImg.sprite = _enemyWaveImg;
        _fillImg.color = UIColors.GetColor(UIColor.Red);
        SetNewTime(_currentWaveDuration);
    }

    void SetNewTime(float time)
    {
        _elapsedWaveTime = 0f;
        _slider.maxValue = time;
    }

    void PlayAnimation() => _animator.TweenScaleBouncy(_sliderRect, _endScale, _scaleOffset, _animDuration, 0, 4);
    

    private void Update() {
        _restTimer.UpdateTime();
        _elapsedWaveTime += Time.deltaTime;
        _slider.value = _currentWaveDuration - _elapsedWaveTime;
    }

    void SetCurrentWave(SOEnemyWave newWave)
    {
        _currentWaveDuration = newWave.WaveDuration;
        SetNewTime(newWave.WaveDuration);
    }
    

    private void OnDestroy() {
        _restTimer.onEnd -= SetEnemyWaveSlider;
        GameStateManager.OnRestStart -= SetRestSlider;
        _currentWorld.OnWaveChange -= SetCurrentWave;
    }
}

