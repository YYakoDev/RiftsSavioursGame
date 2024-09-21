using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PortalFx : MonoBehaviour
{
    AudioSource _audio;
    //Timer _timer;
    [SerializeField, Range(-1f, 10f)] float _deactivateCountdown = 0.5f;
    //[SerializeField] Light2D _light;
    //[SerializeField]float _lightMaxIntensity = 3.55f;
    float _countdownTime;

    [SerializeField] AudioClip[] _openingSfxs;

    private AudioClip openingSfx => _openingSfxs[Random.Range(0, _openingSfxs.Length)];

    private void Awake()
    {
        if (_deactivateCountdown <= 0) this.enabled = false;
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _countdownTime = 0f;
        _audio?.PlayWithVaryingPitch(openingSfx);
    }
    void Update()
    {
        _countdownTime += Time.deltaTime;
        if (_countdownTime >= _deactivateCountdown)
        {
            DeactivateItself();
        }
        /*if (_light.intensity >= _lightMaxIntensity) return;
        _elapsedLightTime += Time.deltaTime / _deactivateCountdown;
        float increase = Mathf.Lerp(0, _lightMaxIntensity, _elapsedLightTime);
        _light.intensity = increase;*/
    }

    public void PlaySpawnSequence(GameObject enemyToActivate)
    {
        //enemyToActivate.transform.position = transform.position - Vector3.down;
    }

    void DeactivateItself()
    {
        gameObject.SetActive(false);
    }
}
