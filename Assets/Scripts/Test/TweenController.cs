using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TweenAnimator))]
public class TweenController : MonoBehaviour
{  
    Canvas _rootCanvas;
    CanvasScaler _rootCanvasScaler;
    TweenAnimator _tweenAnimator;
    RectTransform _rectTransform;
    [SerializeField]private Vector3 _destination;
    [SerializeField]float _duration;
    [SerializeField]bool _loop;
    [SerializeField]CurveTypes _curveType;
    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
        _tweenAnimator = GetComponent<TweenAnimator>();
        _rootCanvas = transform.root.GetComponentInChildren<Canvas>();
    }
    

    private void OnEnable() {
        _tweenAnimator.MoveTo(_rectTransform, _destination, _duration, _curveType ,loop: _loop);
    }

    private void OnDrawGizmos() {

        TweenAnimator.DrawTweenPosition(_destination, Vector3.one * 50f, Color.magenta, null, true);

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
