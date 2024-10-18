using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] Texture2D _defaultTexture;
    [SerializeField] WeaponAiming _aimingLogic;
    bool _autoAiming = false;
    private void Awake() {
        //Cursor.SetCursor(_defaultTexture, Vector2.zero, CursorMode.Auto);
        if(_aimingLogic != null) _aimingLogic.OnAimingChange += ChangeAiming;
    }

    void ChangeAiming(bool state) => _autoAiming = state;

    private void Start()
    {
        var hotspot = new Vector2
        (
            _defaultTexture.width / 2f,
            _defaultTexture.height / 2f
        );
        Cursor.SetCursor(_defaultTexture, hotspot, CursorMode.Auto);
        #if UNITY_WEBGL
            Debug.Log("WEBGL");
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        #endif
    }

    public void SwitchBack()
    {
        if(_autoAiming) HideCursor();
        else ShowCursor();
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }
    
    private void OnDestroy() {
        if(_aimingLogic != null)_aimingLogic.OnAimingChange -= ChangeAiming;
    }
}
