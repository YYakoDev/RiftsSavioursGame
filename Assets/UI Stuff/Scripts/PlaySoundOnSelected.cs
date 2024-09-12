using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaySoundOnSelected : ScaleOnSelected
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _selectedSound;
    
    private void Start() {
        if(_audioSource == null) _audioSource = GetComponent<AudioSource>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        if(_audioSource == null) return;
        _audioSource.PlayWithVaryingPitch(_selectedSound);
    }
}
