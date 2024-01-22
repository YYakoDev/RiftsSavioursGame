using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField]SOPlayerStats _playerStats;
    [SerializeField]PlayerHealthAnimations _animations;
    Slider _healthBar;
    int _currentHealth, _maxHealth;

    int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            UpdateBarValue();
        }
    }
    int MaxHealth 
    {   
        get => _maxHealth; 
        set 
        {
            _maxHealth = value;
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
        _currentHealth = _playerStats.CurrentHealth;
        _maxHealth = _playerStats.MaxHealth;
        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _currentHealth;
        //UpdateHealthBar();
        yield return null;
        yield return null;
        _playerStats.onStatsChange += CheckValues;
    }

    void UpdateBarValue()
    {
        _healthBar.value = _currentHealth;
        PlayAnimations();
    }
    void UpdateBarMaxValue()
    {
        _healthBar.maxValue = _maxHealth;
        _animations.ShakeAnimation();
    }

    void CheckValues()
    {
        if(MaxHealth != _playerStats.MaxHealth) MaxHealth = _playerStats.MaxHealth;
        if(CurrentHealth != _playerStats.CurrentHealth) CurrentHealth = _playerStats.CurrentHealth;
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
