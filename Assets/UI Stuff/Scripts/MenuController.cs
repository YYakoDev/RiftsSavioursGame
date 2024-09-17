using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    public event Action<GameObject> OnMenuChanged;
    GameObject _currentMenu;
    //bool _locked;
    EventSystem _eventSys;

    private void Awake() {
        
    }

    private void Start() {
        _eventSys = EventSystem.current;
    }

    public void SwitchCurrentMenu(GameObject menu)
    {
        _currentMenu = menu;
        OnMenuChanged?.Invoke(menu);
        //Debug.Log("New menu is:   " + menu);
        if(menu != null)
        {
            var obj = menu.GetComponentInChildren<Selectable>(true)?.gameObject;
            if(_eventSys == null) _eventSys = EventSystem.current;
            _eventSys.SetSelectedGameObject(obj);

        }
    }
}
