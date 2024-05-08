using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YYInputManager : MonoBehaviour
{
    static YYInputManager i;
    static bool _stopInput = false;
    [SerializeField]KeyInput[] _keys = new KeyInput[0];
    const string HorizontalAxis = "Horizontal";
    const string VerticalAxis = "Vertical";
    Vector2 _movementAxis;
    static Vector3 _mousePosition;
    static KeyInput[] KeyInputs;
    public static event Action<Vector2> OnMovement;
    public static event Action<float> OnMouseScroll;
    //public static event Action<Vector3> OnMouseInput;
    //public static Vector2 MovementInput => _movementAxis;
    public static Vector3 MousePosition => _mousePosition;

    private void Awake() {
        if(i != null && i != this) Destroy(this);
        else i = this;
        //DontDestroyOnLoad(i);//
        KeyInputs = _keys; // if you have a save system this is where you would change the keycodes to the saved ones (modified by the player)
    }

    private void Start() {
        ResumeInput();
    }

    private void Update() {
        if(_stopInput) return;

        foreach (var key in _keys)
        {
            if (Input.GetKeyDown(key.PrimaryKey) || Input.GetKeyDown(key.SecondaryKey) || Input.GetKeyDown(key.ControllerKey))
            {
                Debug.Log(key.GetInputKeyName());
                key.KeyPressed();
            }
            if (Input.GetKey(key.PrimaryKey) || Input.GetKey(key.SecondaryKey) || Input.GetKey(key.ControllerKey)) key.KeyHolded();
        }

        //movement
        _movementAxis.x = Input.GetAxisRaw(HorizontalAxis);
        _movementAxis.y = Input.GetAxisRaw(VerticalAxis);
        if(!_stopInput)OnMovement?.Invoke(_movementAxis);

        //mouse
        _mousePosition = Input.mousePosition;
        var mouseScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        OnMouseScroll?.Invoke(mouseScroll);
    }

    public static KeyInput GetKey(KeyInputTypes type)
    {
        KeyInput key = null;
        foreach(KeyInput inputKey in KeyInputs)
        {
            if(inputKey.Type == type) return inputKey;
        }
        return key;
    }

    public static void ResumeInput() => _stopInput = false;
    public static void StopInput()
    {
        _stopInput = true;
        OnMovement?.Invoke(Vector2.zero);
    }
}
