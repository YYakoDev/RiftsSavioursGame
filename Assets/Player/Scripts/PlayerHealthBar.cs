using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]SOPlayerStats _playerStats;
    [SerializeField]PlayerHealthAnimations _animations;
    Slider _healthBar;
    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.SetActive(_playerStats != null);
        thisGO.CheckComponent<Slider>(ref _healthBar);

    }

    private IEnumerator Start()
    {
        UpdateHealthBar();
        yield return null;
        yield return null;
        _playerStats.onStatsChange += UpdateAndDoAnimations;
    }

    void UpdateHealthBar()
    {
        if(_healthBar.value != _playerStats.CurrentHealth)
        {
            _healthBar.value = _playerStats.CurrentHealth;
        }
        if(_healthBar.maxValue != _playerStats.MaxHealth)
        {
            _healthBar.maxValue = _playerStats.MaxHealth;
            _animations.ShakeAnimation();
        }
    }

    void UpdateAndDoAnimations()
    {
        if(_healthBar.value != _playerStats.CurrentHealth)
        {
            PlayAnimations();
            _healthBar.value = _playerStats.CurrentHealth;
        }
        if(_healthBar.maxValue != _playerStats.MaxHealth)
        {
            _healthBar.maxValue = _playerStats.MaxHealth;
            _animations.ShakeAnimation();
        }
    }

    void PlayAnimations()
    {
        _animations.Stop();
        CameraShake.Shake(0.5f);
        _animations.BlinkBarAnim();
        _animations.ShakeAnimation();
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= UpdateAndDoAnimations;
    }
    
}
