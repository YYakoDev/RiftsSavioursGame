using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnSelected : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField]Vector2 _initialScale = Vector2.zero;
    [SerializeField]Vector2 _scaleIncrease = Vector2.one * 1.5f;
    Transform _cachedTransform;
    private void Awake() {
        _cachedTransform = transform;
        if(_initialScale == Vector2.zero)_initialScale = _cachedTransform.localScale;
    }


    public void ScaleUp()
    {
        _cachedTransform.localScale = _initialScale * _scaleIncrease;
    }
    public void ScaleDown()
    {
        _cachedTransform.localScale = _initialScale;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        ScaleUp();
    }
    public virtual void OnDeselect(BaseEventData eventData)
    {
        ScaleDown();
    }

    private void OnDisable() {
        ScaleDown();
    }
}
