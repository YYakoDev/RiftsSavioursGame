using UnityEngine;

[System.Serializable]
public class StatModificationValue
{
    [HideInInspector]public string StatToModifyName = "";
    [SerializeField] bool _usePercentage = false;
    [SerializeField] float _addition;
    [SerializeField, Range(-100,100)] int _percentage;

    public bool UsePercentage => _usePercentage;
    public float Addition => _addition;
    public int Percentage => _percentage;
}
