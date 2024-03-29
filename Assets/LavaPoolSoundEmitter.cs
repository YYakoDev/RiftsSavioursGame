using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPoolSoundEmitter : MonoBehaviour
{
    Transform _cachedTransform;
    [SerializeField] AudioSource _audio;
    [SerializeField] float _minVolume = 0.01f, _maxVolume = 0.6f, _minDistance, _maxDistance;
    Timer _distanceCheckTimer;
    float _checkInterval = 0.1f;
    [SerializeField] LayerMask _playerLayer;
    [SerializeField]float _checkRadius = 6f;
    Collider2D _player;

    private void Awake() {
        _distanceCheckTimer = new(_checkInterval, true);
        _distanceCheckTimer.onEnd += ListenerDetection;
    }

    private void Start() {
        _cachedTransform = transform;
    }

    private void Update() {
        _distanceCheckTimer.UpdateTime();
    }

    void ListenerDetection()
    {
        _player = Physics2D.OverlapCircle(_cachedTransform.position, _checkRadius, _playerLayer);
        EvaluateDistance();
    }

    void EvaluateDistance()
    {
        if(_player == null)
        {
            _audio.volume = _minVolume;
            return;
        }
        float distance = Vector3.Distance(_cachedTransform.position, _player.transform.position);
        float volume;
        if(distance < _minDistance) volume = _maxVolume;
        else if(distance >= _maxDistance) volume = _minVolume;
        else
        {
            float result = (_minVolume + _maxVolume / 4f) * 1f + (1f / distance);
            result = Mathf.Clamp(result, _minVolume, _maxVolume);
            volume = result;
        }
        _audio.volume = volume;
    }

    private void OnDestroy() {
        _distanceCheckTimer.onEnd -= ListenerDetection;
    }

    private void OnDrawGizmosSelected() {
        if(_cachedTransform == null) _cachedTransform = transform;
        Gizmos.DrawWireSphere(_cachedTransform.position, _checkRadius);
    }

}
