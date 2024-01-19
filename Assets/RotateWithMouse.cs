using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    [SerializeField] FlipWithMouse _mouseScript;
    [SerializeField] SpriteRenderer _renderer;
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var dirToMouse = (Vector3)_mouseScript.Position - transform.position;
        float angle = Mathf.Atan2(dirToMouse.y, dirToMouse.x) * Mathf.Rad2Deg;
        if(_mouseScript.IsFlipped) angle += 135;
        else angle -= 45;
        _renderer.flipY = _mouseScript.IsFlipped;
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        eulerRotation.z = angle;
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}
