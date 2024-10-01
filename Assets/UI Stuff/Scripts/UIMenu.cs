using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    protected string _buttonName;
    bool _isMenuOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown(_buttonName)) ToggleMenu();
    }

    public virtual void OpenMenu()
    {
        _isMenuOpen = true;
    }
    public virtual void CloseMenu()
    {
        _isMenuOpen = false;
    }

    public virtual void ToggleMenu()
    {
        if(_isMenuOpen) CloseMenu();
        else OpenMenu();
    }
}
