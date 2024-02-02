using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : ScriptableObject
{
    //references
//    protected PickUpsController _pickUpsController;

    //fields
    [Header("Drop Properties")]
    //[SerializeField]private string _name;
    [SerializeField]private Sprite _sprite;
    [SerializeField]private AudioClip[] _pickupSounds = new AudioClip[0];
    [SerializeField, Range(0,100)]int _dropChance = 100;
    [SerializeField, Range(0f, 1f)] float _lightIntensity = 0f;

    //Properties
    //public string Name => _name;
    public Sprite Sprite => _sprite;
    public AudioClip[] PickUpSounds => _pickupSounds;
    public int DropChance => _dropChance;
    public float LightIntensity => _lightIntensity;


    public virtual void OnPickUp(PickUpsController pickUpsController)
    {

    }
}
