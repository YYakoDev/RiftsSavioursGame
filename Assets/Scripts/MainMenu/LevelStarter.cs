using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelStarter : MonoBehaviour
{
    [SerializeField] WhiteBlinkEffect _buttonFX;
    [SerializeField] AudioSource _audio;
    [SerializeField] Image i_blackScreen;
    [SerializeField] GameObject _loadingScreen;
    [SerializeField] Transform _chunks;
    [SerializeField] Vector3 _chunksStartPos, _chunksEndPos;
    AnimationCurve _movementCurve;
    bool _playerInRange = false, _moveChunks = false;
    [SerializeField]InputActionReference _interactKey;
    float _elapsedTime, _movementDuration;
    [SerializeField] float _shakeStrength;
    [SerializeField] AudioClip _buttonSfx, _gearsSfx, _movingSfx;

    private void Awake() {
        _audio = GetComponent<AudioSource>();
    }

    private void Start() {
        _movementCurve = TweenCurveLibrary.GetCurve(CurveTypes.EaseInOut);
        _movementDuration = _movingSfx.length;
        _chunks.position = _chunksStartPos;
        _interactKey.action.performed += Interact;
    }

    private void Update() {
        if(_moveChunks)
        {
            if(_elapsedTime >= _movementDuration)
            {
                _elapsedTime = 0f;
                YYInputManager.ResumeInput();
                _moveChunks = false;
                _loadingScreen.SetActive(true);
                SceneManager.LoadSceneAsync(2);
                return;
            }
            _elapsedTime += Time.deltaTime;
            float percent = _elapsedTime / (_movementDuration);
            _chunks.position = Vector3.Lerp(_chunksStartPos, _chunksEndPos, _movementCurve.Evaluate(percent));
            var newColor = i_blackScreen.color;
            newColor.a = Mathf.Lerp(-0.2f, 1.05f, _movementCurve.Evaluate(percent));
            i_blackScreen.color = newColor;
        }
    }

    void Interact(InputAction.CallbackContext obj)
    {
        if(!_playerInRange || _moveChunks) return;
        StartCoroutine(DoEffects());
        //async load the scene here!

    }
 
    IEnumerator DoEffects()
    {
        _chunks.position = _chunksStartPos;
        float buttonSfxDuration = _buttonSfx.length;
        float gearSfxDuration = _gearsSfx.length;
        //CameraShake.StopShake();
        _buttonFX.Play();
        _audio.PlayWithVaryingPitch(_buttonSfx);
        yield return new WaitForSeconds(buttonSfxDuration + 0.2f);
        _audio.PlayWithVaryingPitch(_gearsSfx);
        CameraEffects.Shake(_shakeStrength, _movementDuration - 1f);
        yield return new WaitForSeconds(0.3f);
        _audio.PlayWithVaryingPitch(_movingSfx);
        //start to move the platform here?
        _moveChunks = true;
        YYInputManager.StopInput();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    private void OnDestroy() {
        _interactKey.action.performed -= Interact;
    }
}
