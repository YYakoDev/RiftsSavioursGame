using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainHubGameStarter : MonoBehaviour, IInteractable
{
    [SerializeField]GameObject _loadingScreen;
    private bool _alreadyInteracted;
    [SerializeField]private Vector3 _buttonOffset;

    [SerializeField]Animator _astrolabeAnimator;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] Transform _playerStandPoint;
    Transform _playerObj;
    Camera _mainCamera;
    CameraFollowAtTarget _cameraScript;
    Vector3 _startingPlayerPosition;
    float _playerSequenceDuration = 1f, _animDuration = 0.5f, _sequenceRealDuration = 1f;
    bool _playerSequenceIsRunning = false;
    Timer _playerSequenceTimer, _animationTimer;

    //audio stuff
    [SerializeField]AudioSource _audio;
    [SerializeField]AudioClip _astrolabeSfx, _astrolabeInteractSfx, _astrolabeGearStopSfx;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _buttonOffset;
    public AudioClip InteractSfx => _astrolabeInteractSfx;

    void Awake()
    {
        var clips = _astrolabeAnimator.runtimeAnimatorController.animationClips;
        var hash = Animator.StringToHash("AstrolabeAnimation");
        foreach(var clip in clips)
        {
            var clipHash = Animator.StringToHash(clip.name);
            if(clipHash == hash)
            {
                _animDuration = clip.averageDuration;
                break;
            }
        }

        _playerSequenceTimer = new(_playerSequenceDuration);
        _playerSequenceTimer.Stop();
        _playerSequenceTimer.onEnd += EndPlayerSequence;

        _animationTimer = new(_animDuration);
        _animationTimer.Stop();
        _animationTimer.onEnd += LoadGame;
    }

    private void Start() {
        YYInputManager.ResumeInput();
        _playerObj = _playerManager.transform;
        _loadingScreen.SetActive(false);
        _mainCamera = Camera.main;
        _cameraScript = _mainCamera.GetComponent<CameraFollowAtTarget>();
        _cameraScript.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        _playerSequenceTimer.UpdateTime();
        _animationTimer.UpdateTime();
        if(_playerSequenceIsRunning)
        {

            var elapsedTime = _sequenceRealDuration - _playerSequenceTimer.CurrentTime;
            var percent = elapsedTime / _sequenceRealDuration;
            var position = Vector3.Lerp(_startingPlayerPosition, _playerStandPoint.position, percent);
            _playerObj.position = position;
            _playerManager.AnimController.PlayStated(PlayerAnimationsNames.Run);
            _playerManager.MovementScript.FlipLogic.FlipCheck(-position.x);
        }
    }

    void LoadGame()
    {
        YYInputManager.ResumeInput();
        _loadingScreen.SetActive(true);
        SceneManager.LoadScene("Game");
    }

    public void Interact()
    {
        YYInputManager.StopInput();
        //play animation
        //when animation finishes, load game
        _playerSequenceTimer.Start();
        _playerSequenceIsRunning = true;
        _playerManager.RigidBody.simulated = false;
        _startingPlayerPosition = _playerObj.position;
        _cameraScript.enabled = false;
        var distance = Vector3.Distance(transform.position, _playerObj.position);
        _sequenceRealDuration = _playerSequenceDuration * Mathf.Clamp01(Mathf.Clamp01(distance) * 2);
        _playerManager.MovementScript.enabled = false;
        _playerManager.AnimController.PlayStated(PlayerAnimationsNames.Run, _sequenceRealDuration);
    }

    void EndPlayerSequence()
    {
        _playerManager.MovementScript.enabled = true;
        _playerManager.AnimController.PlayStated(PlayerAnimationsNames.Iddle);
        _playerManager.RigidBody.simulated = true;
        _playerSequenceIsRunning = false;
        _astrolabeAnimator.Play("Animation");
        YYExtensions.i.ExecuteEventAfterTime(0.09f, () =>
        {
            _audio.PlayOneShot(_astrolabeSfx);
        });
        _animationTimer.Start();
    }

    public void StopAudio()
    {
        _audio.Pause();
        _audio.Stop();
    }

    public void PlayGearStopSFX()
    {
        _audio.UnPause();
        _audio.PlayOneShot(_astrolabeGearStopSfx);
    }

    public void PlayGearSFX()
    {
        _audio.PlayOneShot(_astrolabeSfx);
    }

    private void OnDestroy() {
        _playerSequenceTimer.onEnd -= EndPlayerSequence;
        _animationTimer.onEnd -= LoadGame;
    }
}
