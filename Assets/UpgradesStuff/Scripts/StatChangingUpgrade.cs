using System;
using UnityEngine;


[CreateAssetMenu(menuName = MenuPath + "StatChangingUpgrade")]
public class StatChangingUpgrade : SOUpgradeBase
{
    [SerializeField] StatsTypes[] _statsToModify;
    [SerializeField] StatModificationValue[] _modifications = new StatModificationValue[0];
    public override void SetGroup(UpgradeGroup group)
    {
        SetDescription();
        base.SetGroup(group);
    }

    void SetDescription()
    {
        for (int i = 0; i < _statsToModify.Length; i++)
        {
            var modificationValue = _modifications[i];

            bool positiveUpgrade = modificationValue.UsePercentage ? modificationValue.Percentage >= 0 : modificationValue.Addition >= 0;
            string colorTag = (positiveUpgrade) ? $"<color={UIColors.GetHexColor(UIColor.Green)}>" : $"<color={UIColors.GetHexColor(UIColor.Red)}>" ;
            string text = (positiveUpgrade) ? $"{colorTag} Increases </color>": $"{colorTag} Decreases </color>";
            string fullText = modificationValue.UsePercentage ? $"{text} your {_statsToModify[i].ToString()} by {modificationValue.Percentage}%" : $"{text} your {_statsToModify[i].ToString()} by {modificationValue.Addition}";
            if(i != _statsToModify.Length -1)_description += fullText + "\n";
            else _description += fullText;

        }
    }

    public override void ApplyEffect(PlayerUpgradesManager upgradesManager)
    {
        base.ApplyEffect(upgradesManager);
        var statsManager = upgradesManager.Player.StatsManager;

        for (int i = 0; i < _statsToModify.Length; i++)
        {
            var statType = _statsToModify[i];
            float stat = statsManager.GetStat(statType);
            if(stat == -1f)
            {
                Debug.LogError($"The stat type: {statType} is not present in the StatsManager");
                continue;
            }
            var modificationValue = _modifications[i];
            float newValue = (modificationValue.UsePercentage) ? 
            upgradesManager.StatUp(stat, modificationValue.Percentage) : upgradesManager.StatUp(stat, modificationValue.Addition);

            statsManager.SetStat(statType, newValue);
        }

    }

    private void OnValidate() {
        if(_statsToModify == null) return;
        if(_modifications == null) return;
        if(_modifications.Length != _statsToModify.Length) Array.Resize<StatModificationValue>(ref _modifications, _statsToModify.Length);

        for (int i = 0; i < _statsToModify.Length; i++)
        {
            if(i >= _modifications.Length) break;
            if(_modifications[i] == null) continue;
            var newName = _statsToModify[i].ToString();
            _modifications[i].StatToModifyName = newName;
        }
    }

}