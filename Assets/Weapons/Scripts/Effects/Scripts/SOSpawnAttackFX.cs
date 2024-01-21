using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SpawnAttackFX")]
public class SOSpawnAttackFX : WeaponEffects
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

    public override void OnAttackFX()
    {
        Vector3 weaponPos = _weapon.PrefabTransform.position;
        Transform obj = Instantiate(_fxPrefab, weaponPos + (Vector3)_offset, Quaternion.identity).transform;
        if(_flipOnDirection)
        {
            Vector3 flippedScale = obj.localScale;
            Vector3 direction = weaponPos - _player.position;
            direction.Normalize();
            flippedScale.x = (direction.x > 0) ? -1 : 1;
            obj.localScale = flippedScale;

            obj.position = weaponPos + (Vector3)(_offset * flippedScale.x);
        }
    }
}
