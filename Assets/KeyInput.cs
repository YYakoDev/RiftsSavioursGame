using System;
using UnityEngine;

[Serializable]
public class KeyInput
{
    string _name = "KeyInput";
    [SerializeField]KeyInputTypes _type;
    [SerializeField]KeyCode _keyCode, _secondaryKeyCode;
    public event Action OnKeyPressed;
    public event Action OnKeyHold;

    public KeyCode PrimaryKey => _keyCode;
    public KeyCode SecondayKey => _secondaryKeyCode;
    public KeyInputTypes Type => _type;

    public void KeyPressed() => OnKeyPressed?.Invoke();
    public void KeyHolded() => OnKeyHold?.Invoke();

}
