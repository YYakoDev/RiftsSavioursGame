using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SpawnHitFX")]
public class SOSpawnHitFX : SOSpawnFXBase
{
    public override void OnHitFX(Transform enemy)
    {
        SpawnFX(enemy.position);
    }
}
