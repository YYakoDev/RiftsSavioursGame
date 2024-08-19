using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterData")]
public class SOCharacterData : ScriptableObject
{
    [SerializeField] string _name;
    [SerializeField, TextArea] string _description;
    [SerializeField] SOPlayerStats _stats;
   // [SerializeField] SOPlayerAttackStats _atkStats;
    [SerializeField] Sprite _sprite;
    [SerializeField] AnimatorOverrideController _animator;
    [SerializeField] SODashData _dashData, _backDashData;
    [SerializeField]List<InventoryMaterialData> _startingMaterials = new();

    public string Name => _name;
    public string Description => _description;
    public SOPlayerStats Stats => _stats;
    //public SOPlayerAttackStats ATKStats => _atkStats;
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController Animator => _animator;
    public SODashData DashData => _dashData;
    public SODashData BackDashData => _backDashData;
    public List<InventoryMaterialData> StartingInventory => _startingMaterials;
}
