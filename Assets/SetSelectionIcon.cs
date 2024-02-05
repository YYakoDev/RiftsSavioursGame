using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class SetSelectionIcon : MonoBehaviour
{
    RectTransform _thisRect;
    [SerializeField] Vector2 _scaleOffset;
    EventSystem _currentEventSystem;
    Timer _updateRate;
    GameObject _previouslySelectedObj;
    private void Awake() {
        _thisRect = GetComponent<RectTransform>();
        _currentEventSystem = EventSystem.current;
        _updateRate = new(0.1f, true, true);
        _updateRate.onEnd += TrySelectNewElement;
    }


    void TrySelectNewElement()
    {
        var currentObj = _currentEventSystem.currentSelectedGameObject;
        if(currentObj == null || !currentObj.activeInHierarchy)
            Deselect();
        else if(_previouslySelectedObj != currentObj)
            SetPositionToSelectedItem(currentObj);
    }

    private void Update() {
        _updateRate.UpdateTime();
    }

    void SetPositionToSelectedItem(GameObject obj)
    {
        if(_thisRect == null) _thisRect = GetComponent<RectTransform>();
        _thisRect.localScale = Vector3.one;
        if(obj.TryGetComponent<RectTransform>(out var rect))
        {
            _thisRect.sizeDelta  = rect.sizeDelta * rect.localScale + _scaleOffset;
            transform.position = rect.position;
        }else
        {
            _thisRect.localScale = obj.transform.localScale;
            transform.position = obj.transform.position;
        }
    }

    void Deselect()
    {
        _thisRect.localScale = Vector3.zero;
    }

    private void OnDestroy() {
        _updateRate.onEnd -= TrySelectNewElement;
    }
}
