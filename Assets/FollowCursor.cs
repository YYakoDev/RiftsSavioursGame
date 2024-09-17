using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class FollowCursor : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] InputActionReference _pointerPos;

    private void Start() {
        _image = GetComponent<Image>();
    }

    private void Update() {
        var mousePos = _pointerPos.action.ReadValue<Vector2>();
        transform.position = mousePos;
    }
}
