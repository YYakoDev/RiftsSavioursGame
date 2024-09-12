using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototoypeDirectionalSound : MonoBehaviour
{
    Transform _cachedTransform;
    [SerializeField] AudioSource _audio;
    [SerializeField] Transform _target;
    [SerializeField] float _distanceThreshold, _minVolume = 0.1f;
    float _updateRate = 0.1f, _nextUpdate, _defaultVolume;
    

    private void Awake() {
        if(_audio == null) _audio = GetComponent<AudioSource>();
        if(_audio == null) this.enabled = false;
        _defaultVolume = _audio.volume;
        _updateRate += Random.Range(-_updateRate / 4f, _updateRate / 4f);

        
    }

    public void SetTarget(Transform target) => _target = target;


    // Start is called before the first frame update
    void Start()
    {
        _cachedTransform = transform;
        //TEST ONLY
        _target = HelperMethods.MainCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(_nextUpdate >= 0)
        {
            _nextUpdate -= Time.deltaTime;
            if(_nextUpdate < 0)
            {
                UpdateAudio();
                _nextUpdate = _updateRate;
            }
        }
    }

    void UpdateAudio()
    {
        if(_target == null) return;
        var distance = Vector3.Distance(_cachedTransform.position, _target.position);
        if(distance > _distanceThreshold)
        {
            var diff = _distanceThreshold / distance;
            var volume = _defaultVolume * diff;
            if(volume < _minVolume) volume = _minVolume;
            _audio.volume = volume;

            var dirToTarget = _target.position - _cachedTransform.position;
            dirToTarget.Normalize();
            _audio.panStereo = -dirToTarget.x / 1.2f;

            _audio.reverbZoneMix = 1f - (0.25f - diff);
        }
        else
        {
            _audio.volume = _defaultVolume;
            _audio.panStereo = 0f;
            _audio.reverbZoneMix = 1f;
        }
    }

    private void OnDrawGizmosSelected() {
        if(_target == null) return;
        Gizmos.DrawWireSphere(_target.position, _distanceThreshold);
    }
}
