using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimator))]
public class DisclaimerMenu : MonoBehaviour
{
    RectTransform _rectTransform;
    TweenAnimator _animator;
    [SerializeField] MenuQuitter _menuQuitter;

    // Start is called before the first frame update
    void Start()
    {
        _menuQuitter.SetCurrentMenu(gameObject);
        _animator = GetComponent<TweenAnimator>();
        _rectTransform = GetComponent<RectTransform>();
        var endScale = _rectTransform.localScale;
        _rectTransform.localScale = Vector3.zero;
        _animator.Scale(_rectTransform, endScale, 0.5f, CurveTypes.EaseInBounce);
    }
}
