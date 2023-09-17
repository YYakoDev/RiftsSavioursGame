using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsScaler : MonoBehaviour
{
    [SerializeField]Camera _mainCamera;
    [SerializeField]Transform[] _spawnPoints;
    [SerializeField]float _offset = 1f;
    float _cameraHeight;
    float _cameraWidth;
    // Start is called before the first frame update
    void Start()
    {
        if(_mainCamera == null) _mainCamera = Camera.main;
        UpdateCameraValues();
        ScaleSpawnPointsPosition();
    }

    public void UpdateCameraValues()
    {
        //IF THE RESOLUTIONS CHANGES THIS VALUES WILL BE INACCURATE UNLESS YOU REGISTER THOSE CHANGES AND CALL THIS METHOD ON SOME EVENT
        _cameraHeight = _mainCamera.orthographicSize;
        _cameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
    }

    public void ScaleSpawnPointsPosition()
    {
        for(int i = 0; i <Directions.NormalizedDirections.Length; i++)
        {
            float newXPoint = 0;
            float newYPoint = 0;
            Vector2 direction = Directions.NormalizedDirections[i];

            if(direction.x != 0)
            {
                newXPoint = SetOffset(direction.x, _cameraWidth, _offset);
                newXPoint *= Mathf.Sign(direction.x);
            }
            if(direction.y != 0)
            {
                newYPoint = SetOffset(direction.y, _cameraHeight, _offset);
                newYPoint *= Mathf.Sign(direction.y);
            }
            Vector2 newPosition = new Vector2(newXPoint,newYPoint);
            _spawnPoints[i].localPosition = newPosition;
        }
    }

    float SetOffset(float point, float cameraValue, float offset = 1f)
    {
        float newPoint = Mathf.Abs(point) + Mathf.Abs(cameraValue) + offset;
        return newPoint;
    }
}
