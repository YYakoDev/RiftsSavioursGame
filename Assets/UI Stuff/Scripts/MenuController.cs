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
    bool _locked;
    EventSystem _eventSys;

    private void Start() {
        _eventSys = EventSystem.current;
    }

    public void SwitchCurrentMenu(GameObject menu, bool lockState = false)
    {
        if (_locked) return;
        _locked = lockState;
        _currentMenu = menu;
        OnMenuChanged?.Invoke(menu);
        if(menu != null)
        {
            var obj = menu.GetComponentInChildren<Selectable>()?.gameObject;
            Debug.Log(obj);
            _eventSys.SetSelectedGameObject(obj);
        }
    }
}
