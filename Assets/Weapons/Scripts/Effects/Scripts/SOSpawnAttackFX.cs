using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SpawnAttackFX")]
public class SOSpawnAttackFX : SOSpawnFXBase
{
    public override void OnAttackFX()
    {
        Vector3 weaponPos = _weapon.PrefabTransform.position;
        SpawnFX(weaponPos);
    }
}
