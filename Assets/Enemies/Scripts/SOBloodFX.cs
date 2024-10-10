using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BloodFX")]
public class SOBloodFX : ScriptableObject
{
    [SerializeField] AnimatorOverrideController _animator;
    [SerializeField] Color _initialColor;
    [SerializeField] Vector3 _size = Vector3.one;
    [SerializeField] float _coagulationTime = 10f;

    public AnimatorOverrideController Animator => _animator;
    public Color InitialColor => _initialColor;
    public Vector3 Size => _size;
    public float CoagulationTime => _coagulationTime;

}
