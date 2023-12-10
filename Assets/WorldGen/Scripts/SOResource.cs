using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Resources/ResourceInfo")]
public class SOResource : ScriptableObject
{
    [SerializeField]ResourceInfo _info;
    public ResourceInfo Info => _info;
}
