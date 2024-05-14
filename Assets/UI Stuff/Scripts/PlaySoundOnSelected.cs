using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnSelected : ScaleOnSelected
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _selectedSound;
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        _audioSource.PlayWithVaryingPitch(_selectedSound);
    }
}
