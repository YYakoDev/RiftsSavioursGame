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
        _playerStats.onStatsChange += UpdateHealthBar;
        UpdateHealthBar();

    }


    void UpdateHealthBar()
    {
        if(_healthBar.value != _playerStats.CurrentHealth)
        {
            _healthBar.value = _playerStats.CurrentHealth;
            //_animations.ShakeAnimation();
            _animations.BlinkBarAnim();
        }
        if(_healthBar.maxValue != _playerStats.MaxHealth)
        {
            _healthBar.maxValue = _playerStats.MaxHealth;
        }
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= UpdateHealthBar;
    }
    
}
