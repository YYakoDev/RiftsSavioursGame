using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CraftingMaterials/CraftingMaterial")]
public class CraftingMaterial : ScriptableObject
{
    [SerializeField]private Sprite _sprite;
    //properties
    public Sprite Sprite => _sprite;

}
