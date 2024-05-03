using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    KeyInput _hotkey;
    [SerializeField]Canvas _canvas;
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] Sprite _settingsIcon;
    [SerializeField] Vector3 _iconPosition;
    UISkill _inputItem;
    // Start is called before the first frame update
    void Start()
    {
        _inputItem = _uiSkillsManager.SetInputSkill(KeyInputTypes.Settings, _settingsIcon);
        var itemTransform = _inputItem.transform;
        itemTransform.SetParent(_uiSkillsManager.transform, false);
        itemTransform.localPosition = _iconPosition;
        _hotkey = YYInputManager.GetKey(KeyInputTypes.Settings);
        _hotkey.OnKeyPressed += PlaySettingsButtonAnimation;
        //itemTransform.localScale = Vector3.one;
    }

    void PlaySettingsButtonAnimation()
    {
        _inputItem.Interact();
    }

    private void OnDestroy() {
        _hotkey.OnKeyPressed -= PlaySettingsButtonAnimation;
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_iconPosition), Vector3.one * 30f);
    }
}
