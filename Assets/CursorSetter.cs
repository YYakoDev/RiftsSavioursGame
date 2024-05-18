using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    [SerializeField] Texture2D _defaultTexture;
    private void Awake() {
        Cursor.SetCursor(_defaultTexture, Vector2.zero, CursorMode.Auto);
    }
}
