using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingMaterials/CraftingMaterial")]
public class CraftingMaterial : ScriptableObject
{
    [HideInInspector]public Sprite Sprite;
}
