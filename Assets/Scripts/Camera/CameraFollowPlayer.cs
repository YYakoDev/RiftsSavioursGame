using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]Transform _target;
    [SerializeField] Vector2 _positionOffset;
    //[Range(0,1)][SerializeField]float followSmoothing = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        if(_target == null) _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 _followPosition = new Vector3(_target.position.x + _positionOffset.x,_target.position.y + _positionOffset.y,transform.position.z);
        
       
        //transform.position = Vector3.Lerp(transform.position,_followPosition,followSmoothing);
        transform.position = _followPosition;
    }
}
