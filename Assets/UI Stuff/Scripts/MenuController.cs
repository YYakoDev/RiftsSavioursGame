using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [SerializeField] CursorSetter _cursorSetter;
    public event Action<GameObject> OnMenuChanged;
    GameObject _currentMenu;
    //bool _locked;
    EventSystem _eventSys;

    public CursorSetter CursorSetter => _cursorSetter;

    private void Awake() {
        
    }

    private void Start() {
        _eventSys = EventSystem.current;
    }

    public void SwitchCurrentMenu(GameObject menu, bool showCursor = true)
    {
        _currentMenu = menu;
        OnMenuChanged?.Invoke(menu);
        //Debug.Log("New menu is:   " + menu);
        if(menu != null)
        {
            var obj = menu.GetComponentInChildren<Selectable>(true)?.gameObject;
            if(_eventSys == null) _eventSys = EventSystem.current;
            _eventSys.SetSelectedGameObject(obj);
            if(showCursor)_cursorSetter.ShowCursor();
        }else
        {
            _cursorSetter.SwitchBack();
        }
    }
}
