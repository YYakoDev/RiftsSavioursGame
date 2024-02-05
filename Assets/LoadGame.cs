using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] AsyncLoader _levelLoader;
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip _buttonSFX;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _audio?.PlayWithVaryingPitch(_buttonSFX);
            _levelLoader.LoadGame();
        }
    }
}
