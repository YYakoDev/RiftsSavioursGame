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
    }

    public void PreviewSprite()
    {
        if(_resourceData == null) return;
        if(Application.isPlaying)
        {
            DestroySpritePreview();
            return;
        }
        if(!TryGetComponent<SpriteRenderer>(out var _renderer))
        {
            _renderer = gameObject.AddComponent<SpriteRenderer>();
        }
        transform.localScale = Vector3.one * _resourceData.Info.ScaleFactor;
        _renderer.sprite = _resourceData.Info.Sprite;
    }

    void DestroySpritePreview()
    {
        TryGetComponent<SpriteRenderer>(out var _renderer);
        if (_renderer != null)
        {
            Destroy(_renderer);
        }
    }
}
