using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuSounds : MonoBehaviour
{
    [SerializeField]AudioSource _audio;
    [SerializeField]AudioClip _toolsMoving, _toolsOut;
    private void Awake() {
        gameObject.CheckComponent<AudioSource>(ref _audio);
    }
    
    public void PlayToolsSound()
    {
        _audio.PlayWithVaryingPitch(_toolsMoving);
    }

    public void PlayToolMovingOut()
    {
        _audio.PlayWithVaryingPitch(_toolsOut);
    }
}
