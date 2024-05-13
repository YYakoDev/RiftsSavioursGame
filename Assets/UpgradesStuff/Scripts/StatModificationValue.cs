using UnityEngine;

[System.Serializable]
public class StatModificationValue
{
    public StatModificationValue(bool usePercentage, float addition, int percentage)
    {
        _usePercentage = usePercentage;
        _addition = addition;
        _percentage = percentage;
    }
    [HideInInspector]public string StatToModifyName = "";
    [SerializeField] bool _usePercentage = false;
    [SerializeField] float _addition;
    [SerializeField, Range(-100,100)] int _percentage;

    public bool UsePercentage => _usePercentage;
    public float Addition => _addition;
    public int Percentage => _percentage;

    public void ChangePercentage(int newPercentage)
    {
        _percentage = newPercentage;
    }
    public void ChangeAddition(int newAddition)
    {
        _addition = newAddition;
    }
}
