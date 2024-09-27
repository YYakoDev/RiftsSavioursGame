using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeGpuInstancing : MonoBehaviour
{
    [SerializeField] RenderParams _renderParams = new();
    [SerializeField] Mesh _mesh;
    [SerializeField] Material _instancedMaterial;
    [SerializeField] int _instancesCount;
    Matrix4x4[] _matrices;
    float _goBackTime;
    [SerializeField] float _rot, _scale = 2f;
    [SerializeField] Vector3 _directionToMove;
    Vector3 _translation;
    // Start is called before the first frame update
    void Start()
    {
        _renderParams = new(_instancedMaterial);
        _matrices = new Matrix4x4[_instancesCount];
        _goBackTime = 1f;
        _translation = new(0.1f, 0.1f, 0f);
        SpriteRenderer renderer = new();
        //renderer.
        //Texture3D texture = new(1, 2, 1, UnityEngine.Experimental.Rendering.DefaultFormat.HDR, flags);
        //texture.
    }

    // Update is called once per frame
    void Update()
    {
        _goBackTime -= Time.deltaTime;
        if(_goBackTime < 0)
        {
            _goBackTime = 1f;
            _translation = new(0.1f, 0.1f, 0f);
        }
        _translation += _directionToMove * 0.3f;
        for(int i=0; i<_instancesCount; ++i)
        {
            var pos = Vector3.right * i * 2f + _translation;
            var newRot = Quaternion.identity.eulerAngles;
            newRot.z = _rot;
            _matrices[i].SetTRS(pos, Quaternion.Euler(newRot), Vector3.one * _scale);
            //
            //_matrices[i].SetColumn(0, transform.rotation.eulerAngles);
        }
        Graphics.RenderMeshInstanced(_renderParams, _mesh, 0, _matrices);
    }
}
