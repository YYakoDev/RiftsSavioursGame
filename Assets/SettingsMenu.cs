using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]Canvas _canvas;
    [SerializeField] UISkillsManager _uiSkillsManager;
    [SerializeField] Sprite _settingsIcon;
    [SerializeField] Vector3 _iconPosition;

    // Start is called before the first frame update
    void Start()
    {
        int index = _uiSkillsManager.CreateNewSkill(KeyInputTypes.Settings, _settingsIcon);
        var itemTransform = _uiSkillsManager.GetSkillItem(index).transform;
        itemTransform.SetParent(_uiSkillsManager.transform);
        itemTransform.localPosition = _iconPosition;
        itemTransform.localScale = Vector3.one;
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_canvas.TranslateUiToWorldPoint(_iconPosition), Vector3.one * 30f);
    }
}
