using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputItem : MonoBehaviour
{
    [SerializeField]protected InputActionReference _inputRef;

    protected string _inputName;
    protected string _bindings;

    public string InputName => _inputName;
    public string Bindings => _bindings;

    private void Start() {
        GetInputData();
    }

    public virtual void GetInputData()
    {
        _inputName = _inputRef.action.name;
        _bindings = _inputRef.action.GetBindingDisplayString();
        //Debug.Log("Input name:  " + _inputName + "  \nBindings: " + _bindings);
    }
}
