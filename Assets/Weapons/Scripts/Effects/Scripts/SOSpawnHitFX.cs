using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SpawnHitFX")]
public class SOSpawnHitFX : WeaponEffects
{
    [SerializeField] GameObject _fxPrefab;
    [SerializeField] bool _flipOnDirection;
    Transform _player;

    public override void Initialize(PlayerAttackEffects atkEffects)
    {
        base.Initialize(atkEffects);
        _player = atkEffects.transform;
    }

    public override void OnHitFX(Vector3 pos)
    {
        Transform obj = Instantiate(_fxPrefab, pos, Quaternion.identity).transform;
        if(_flipOnDirection)
        {
            Vector3 flippedScale = obj.localScale;
            Vector3 direction = pos - _player.position;
            direction.Normalize();
            flippedScale.x = (direction.x > 0) ? -1 : 1;
            obj.localScale = flippedScale;
        }
        
    }
}
