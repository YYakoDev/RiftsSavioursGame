using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterData")]
public class SOCharacterData : ScriptableObject
{
    [SerializeField] SOPlayerStats _stats;

    public SOPlayerStats Stats => _stats;
}
