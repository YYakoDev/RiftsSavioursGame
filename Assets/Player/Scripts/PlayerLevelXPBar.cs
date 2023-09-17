using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelXPBar : MonoBehaviour
{
    [SerializeField]SOPlayerStats _playerStats;
    Slider _xpBar;
    int _currentLevel;
    [SerializeField]TextMeshProUGUI t_levelNumber;
    private void Awake() {
        
        gameObject.SetActive(_playerStats != null);
        gameObject.CheckComponent<Slider>(ref _xpBar); _xpBar.wholeNumbers = true;
        if(t_levelNumber == null) t_levelNumber = GetComponentInChildren<TextMeshProUGUI>();
        
        _playerStats.onStatsChange += UpdateXPBar;
        UpdateXPBar();
    }


    void UpdateXPBar()
    {
        if(_xpBar.value != _playerStats.CurrentXP)
        {
            _xpBar.value = _playerStats.CurrentXP;
        }
        if(_xpBar.maxValue != _playerStats.XPToNextLevel)
        {
            _xpBar.maxValue = _playerStats.XPToNextLevel;
        }
        
        if(_currentLevel != _playerStats.Level)
        {
            _currentLevel = _playerStats.Level;
            UpdateLevelText();
        }
    }

    void UpdateLevelText()
    {   
        t_levelNumber.text = _currentLevel.ToString();
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= UpdateXPBar;
    }
}
