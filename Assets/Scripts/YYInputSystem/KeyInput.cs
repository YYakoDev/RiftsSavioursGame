using System;
using UnityEngine;

[Serializable]
public class KeyInput
{
    //string _name = "KeyInput";
    [SerializeField]KeyInputTypes _type;
    [SerializeField]KeyCode _keyCode, _secondaryKeyCode;
    public event Action OnKeyPressed;
    public event Action OnKeyHold;

    public KeyCode PrimaryKey => _keyCode;
    public KeyCode SecondayKey => _secondaryKeyCode;
    public KeyInputTypes Type => _type;

    public void KeyPressed() => OnKeyPressed?.Invoke();
    public void KeyHolded() => OnKeyHold?.Invoke();
    public string GetInputKeyName()
    {
        string result = "";
        switch(_keyCode)
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
