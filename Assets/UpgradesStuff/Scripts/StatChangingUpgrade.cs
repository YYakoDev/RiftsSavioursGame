using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;


[CreateAssetMenu(menuName = MenuPath + "StatChangingUpgrade")]
public class StatChangingUpgrade : SOUpgradeBase
{
    [SerializeField]protected PlayerStatsBase _stats;
    protected IEnumerable<PropertyInfo> _queryResult;
    protected PropertyInfo[] _queryPropertiesArray = new PropertyInfo[0];
    [SerializeField, ReadOnly]protected int[] _indexes;
    [SerializeField]protected UpgradeStatChange[] _statsChanges;

    [SerializeField, HideInInspector]public int ElementCount;

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
    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        if(_queryResult == null) SearchVariables();
        else SetArray();
        for (int i = 0; i < _indexes.Length; i++)
        {
            int index = _indexes[i];
            if(index >= _queryPropertiesArray.Length) continue;
            UpgradeStatChange statChange = _statsChanges[i];
            var property = _queryPropertiesArray[index];
            object currentValue = property.GetValue(_stats);
            float newValue = 0;
            var currentValueAsFloat = (float)Convert.ChangeType(currentValue, newValue.GetType());
            newValue = (statChange.UsePercentage) ?
            upgradesManager.StatUp(currentValueAsFloat, statChange.Percentage) : upgradesManager.StatUp(currentValueAsFloat, statChange.EffectAmount);
            property.SetValue(_stats, Convert.ChangeType(newValue, currentValue.GetType()));
        }
    }
}
