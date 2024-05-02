using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMoveToWorldPointTest : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] Transform _worldPoint;
    [SerializeField] RectTransform _squareRepresentation;
    [SerializeField] float _duration = 1f;
    TweenAnimator _animator;
    private void Awake() {
        _animator = GetComponent<TweenAnimator>();
    }
    private void OnEnable() {
        //var result = _canvas.TranslateWorldPointToUI(_worldPoint.position);
        //_animator.MoveTo(this.GetComponent<RectTransform>(), result , _duration);
    }

    private void OnValidate() {
        if(_canvas == null) _canvas = this.GetComponentInParent<Canvas>();
        if(_squareRepresentation != null)
        {
            //var result = _canvas.TranslateWorldPointToUI(_worldPoint.position);
            //_squareRepresentation.localPosition = result;
        }
    }
}
