using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SpawnHitFX")]
public class SOSpawnHitFX : WeaponEffects
{
    [SerializeField] GameObject _fxPrefab;
    [SerializeField] bool _flipOnDirection;
    [SerializeField] Vector2 _offset = Vector2.zero;
    Transform _player;

    public override void Initialize(WeaponBase weapon)
    {
        base.Initialize(weapon);
        _player = weapon.FxsScript.transform;
    }

    public override void OnHitFX(Vector3 pos)
    {
        Transform obj = Instantiate(_fxPrefab, pos + (Vector3)_offset, Quaternion.identity).transform;
        if(_flipOnDirection)
        {
            Vector3 flippedScale = obj.localScale;
            Vector3 direction = pos - _player.position;
            direction.Normalize();
            flippedScale.x = (direction.x > 0) ? -1 : 1;
            obj.localScale = flippedScale;

            obj.position = pos + (Vector3)(_offset * flippedScale.x);
        }
        
    }
}
