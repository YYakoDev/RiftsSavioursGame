using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePointer : MonoBehaviour
{
    [SerializeField]SOResource _resourceData;
    SpriteRenderer _renderer;
    public static event Action<ResourceInfo, Vector3> OnResourceSignal;
    
    private void Start() {
        if(_resourceData == null) gameObject.SetActive(false);
        DestroySpritePreview();
        OnResourceSignal?.Invoke(_resourceData.Info, transform.position);
        Debug.Log("Sending signal");
    }

    public void PreviewSprite()
    {
        if(_resourceData == null) return;
        if(Application.isPlaying)
        {
            DestroySpritePreview();
            return;
        }

        if(!TryGetComponent<SpriteRenderer>(out var _renderer)) _renderer = gameObject.AddComponent<SpriteRenderer>();
        
        transform.localScale = Vector3.one * _resourceData.Info.ScaleFactor;
        int sortOrder = -(int)((transform.position.y - _resourceData.Info.SpriteOrderOffset) * 35);
        _renderer.sortingOrder = sortOrder;
        _renderer.sprite = _resourceData.Info.Sprite;
    }

    void DestroySpritePreview()
    {
        if(TryGetComponent<SpriteRenderer>(out var _renderer)) Destroy(_renderer);
    }

    private void OnValidate() {
        if(_resourceData == null || name == _resourceData.name) return;
        name = _resourceData.name;
    }
}
