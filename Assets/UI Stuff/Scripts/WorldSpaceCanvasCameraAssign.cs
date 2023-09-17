using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCanvasCameraAssign : MonoBehaviour
{
    Canvas _canvas;
    private void Awake() {
        _canvas = this.GetComponent<Canvas>();

    }

    void Start()
    {
        if(_canvas.worldCamera == null)
        {
            _canvas.worldCamera = Camera.main;
        }
    }
}
