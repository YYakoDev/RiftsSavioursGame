using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    const string UIActionMap = "UI", GameplayActionMap = "Gameplay";

    private void Awake()
    {

    }

    void Test(InputAction.CallbackContext obj)
    {
        Debug.Log("Testing!");
    }

    private void OnEnable() {
        
        //_playerControls.Enable();//
    }
    private void OnDisable()
    {
        //_playerControls.Disable();
    }

    private IEnumerator Start() {
        yield return null;
        yield return null;
        
        //Debug.Log();
    }

    public void ResetActionMap()
    {
        ChangeInputToGameplay();
    }
    public void ChangeInputToUI()
    {
        _input.SwitchCurrentActionMap(UIActionMap);
    }
    public void ChangeInputToGameplay()
    {
        _input.SwitchCurrentActionMap(GameplayActionMap);
    }

    private void OnDestroy() {
        //_input.actions["Escape"].performed -= Test;
    }
}
