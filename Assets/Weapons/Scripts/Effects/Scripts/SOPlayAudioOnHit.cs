using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuPath + "PlayAudioOnHit")]
public class SOPlayAudioOnHit : WeaponEffects
{
    [SerializeField] AudioClip _clip;
    public override void OnHitFX(Vector3 pos)
    {
        _effects.PlayAudio(_clip);
    }
}
