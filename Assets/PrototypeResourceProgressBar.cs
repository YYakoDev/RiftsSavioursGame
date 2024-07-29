using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeResourceProgressBar : MonoBehaviour
{
    Slider _slider;
    float _previousValue = 0f, _currentValue = 0f, _newValue = 0f;
    float _elapsedTime;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.minValue = 0;
        _slider.maxValue = 100;
        _slider.value = 0;
    }

    private void Update() {
        if(_currentValue != _newValue)
        {
            _elapsedTime += 2f * Time.deltaTime;
            _currentValue = Mathf.Lerp(_currentValue, _newValue, _elapsedTime);
            _slider.value = _currentValue;
        }
    }

    public void SetValue(int resourceMaxHealth, int resourceCurrentHealth)
    {
        var result = (resourceCurrentHealth * 100f) / resourceMaxHealth;
        _slider.value = result;
    }

    public bool SetValue(Resource resource)
    {
        var damage = 40f - resource.MaxHealth * 2f;
        //_previousValue = _currentValue;
        _elapsedTime = 0f;
        _newValue += damage;
        var percent = _newValue / 100f;
        return percent >= 1f;
    }
}
