using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeUILevelUpSys : MonoBehaviour
{
    [SerializeField] Slider _xpSlider;
    [SerializeField] PrototypeLevelUpSystem _levelUpSys;
    private void Start() {
        UpdateMax();
        _levelUpSys.OnLevelUp += UpdateMax;
        _levelUpSys.OnXPChange += ChangeXP;
    }

    void ChangeXP()
    {
        var value = _levelUpSys.XPAmount;
        _xpSlider.value = value;
        if(value >= _levelUpSys.LevelUpThreshold)
        {
            //make the slider glow, do a little shake animation maybe on loop and play a sound cue
        }
    }

    void UpdateMax()
    {
        _xpSlider.maxValue = _levelUpSys.LevelUpThreshold;
    }

    private void OnDestroy() {
        _levelUpSys.OnXPChange -= ChangeXP;
        _levelUpSys.OnLevelUp -= UpdateMax;
    }
}
