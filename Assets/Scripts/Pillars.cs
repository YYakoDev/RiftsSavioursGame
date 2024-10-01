using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillars : MonoBehaviour
{
    [SerializeField] Animator[] _pillarAnimators;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _pillarBreakingSound;

    public void PlayAnimations()
    {
        _audio.PlayWithVaryingPitch(_pillarBreakingSound);
        foreach(var animator in _pillarAnimators) animator.Play("Animation");
    }
}
