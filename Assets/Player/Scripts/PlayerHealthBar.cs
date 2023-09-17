using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]SOPlayerStats _playerStats;
    Slider _healthBar;
    private void Awake() {
        
        gameObject.SetActive(_playerStats != null);

        gameObject.CheckComponent<Slider>(ref _healthBar);
        _playerStats.onStatsChange += UpdateHealthBar;
        
        UpdateHealthBar();

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
        }
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= UpdateHealthBar;
    }
    
}
