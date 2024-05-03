using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TweenAnimator))]
public class ResourceUIPopup : MonoBehaviour
{
    RectTransform _rect;
    TweenAnimator _animator;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] Image _icon;


    int _previousMatCount = 0;

    private void Awake() {
        _previousMatCount = 0;
    }

    public RectTransform GetRect()
    {
        if(_rect == null) _rect = GetComponent<RectTransform>();
        return _rect;
    }
    public TweenAnimator GetAnimator()
    {
        if(_animator == null) _animator = GetComponent<TweenAnimator>();
        return _animator;
    }

    public void Initialize(int matCount, Sprite icon)
    {
        _previousMatCount = matCount;
        SetText(_previousMatCount.ToString());
        _icon.sprite = icon;
    }

    public void AddToMaterialCount(int addition)
    {
        _previousMatCount += addition;
        SetText(_previousMatCount.ToString());
    }

    void SetText(string textToAdd)
    {
        _text.text = "+" + textToAdd;
    }
}
