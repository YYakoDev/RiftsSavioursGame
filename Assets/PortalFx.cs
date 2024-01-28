using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PortalFx : MonoBehaviour
{
    AudioSource _audio;
    Timer _timer;
    [SerializeField, Range(-1f, 10f)] float _deactivateCountdown = 0.5f;
    [SerializeField] Light2D _light;
    [SerializeField]float _lightMaxIntensity = 3.55f;
    float _elapsedLightTime = 0;

    [SerializeField] AudioClip[] _openingSfxs;

    private AudioClip openingSfx => _openingSfxs[Random.Range(0, _openingSfxs.Length)];

    private void Awake() {
        if(_deactivateCountdown <= 0) this.enabled = false;
        _timer = new(_deactivateCountdown);
        _timer.onEnd += DeactivateItself;
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _elapsedLightTime = 0f;
        _light.intensity = 0f;
        _timer.Start();
        _audio?.PlayWithVaryingPitch(openingSfx);
    }
    void Update()
    {
        _timer.UpdateTime();
        if(_light.intensity >= _lightMaxIntensity) return;
        _elapsedLightTime += Time.deltaTime / _deactivateCountdown;
        float increase = Mathf.Lerp(0, _lightMaxIntensity, _elapsedLightTime);
        _light.intensity = increase;
    }
    void DeactivateItself()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        _timer.onEnd -= DeactivateItself;
    }
}
