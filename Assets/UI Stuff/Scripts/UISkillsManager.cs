using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UISkillsManager : MonoBehaviour
{
    [SerializeField] UISkill _uiItemPrefab;
    [SerializeField] Transform _skillsParent;
    UISkill[] _skillsIntances = new UISkill[0];
    Vector2 _lastItemSize = Vector2.zero;

    UISkill CreateInputSkill(InputActionReference inputType)
    {
        var result = SearchIfItExist(inputType);
        if(result != null) return result;
        UISkill skillItem = Instantiate(_uiItemPrefab);
        AddItemInstance(skillItem);
        return skillItem;
    }

    public UISkill SetInputSkill(InputActionReference inputType, Sprite skillIcon)
    {
        var skill = CreateInputSkill(inputType);
        skill.Initialize(inputType, skillIcon);
        var size = skill.GetItemSize();
        if(_lastItemSize.sqrMagnitude < size.sqrMagnitude)
        {
            _lastItemSize = size;
        }
        foreach(UISkill uiItem in _skillsIntances) uiItem.SetItemSize(_lastItemSize);
        return skill;
    }
    public UISkill SetInputSkill(InputActionReference inputType, Sprite skillIcon, float cooldown)
    {
        var skill = CreateInputSkill(inputType);
        skill.Initialize(inputType, skillIcon, cooldown);
        var size = skill.GetItemSize();
        if(_lastItemSize.sqrMagnitude < size.sqrMagnitude)
        {
            _lastItemSize = size;
        }
        foreach(UISkill uiItem in _skillsIntances) uiItem.SetItemSize(_lastItemSize);
        return skill;
    }

    UISkill SearchIfItExist(InputActionReference type)
    {
        UISkill result = null;

        for (int i = 0; i < _skillsIntances.Length; i++)
        {
            var item = _skillsIntances[i];
            if(item.InputType == type)
            {
                result = item;
                break;
            }
        }
        return result;
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
