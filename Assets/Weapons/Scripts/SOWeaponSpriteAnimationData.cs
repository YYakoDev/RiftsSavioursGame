using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapons/WeaponSpriteData", fileName = "Data")]
public class SOWeaponSpriteAnimationData : ScriptableObject
{
    [SerializeField]Sprite _sprite;
    [SerializeField]AnimatorOverrideController _animOverrideController;
    [SerializeField]private bool _flipSprite = true;
    [SerializeField]private Vector3 _spawnPosition = new Vector3(-0.55f, 0.25f, 0f);
    [SerializeField]private float _spawnRotation = 0;
    //properties
    public Sprite Sprite => _sprite;
    public AnimatorOverrideController AnimatorOverride => _animOverrideController;
    public bool FlipSprite => _flipSprite;
    public Vector3 SpawnPosition => _spawnPosition;
    public float SpawnRotation => _spawnRotation;
    //public AnimationClip[] AttackAnimations => _attackAnimations;
}
