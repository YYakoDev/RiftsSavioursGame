using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipWithMouse : MonoBehaviour
{
    /*Camera _mainCamera;
    Vector2 _mousePosition;
    bool _isFlipped;
    public Vector2 Position => _mousePosition;
    public bool IsFlipped => _isFlipped;
    private float Sign => (_isFlipped) ? -1 : 1;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        SetMousePosition();
    }

    void SetMousePosition()
    {
        _mousePosition = _mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition);
    }
    private void FixedUpdate()
    {
        var dirToMouse = _mousePosition.x - transform.position.x;
        CheckForFlip(dirToMouse);
    }

    void CheckForFlip(float xPoint)
    {
        Vector2 scale = transform.localScale;
        offset = 2f * Sign;
        if(xPoint < offset && _isFlipped)
        {
            scale.x = 1;
            _isFlipped = false;
        } 
        else if(xPoint >= offset && !_isFlipped)
        {
            scale.x = -1;
            _isFlipped = true;
        }
        
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_mousePosition, 1f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + (Vector3.up + (Vector3.right * offset)), 1f);
    }*/
}
