using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/WeaponSpriteData", fileName = "Data")]
public class SOWeaponSpriteAnimationData : ScriptableObject
{
    [SerializeField]Sprite _sprite;
    [SerializeField]AnimatorOverrideController _animOverrideController;
    AnimationClip[] _attackAnimations;

    //properties
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController AnimatorOverride => _animOverrideController;
    public AnimationClip[] AttackAnimations => _attackAnimations;

    public void SetAnimations()
    {
        _attackAnimations = _animOverrideController.animationClips;
        foreach(var clip in _attackAnimations)
        {
            Debug.Log(clip.name);
        }
    }
}
