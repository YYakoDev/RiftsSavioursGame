using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Drops/ResourceDrop")]
public class ResourceDrop : Drop
{
    [SerializeField]private CraftingMaterial _craftingMaterial;
    public override void OnPickUp(PickUpsController pickUpsController)
    {
        pickUpsController.AddMaterial(_craftingMaterial);
    }

    public void SetSprite()
    {
        if(_craftingMaterial != null) _craftingMaterial.Sprite = Sprite;
    }

    private void OnValidate() {
        SetSprite();
    }
}
