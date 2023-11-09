using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class TweenController : MonoBehaviour
{  
    CanvasScaler _rootCanvas;
    TweenAnimator _tweenAnimator;
    RectTransform _rectTransform;
    [SerializeField]private Vector3 _destination;
    [SerializeField]float _duration;
    [SerializeField]bool _loop;
    [SerializeField]CurveTypes _curveType;
    Vector2 _canvasReferenceResolution;
    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _tweenAnimator = GetComponent<TweenAnimator>();
        _rootCanvas = transform.root.GetComponentInChildren<CanvasScaler>();
        _canvasReferenceResolution = _rootCanvas.referenceResolution;
    }
    

    private void OnEnable() {
        _tweenAnimator.MoveTo(_rectTransform, _destination, _duration, _curveType ,loop: _loop);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        if(_rootCanvas == null)
        {
            _rootCanvas = transform.root.GetComponentInChildren<CanvasScaler>();
            _canvasReferenceResolution = _rootCanvas.referenceResolution;
        }
        Debug.Log(Display.main.renderingWidth);
        Gizmos.DrawWireCube(HelperMethods.TranslateUiToWorldPoint(_canvasReferenceResolution, _destination), Vector3.one * 50);
    }

    /*Vector2 TranslateWorldToUIPoint(Vector2 position)
    {
        float screenXCenter = Screen.width / 2f;
        float screenYCenter = Screen.height / 2f;

        float canvasXCenter = _rootCanvas.referenceResolution.x / 2f;
        float canvasYCenter = _rootCanvas.referenceResolution.y / 2f;

        Vector2 UIPoint = new Vector2
        (
            (position.x - screenXCenter) * (canvasXCenter/screenXCenter), 
            (position.y - screenYCenter) * (canvasYCenter/screenYCenter)  
        );
        return UIPoint;
    }*/
}
