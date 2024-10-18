using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeEdgeTeleporter : MonoBehaviour
{
    Transform _cachedTransform;
    [SerializeField] Vector3 _offsetPosition;
    [SerializeField] PrototypeEdgeTeleporter _oppositeEdge;
    [SerializeField] bool _isVertical;

    private void Awake() {
        _cachedTransform = transform;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        var otherTransform = other.transform;
        var finalPos = _oppositeEdge._cachedTransform.position + _oppositeEdge._offsetPosition;
        if(_isVertical) finalPos.y = otherTransform.position.y;
        else finalPos.x = otherTransform.position.x;
        otherTransform.position = finalPos;
        if(other.CompareTag("Player"))
        {
            var pos = finalPos;
            pos.z = -10f;
            //pos /= 2f;
            CameraEffects.Shake(0.5f, 0.015f);
            HelperMethods.MainCamera.transform.position = pos;
        }
        //other.transform.position = _oppositeEdge._cachedTransform.position + _oppositeEdge._offsetPosition;
    }

    private void OnDrawGizmosSelected() {
        if(Application.isPlaying) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + _offsetPosition, Vector3.one);
    }
}
