using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Drops/ResourceDrop")]
public class ResourceDrop : Drop
{
    [SerializeField]private CraftingMaterial _craftingMaterial;
    [SerializeField]private int _amount = 1;
    public override void OnPickUp(PickUpsController pickUpsController)
    {
        pickUpsController.AddMaterial(_craftingMaterial, _amount);
    }

    public void SetSprite()
    {
        if(_craftingMaterial != null) _craftingMaterial.Sprite = Sprite;
    }

    private void OnValidate() {
        SetSprite();
    }
}
