using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Drops/XPDrop")]
public class XPDrop : Drop
{
    [SerializeField]private int _xpAmount = 1;
    public override void OnPickUp(PickUpsController pickUpsController)
    {
        pickUpsController.AddXP(_xpAmount);
    }
}
