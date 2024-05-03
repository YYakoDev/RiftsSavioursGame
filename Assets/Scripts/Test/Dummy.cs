using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WhiteBlinkEffect), typeof(AudioSource))]
public class Dummy : MonoBehaviour, IDamageable
{
    AudioSource _audio;
    WhiteBlinkEffect _blinkFx;
    [SerializeField] AudioClip _onHitSfx;

    private void Awake() {
        _blinkFx = GetComponent<WhiteBlinkEffect>();
        _audio = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        _audio.PlayWithVaryingPitch(_onHitSfx);
        _blinkFx?.Play();
    }
    public void Die(){}

}
