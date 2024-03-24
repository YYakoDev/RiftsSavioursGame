using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/WeaponSpriteData", fileName = "Data")]
public class SOWeaponSpriteAnimationData : ScriptableObject
{
    [SerializeField]Sprite _sprite;
    [SerializeField]AnimatorOverrideController _animOverrideController;

    //properties
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController AnimatorOverride => _animOverrideController;
    //public AnimationClip[] AttackAnimations => _attackAnimations;
}
