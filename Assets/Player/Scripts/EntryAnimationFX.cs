using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class EntryAnimationFX : MonoBehaviour
{
    Animator _animator;
    AudioSource _audio;
    [SerializeField]AudioClip[] _introSFXs;
    private readonly int EntryAnimation = Animator.StringToHash("EntryAnimation");
    //
    AudioClip _introSFX => _introSFXs[Random.Range(0, _introSFXs.Length)];
    private void Awake() {

        _audio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }
    
    public void PlayFXAnimation()
    {
        PlaySound();
        _animator.Play(EntryAnimation);
    }

    void PlaySound()
    {
        _audio.PlayWithVaryingPitch(_introSFX);
    }
}
