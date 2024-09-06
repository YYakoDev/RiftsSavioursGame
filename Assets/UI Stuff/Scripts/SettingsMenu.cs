using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]InputActionReference _hotkey;
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
        _hotkey.action.performed += PlaySettingsButtonAnimation;
        //itemTransform.localScale = Vector3.one;
    }

    void PlaySettingsButtonAnimation(InputAction.CallbackContext obj)
    {
        _inputItem.Interact();
    }

    private void OnDestroy() {
        _hotkey.action.performed -= PlaySettingsButtonAnimation;
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_iconPosition), Vector3.one * 30f);
    }
}
