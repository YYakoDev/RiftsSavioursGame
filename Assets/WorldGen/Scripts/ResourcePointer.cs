using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePointer : MonoBehaviour
{
    [SerializeField]SOResource _resourceData;
    SpriteRenderer _renderer;
    public ResourceInfo Info => _resourceData.Info;
    public Vector3 Position => transform.position;
    public GameObject SpawnedResource { get; set; }
    bool _invoke = false;
    public static event Action<ResourcePointer> OnSignal;

    private void OnEnable() {
        if(_invoke)OnSignal?.Invoke(this);
        _invoke = true;
    }
    private void Start() {
        if(_resourceData == null) gameObject.SetActive(false);
        OnSignal?.Invoke(this);
        DestroySpritePreview();
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
        PreviewSprite();
    }
}
