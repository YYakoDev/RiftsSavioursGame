using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPoint : MonoBehaviour
{
    [SerializeField] PlayerMovement _movementScript;
    Transform _ownTransform;
    [SerializeField] Transform _lightObj;
    void Start()
    {
        _ownTransform = transform;
        _movementScript.OnMovement += SetPosition;
    }


    void SetPosition(Vector2 movePos)
    {
        _ownTransform.localPosition = Vector3.zero + (Vector3)movePos;
        //PointLight();
    }

    private void OnDestroy() {
        _movementScript.OnMovement -= SetPosition;
    }

    void PointLight()
    {
        Vector3 targetPos = _ownTransform.localPosition;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg - 90f;
        _lightObj.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
