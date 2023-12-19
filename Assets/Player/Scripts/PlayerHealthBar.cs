using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]SOPlayerStats _playerStats;
    [SerializeField]PlayerHealthAnimations _animations;
    Slider _healthBar;

    int currentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            UpdateBarValue();
        }
    }
    int maxHealth 
    {   
        get => maxHealth; 
        set 
        {
            maxHealth = value;
            UpdateBarMaxValue();
        }
    }


    private void Awake() {
        GameObject thisGO = gameObject;
        thisGO.SetActive(_playerStats != null);
        thisGO.CheckComponent<Slider>(ref _healthBar);

    }

    private IEnumerator Start()
    {
        //UpdateHealthBar();
        yield return null;
        yield return null;
        _playerStats.onStatsChange += CheckValues;
    }

    void UpdateBarValue()
    {
        _healthBar.value = currentHealth;
        PlayAnimations();
    }
    void UpdateBarMaxValue()
    {
        _healthBar.maxValue = maxHealth;
        _animations.ShakeAnimation();
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

    void CheckValues()
    {
        if(currentHealth != _playerStats.CurrentHealth) currentHealth = _playerStats.CurrentHealth;
        if(maxHealth != _playerStats.MaxHealth) maxHealth = _playerStats.MaxHealth;
    }

    void PlayAnimations()
    {
        _animations.Stop();
        CameraShake.Shake(1f);
        _animations.BlinkBarAnim();
        _animations.ShakeAnimation();
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= CheckValues;
    }
    
}
