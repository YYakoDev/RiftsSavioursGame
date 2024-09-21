using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystemEffects : MonoBehaviour
{
    [SerializeField] WaveSystem _waveSys;
    //[SerializeField] EnemyWaveSpawner _waveSpawner;
    [SerializeField] AudioSource _audio;
    [SerializeField] float _fxDuration = 0.8f;
    [SerializeField] CurveTypes _timeSlowdownCurveType, _cameraZoomCurveType;
    [SerializeField] AudioClip _waveEndSfx;
    bool _doFx;
    float _elapsedFXTime = 0;
    AnimationCurve _timeSlowdownCurve, _cameraZoomCurve;

    private void Awake()
    {
        _waveSys.OnWaveEnd += PlayLastKillFX;
    }

    private void Start() {
        _timeSlowdownCurve = TweenCurveLibrary.GetCurve(_timeSlowdownCurveType);
        _cameraZoomCurve = TweenCurveLibrary.GetCurve(_cameraZoomCurveType);
    }


    void PlayLastKillFX()
    {
        _doFx = true;
        _waveSys.StopWaves();
        _audio.PlayWithVaryingPitch(_waveEndSfx, 0.8f);
        GameFreezer.FreezeGame(0.03f);
    }

    private void Update()
    {
        if(!_doFx) return;
        _elapsedFXTime += Time.deltaTime;
        var halvedFxDuration = _fxDuration / 2f;
        if(_elapsedFXTime <=halvedFxDuration)
        {
            var percent = _elapsedFXTime / (halvedFxDuration);
            var cameraZoom = Mathf.Lerp(0, 10, _cameraZoomCurve.Evaluate(percent));
            var slowdownStrength = Mathf.Lerp(0.9f, 0.4f, _timeSlowdownCurve.Evaluate(percent));
            TimeScaleManager.SetTimeScale(slowdownStrength);
            CameraEffects.Scale(Mathf.RoundToInt(cameraZoom), 0.05f);
        }else
        {
            var percent = (_elapsedFXTime - halvedFxDuration) / (halvedFxDuration);
            var cameraZoom = Mathf.Lerp(10, 0, _cameraZoomCurve.Evaluate(percent));
            var slowdownStrength = Mathf.Lerp(0.4f, 1f, _timeSlowdownCurve.Evaluate(percent));
            TimeScaleManager.SetTimeScale(slowdownStrength);
            CameraEffects.Scale(Mathf.RoundToInt(cameraZoom), 0.05f);
        }
        if(_elapsedFXTime > _fxDuration)
        {
            _doFx = false;
            _elapsedFXTime = 0f;
            CameraEffects.ResetScale();
            TimeScaleManager.SetTimeScale(1f);
            _waveSys.ResumeWaves();
        }
    }

    private void OnDestroy() 
    {
        _waveSys.OnWaveEnd -= PlayLastKillFX;
    }
}
