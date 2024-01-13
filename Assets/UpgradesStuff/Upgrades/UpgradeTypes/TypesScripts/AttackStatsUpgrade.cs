using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "AttackStatsUpgrade")]
public class AttackStatsUpgrade : StatChangingUpgrade
{
    [SerializeField]SOPlayerAttackStats _attackStats;
    PlayerStatsBase[] _statsArray;
    [SerializeField, HideInInspector]int _statsPropertiesCount = 0;

    void Initialize()
    {
        _statsArray = new PlayerStatsBase[2]
        {
            _stats,
            _attackStats
        };
        var query =
        from property in _statsArray[0].GetType().GetProperties()
        where property.CanWrite
        where (property.DeclaringType == _statsArray[0].GetType())
        select property;

        _statsPropertiesCount = query.Count();
    }

    public override PropertyInfo[] SearchVariables()
    {
        if(_stats == null || _attackStats == null) return null;
        Initialize();
        _queryResult =
        from stat in _statsArray
        from property in stat.GetType().GetProperties()
        where (property.CanWrite)
        where (property.DeclaringType == stat.GetType())
        select property;
        return SetArray();
    }

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        _parent.AdvanceIndex();
        if(_queryResult == null) SearchVariables();
        else SetArray();
        for (int i = 0; i < _indexes.Length; i++)
        {
            int index = _indexes[i];
            if(index >= _queryPropertiesArray.Length) continue;
            PlayerStatsBase SOStats = (index >= _statsPropertiesCount) ? _statsArray[1] : _statsArray[0];

            UpgradeStatChange statChange = _statsChanges[i];
            var property = _queryPropertiesArray[index];
            object currentValue = property.GetValue(SOStats);
            float newValue = 0;
            var currentValueAsFloat = (float)Convert.ChangeType(currentValue, newValue.GetType());
            newValue = (statChange.UsePercentage) ?
            upgradesManager.StatUp(currentValueAsFloat, statChange.Percentage) : upgradesManager.StatUp(currentValueAsFloat, statChange.EffectAmount);
            property.SetValue(SOStats, Convert.ChangeType(newValue, currentValue.GetType()));
        }
    }
}
