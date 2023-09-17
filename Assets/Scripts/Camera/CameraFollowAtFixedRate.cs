using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAtFixedRate : MonoBehaviour
{
    [SerializeField]Transform _target;
    [SerializeField]float _smoothFollow = 10f;
    [SerializeField]Vector2 _offset;
    // Start is called before the first frame update
    void Start()
    {
        if(_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Mathf.Abs(Vector3.Distance(transform.position, _target.position)) < 0.2f)return;
        MoveCamera();
    }

    void MoveCamera()
    {
        Vector3 targetPosition = _target.position + (Vector3)_offset;
        targetPosition.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothFollow * Time.fixedDeltaTime);
    }
}
