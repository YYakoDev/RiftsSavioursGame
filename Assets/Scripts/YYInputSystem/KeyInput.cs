using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class KeyInput
{
    //string _name = "KeyInput";
    bool _enabled = true;
    [SerializeField]KeyInputTypes _type;
    [SerializeField]InputActionReference _key;
//    [SerializeField]JoystickKeyCodes _controllerKey;
    public event Action OnKeyPressed;
    public event Action OnKeyHold;
    public event Action OnKeyUp;

    public bool Enabled => _enabled;
    public KeyInputTypes Type => _type;

    public void KeyPressed() => OnKeyPressed?.Invoke();
    public void KeyHolded() => OnKeyHold?.Invoke();
    public void KeyUp() => OnKeyUp?.Invoke();
    /*public string GetInputKeyName()
    {
        string result = "";
        switch(_keyCode) //this is the primary key if you are using a keyboard!!  if you are using a controller you can use this primary key!
        {
            case KeyCode.Mouse0:
                result = "LMB";
                break;
            case KeyCode.Mouse1:
                result = "RMB";
                break;
            case KeyCode.Backspace:
                result = "BackSpc";
                break;
            case KeyCode.Escape:
                result = "Esc";
                break;
            case KeyCode.LeftControl:
                result = "LCtrl";
                break;
            default:
                result = _keyCode.ToString();
                break;
        }
        return result;
    }*/

    public void SetActive(bool enabled)
    {
        _enabled = enabled;
    }
}
