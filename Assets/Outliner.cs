using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outliner : MonoBehaviour
{
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] Mesh _mesh;
    [SerializeField] MeshRenderer _meshRenderer;
    [SerializeField] Material _instancedMaterial;
    Matrix4x4 _matrix;
    RenderParams _renderParams;
    [SerializeField] Sprite _sprite;
    const float Rotation = 180f, Scale = 2f;

    // Start is called before the first frame update
    void Start()
    {
        _renderParams = new(_instancedMaterial);
        _matrix = new();
        _meshRenderer.transform.localScale = Vector3.one * Scale;
        var rot = _meshRenderer.transform.localRotation.eulerAngles;
        rot.z = Rotation;
        _meshRenderer.transform.localRotation = Quaternion.Euler(rot);
    }

    // Update is called once per frame
    void Update()
    {
        _meshRenderer.material.SetTexture("_BaseMap", _renderer.sprite.texture);
        //
    }
}
