using System;
using UnityEngine;

[Serializable]
public class KeyInput
{
    //string _name = "KeyInput";
    [SerializeField]KeyInputTypes _type;
    [SerializeField]KeyCode _keyCode, _secondaryKeyCode;
    [SerializeField]JoystickKeyCodes _controllerKey;
    public event Action OnKeyPressed;
    public event Action OnKeyHold;

    public KeyCode PrimaryKey => _keyCode;
    public KeyCode SecondaryKey => _secondaryKeyCode;
    public KeyCode ControllerKey => (KeyCode)_controllerKey;
    public KeyInputTypes Type => _type;

    public void KeyPressed() => OnKeyPressed?.Invoke();
    public void KeyHolded() => OnKeyHold?.Invoke();
    public string GetInputKeyName()
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
            default:
                result = _keyCode.ToString();
                break;
        }
        return result;
    }
}
