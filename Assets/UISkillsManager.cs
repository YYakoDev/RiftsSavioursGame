using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillsManager : MonoBehaviour
{
    [SerializeField] UISkill _uiItemPrefab;
    [SerializeField] Transform _skillsParent;
    UISkill[] _skillsIntances = new UISkill[0];

    public int CreateNewSkill(KeyInputTypes inputType, Sprite skillIcon)
    {
        if(SearchIfItExist(inputType)) return -1;
        UISkill skillItem = Instantiate(_uiItemPrefab);
        var index = AddItemInstance(skillItem);
        skillItem.Initialize(inputType, skillIcon);
        return index;
    }
    public int CreateNewSkill(KeyInputTypes inputType, Sprite skillIcon,  float cooldown)
    {
        if(SearchIfItExist(inputType)) return -1;
        UISkill skillItem = Instantiate(_uiItemPrefab);
        var index = AddItemInstance(skillItem);
        skillItem.Initialize(inputType, skillIcon, cooldown);
        return index;
    }

    public bool SearchIfItExist(KeyInputTypes type)
    {
        bool result = false;
        foreach(UISkill item in _skillsIntances)
        {
            result = (item.InputType == type);
            if(result) break;
        }
        return result;
    }

    public UISkill GetSkillItem(int index)
    {
        return _skillsIntances[index];
    }
    public void UpdateSkillSprite(int skillIndex, Sprite icon)
    {
        _skillsIntances[skillIndex].SetSkillIcon(icon);
    }

    public void UpdateSkillCooldown(int skillIndex, float cooldown)
    {
        _skillsIntances[skillIndex].SetCooldown(cooldown);
    }

    int AddItemInstance(UISkill item)
    {
        var currentLength = _skillsIntances.Length;
        Array.Resize<UISkill>(ref _skillsIntances, currentLength + 1);
        _skillsIntances[currentLength] = item;
        item.transform.SetParent(_skillsParent, false);
        return currentLength;
    }
}
