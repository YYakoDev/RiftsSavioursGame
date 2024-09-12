using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRaycastEnabler : MonoBehaviour
{
    [SerializeField] GameObject _parent;
    [SerializeField] MenuController _menuController;
    Selectable[] _images = null;

    private void Awake()
    {
        _images = GetComponentsInChildren<Selectable>();
    }

    private void Start()
    {
        if(_parent.Equals(null)) _parent = gameObject;
        if(_menuController != null)_menuController.OnMenuChanged += CheckNewMenu;
    }

    public void EnableRaycast() => SwitchImageRaycast(true);
    public void DisableRaycast() => SwitchImageRaycast(false);
    void CheckNewMenu(GameObject newMenu)
    {
        if(newMenu != _parent) DisableRaycast();
        else EnableRaycast();
    }

    void SwitchImageRaycast(bool state)
    {
        foreach(var img in _images)
        {
            img.interactable = state;//
            //img.raycastTarget = state;
        }
    }

    private void OnDestroy() {
        if(_menuController != null) _menuController.OnMenuChanged -= CheckNewMenu;
    }

}
