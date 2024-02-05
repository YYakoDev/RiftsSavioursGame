using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CharacterData")]
public class SOCharacterData : ScriptableObject
{
    [SerializeField] SOPlayerStats _stats;
    [SerializeField] SOPlayerAttackStats _atkStats;
    [SerializeField] Sprite _sprite;
    [SerializeField] AnimatorOverrideController _animator;

    public SOPlayerStats Stats => _stats;
    public SOPlayerAttackStats ATKStats => _atkStats;
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController Animator => _animator;
}
