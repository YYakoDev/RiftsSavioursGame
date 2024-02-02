using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;


[CreateAssetMenu(menuName = MenuPath + "StatChangingUpgrade")]
public class StatChangingUpgrade : SOUpgradeBase
{
    [SerializeField]protected PlayerStatsBase _stats;
    protected IEnumerable<PropertyInfo> _queryResult;
    protected PropertyInfo[] _queryPropertiesArray = new PropertyInfo[0];
    [SerializeField, ReadOnly]protected int[] _indexes;
    [SerializeField]protected UpgradeStatChange[] _statsChanges;
    [SerializeField, HideInInspector]public int ElementCount;
    public int[] Indexes => _indexes;

    //SAVED STUFF
    [SerializeField, ReadOnly]protected PropertyInfo[] _savedProperties;
    [SerializeField, ReadOnly]protected string _savedText;

    public override void SetGroup(UpgradeGroup group)
    {
        base.SetGroup(group);
        QueryCheck();
        if(_savedText == null || _savedText == string.Empty) SaveText();
        ReadSavedText();
    }

    void SaveText()
    {
        QueryCheck();
        _savedText = string.Empty;
        for (int i = 0; i < _indexes.Length; i++)
        {
            int index = _indexes[i];
            var propertyName = _queryPropertiesArray[index].Name;
            if(i == _indexes.Length -1) _savedText += propertyName;
            else _savedText += propertyName + ",";
        }
        //Debug.Log("Saved text from upgrade:   " + Name + "  SavedContent:  " + _savedText);
    }

    void ReadSavedText()
    {
        QueryCheck();
        string[] propertiesName = _savedText.Split(",");
        if(_savedProperties == null || _savedProperties.Length != _indexes.Length) Array.Resize<PropertyInfo>(ref _savedProperties, _indexes.Length);
        for (int i = 0; i < propertiesName.Length; i++)
        {
            for (int j = 0; j < _queryPropertiesArray.Length; j++)
            {
                if(_queryPropertiesArray[j].Name == propertiesName[i])
                {
                    _savedProperties[i] = _queryPropertiesArray[j];
                    _indexes[i] = j;
                    break;
                }
            }
        }
    }

    public virtual PropertyInfo[] SearchVariables()
    {
        if(_stats == null) return null;
        Type type = _stats.GetType();
        var rawProperties = type.GetProperties();
        _queryResult =
            from property in rawProperties
            where (property.CanWrite)
            where (property.DeclaringType == type)
            select property;
        return SetArray();

    }
    public void SetIndexes(int[] indexes)
    {
        _indexes = indexes;
        Array.Resize<UpgradeStatChange>(ref _statsChanges, _indexes.Length);
        SaveText();
    }

    protected virtual PropertyInfo[] SetArray()
    {
        int i = 0;
        int count = _queryResult.Count();
        if (count != _queryPropertiesArray.Length) Array.Resize<PropertyInfo>(ref _queryPropertiesArray, count);
        foreach (var item in _queryResult)
        {
            _queryPropertiesArray[i] = item;
            i++;
        }
        return _queryPropertiesArray;
    }

    protected void QueryCheck()
    {
        if(_queryPropertiesArray == null || _queryPropertiesArray.Length == 0)
        {
            if(_queryResult == null) SearchVariables();
            else SetArray();
        }
    }
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        for (int i = 0; i < _savedProperties.Length; i++)
        {
            int index = _indexes[i];
            UpgradeStatChange statChange = _statsChanges[i];
            var property = _savedProperties[i];
            ApplyValueToProperty(property, statChange, upgradesManager);
        }
        
    }
    protected void ApplyValueToProperty
    (PropertyInfo property, UpgradeStatChange statChange, PlayerUpgradesManager upgradesManager, PlayerStatsBase stats = null)
    {
        if(stats == null) stats = _stats;
        object currentValue = property.GetValue(stats);
        float newValue = 0;
        var currentValueAsFloat = (float)Convert.ChangeType(currentValue, newValue.GetType());
        newValue = (statChange.UsePercentage) ?
        upgradesManager.StatUp(currentValueAsFloat, statChange.Percentage) : upgradesManager.StatUp(currentValueAsFloat, statChange.EffectAmount);
        property.SetValue(stats, Convert.ChangeType(newValue, currentValue.GetType()));
    }


    public void TryReadText()
    {
        if(_savedText == null || _savedText == string.Empty)
        {
            Debug.Log("Saved text was empty at the upgrade:   " + Name);
            return;
        }
        ReadSavedText();
    }

    public void SaveTextAndRead()
    {
        SaveText();
        ReadSavedText();
    }

    private void OnValidate() {
        TryReadText();
    }
}
