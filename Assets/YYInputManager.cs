using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YYInputManager : MonoBehaviour
{
    static YYInputManager i;
    static bool _stopInput = false;
    static Timer _stopInputTime;
    [SerializeField]KeyInput[] _keys = new KeyInput[0];
    const string HorizontalAxis = "Horizontal";
    const string VerticalAxis = "Vertical";
    Vector2 _movementAxis;
    static Vector3 _mousePosition;
    static KeyInput[] KeyInputs;
    public static event Action<Vector2> OnMovement;
    //public static event Action<Vector3> OnMouseInput;
    //public static Vector2 MovementInput => _movementAxis;
    public static Vector3 MousePosition => _mousePosition;

    private void Awake() {
        if(i != null && i != this) Destroy(this);
        else i = this;
        DontDestroyOnLoad(i);//
        KeyInputs = _keys; // if you have a save system this is where you would change the keycodes to the saved ones (modified by the player)
    }

    private void Start() {
   
        _stopInputTime = new(0f);
        _stopInputTime.Stop();
        _stopInputTime.onEnd += ResumeInput;
    }

    private void Update() {
        _stopInputTime.UpdateTime();
        if(_stopInput) return;

        foreach (var key in _keys)
        {
            if (Input.GetKeyDown(key.PrimaryKey) || Input.GetKeyDown(key.SecondayKey)) key.KeyPressed();
            if (Input.GetKey(key.PrimaryKey) || Input.GetKey(key.SecondayKey)) key.KeyHolded();
        }
        _movementAxis.x = Input.GetAxisRaw(HorizontalAxis);
        _movementAxis.y = Input.GetAxisRaw(VerticalAxis);
        OnMovement?.Invoke(_movementAxis);
        _mousePosition = Input.mousePosition;
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

    private static void ResumeInput() => _stopInput = false;
    public static void StopInput() => _stopInput = true;
    
    public static void StopInput(float duration)
    {
        _stopInput = true;
        _stopInputTime.ChangeTime(duration);
        _stopInputTime.Start();
    }

    private void OnDestroy() {
        _stopInputTime.onEnd -= ResumeInput;
    }
}
