using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] Texture2D _defaultTexture;
    private void Awake() {
        //Cursor.SetCursor(_defaultTexture, Vector2.zero, CursorMode.Auto);
    }

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
}
