using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIconAnimation : MonoBehaviour
{
    [SerializeField] Image _characterIcon;
    [SerializeField] float _animDuration;
    Timer _animTimer;
    [SerializeField] Material _uiDefaultMat, _whiteBlinkMat;
    [SerializeField] SOPlayerStats _playerStats;
    int _health;

    private void Awake() {
        _animTimer = new(_animDuration);
        _animTimer.Stop();
        _animTimer.onEnd += ResetMaterial;
    }

    private void Start() {
        _playerStats.onStatsChange += CheckPlayerHealth;
    }

    private void Update() {
        _animTimer.UpdateTime();
    }

    void CheckPlayerHealth()
    {
        var currentHealth = _playerStats.CurrentHealth;
        if(_health != currentHealth)
        {
            if(_health > currentHealth)
            {
                DoAnimation();
            }
            _health = currentHealth;
        }
    }

    void DoAnimation()
    {
        _characterIcon.material = _whiteBlinkMat;
        _animTimer.Start();
    }

    void ResetMaterial()
    {
        _characterIcon.material = _uiDefaultMat;
    }

    private void OnDestroy() {
        _playerStats.onStatsChange -= CheckPlayerHealth;
        _animTimer.onEnd -= ResetMaterial;        
    }
}
