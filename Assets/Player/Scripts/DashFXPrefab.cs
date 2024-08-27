using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashFXPrefab : MonoBehaviour
{
    [SerializeField] SOPlayerStats _stats;
    AudioSource _audio;
    SpriteRenderer _renderer;
    Animator _animator;
    SODashData _dashData;
    float _animDuration = 1f;

    public SODashData DashData => _dashData;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    public void SetDashReferences(SODashData dashData)
    {
        _dashData = dashData;
        _animator.runtimeAnimatorController = dashData.DashAnimator;
        _animDuration = dashData.DashAnimator["DashAnimation"].averageDuration;
        var speed = _animDuration / _dashData.DashDuration;
        _animator.speed = speed;
        var layerID = (_dashData.AbovePlayer) ? SortingLayerManager.GetLayer(LayerType.Above) : SortingLayerManager.GetLayer(LayerType.Below);
        _renderer.sortingLayerID = layerID;
    }

    void UpdateDashDuration()
    {
        var speed = _animDuration / _dashData.DashDuration;
        _animator.speed = speed;
    }

    public void Play(Vector3 direction)
    {
        _renderer.enabled = true;
        _animator.enabled = true;
        UpdateDashDuration();
        var rot = transform.rotation.eulerAngles;
        rot.z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rot);

        YYExtensions.i.PlayAnimationWithEvent(_animator, "Animation", Stop);
        _dashData.PlayFX();
        _audio.PlayOneShot(_dashData.DashSfx);
    }

    void Stop()
    {
        //_animator.enabled = false;
        _renderer.enabled = false;
        _dashData.StopFX();
    }
}
