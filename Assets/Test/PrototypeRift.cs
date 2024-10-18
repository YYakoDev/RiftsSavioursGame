using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRift : MonoBehaviour, IInteractable
{
    [SerializeField] WaveSystem _waveSys;
    [SerializeField] Animator _animator;
    [SerializeField] AudioSource _audio;
    [SerializeField] Pillars _pillars;
    bool _alreadyInteracted = false, _doAnimation = false, _doInverse;
    [SerializeField] float _vfxsDuration = 1f, _inverseDuration = 1f;
    [SerializeField] float _lensDistortion = 0.5f, _hueShift = 150f;
    [SerializeField] CurveTypes _animCurveType, _inverseAnimCurveType;
    [SerializeField] Vector3 _buttonOffset;
    float _elapsedTime;
    AnimationCurve _animCurve, _inverseAnimCurve;
    [SerializeField] AudioClip _animationEndClip, _interactSfx, _explosionSfx, _animSfx;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }

    public Vector3 Offset => _buttonOffset;

    public AudioClip InteractSfx => _interactSfx;

    private void Awake() {    
    }
    private void Start() {
        _animCurve = TweenCurveLibrary.GetCurve(_animCurveType);
        _inverseAnimCurve = TweenCurveLibrary.GetCurve(_inverseAnimCurveType);
    }

    public void Interact()
    {
        _animator.Play("Closing");

    }

    public void PlayVFXS()
    {
        _doAnimation = true;
        _audio.PlayWithVaryingPitch(_animSfx);
        CameraEffects.Shake(1f, _vfxsDuration);
        if(_pillars != null) _pillars.PlayAnimations();
    }

    public void PlayExplosionSFX() => _audio.PlayWithVaryingPitch(_explosionSfx);

    private void Update()
    {
        if(!_doAnimation) return;
        _elapsedTime += Time.deltaTime;
        if(!_doInverse)
        {
            var percent = _elapsedTime / _vfxsDuration;
            var lensDistortion = Mathf.Lerp(0f, _lensDistortion, _animCurve.Evaluate(percent));
            var hueShift = Mathf.Lerp(0f, _hueShift, _animCurve.Evaluate(percent));
            PostProcessingManager.SetLensDistortion(lensDistortion);
            PostProcessingManager.SetHue(hueShift);
            if(percent >= 1.01f)
            {
                _elapsedTime = 0f;
                _doInverse = true;
            }
        }else
        {
            var percent = _elapsedTime / _inverseDuration;
            var lensDistortion = Mathf.Lerp(_lensDistortion, 0f, _inverseAnimCurve.Evaluate(percent));
            var hueShift = Mathf.Lerp(_hueShift, 0f, _animCurve.Evaluate(percent));
            PostProcessingManager.SetLensDistortion(lensDistortion);
            PostProcessingManager.SetHue(hueShift);
            if(percent >= 1.01f)
            {
                _audio.PlayWithVaryingPitch(_animationEndClip);
                PostProcessingManager.RestoreDefaultValues();
                //_waveSys.StartWaves();
                _doAnimation = false;
                gameObject.SetActive(false);
            }
        }


    }
}

