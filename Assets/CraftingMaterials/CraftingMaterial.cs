using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingMaterials/CraftingMaterial")]
public class CraftingMaterial : ScriptableObject
{
    [HideInInspector]public Sprite sprite;
    //properties
    public Sprite Sprite => sprite;

}
