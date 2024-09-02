using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "ExplodeOnDeathBehaviour")]
public class SOEnemyDeathExplosionBehaviour : SOEnemyBehaviour
{
    public override void Action()
    {
        //do the physics check here, similar to the anvil (spawn a explosion prefab too!)
        //Debug.Log("Death on explosion");
    }
}
