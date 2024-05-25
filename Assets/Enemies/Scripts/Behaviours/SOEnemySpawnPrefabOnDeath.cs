using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "SpawnOnDeathBehaviour")]
public class SOEnemySpawnPrefabOnDeath : SOEnemyBehaviour
{
    [SerializeField] GameObject _prefab;
    public override void Action()
    {
        Instantiate(_prefab, _brain.transform.position, Quaternion.identity);
    }
}
