using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToMouse : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private Camera _mainCamera;
    Vector2 _mouseDirection = Vector2.zero;

    float _stopAimingTime = 0f;

    //properties
    public Vector2 MouseDirection => _mouseDirection;

    // Start is called before the first frame update
    void Start()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
    }

    void Update()
    {
        if(_stopAimingTime > 0)
        {
            _stopAimingTime -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_stopAimingTime > 0)return;
        RotateWithMouse();
    }


    void RotateWithMouse()
    {
        _mouseDirection = _mainCamera.ScreenToWorldPoint(YYInputManager.MousePosition) - transform.position;
        float angle = Mathf.Atan2(-_mouseDirection.y,-_mouseDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Flip(_mouseDirection);

    }

    void Flip(Vector2 direction)
    {
        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = 1; 
        }
        else if(direction.x > 0)
        {
            scale.y = -1; 
        }
    
        transform.localScale = scale;
    }

    public void StopAiming(float duration)
    {
        _stopAimingTime = duration;
    }
}
