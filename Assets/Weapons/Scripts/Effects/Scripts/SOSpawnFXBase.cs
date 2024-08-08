using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSpawnFXBase : WeaponEffects
{
    [SerializeField] GameObject _fxPrefab;
    [SerializeField] bool _flipOnDirection, _offsetRotation = true;
    [SerializeField] Vector2 _offset = Vector2.zero;
    [SerializeField, Range(0, 35)] float _angleOffset = 25;
    Transform _player;
    NotMonoObjectPool _pool;
    public override void Initialize(WeaponBase weapon)
    {
        base.Initialize(weapon);
        _player = weapon.FxsScript.transform;
        _pool = new(25, _fxPrefab, _player, true);
    }
    protected void SpawnFX(Vector3 spawnPosition)
    {
        Transform obj = _pool.GetObjectFromPool().transform;
        obj.position = spawnPosition + (Vector3)_offset;
        var rotation = _fxPrefab.transform.rotation;
        if(_flipOnDirection)
        {
            Vector3 flippedScale = obj.localScale;
            Vector3 direction = spawnPosition - _player.position;
            direction.Normalize();
            flippedScale.x = (direction.x > 0) ? -1 : 1;
            obj.localScale = flippedScale;
            rotation.z *= flippedScale.x;
            obj.position = spawnPosition + (Vector3)(_offset * flippedScale.x);
        }
        var euler = rotation.eulerAngles;
        if(_offsetRotation) euler.z += Random.Range(_angleOffset, -_angleOffset);
        obj.rotation = Quaternion.Euler(euler);
        obj.SetParent(null);
        obj.gameObject.SetActive(true);
    }
}
