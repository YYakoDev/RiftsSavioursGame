using System;
using UnityEngine;

[System.Serializable]
public class UpgradeStatChange : System.Object
{
    [SerializeField]bool _usePercentage;
    [SerializeField, Range(0, 100)] int _percentage = 1;
    [SerializeField]float _flatAmount = 0f;

    public bool UsePercentage => _usePercentage;
    public float EffectAmount => _flatAmount;
    public int Percentage => _percentage;
}
