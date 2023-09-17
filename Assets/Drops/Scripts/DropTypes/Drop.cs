using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : ScriptableObject
{
    //references
//    protected PickUpsController _pickUpsController;

    //fields
    [Header("Drop Properties")]
    [SerializeField]private string _name;
    [SerializeField]private Sprite _sprite;
    [SerializeField]private AnimatorOverrideController _animatorOverride;
    [SerializeField, Range(0,100)]int _dropChance = 100;

    //Properties
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController AnimatorOverride => _animatorOverride;
    public int DropChance => _dropChance;
    

    public virtual void OnPickUp(PickUpsController pickUpsController)
    {

    }
}
