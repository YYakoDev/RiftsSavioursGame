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
    Vector3 _startingPlayerPosition;
    float _playerSequenceDuration = 1f, _animDuration = 0.5f;
    bool _playerSequenceIsRunning = false;
    Timer _playerSequenceTimer, _animationTimer;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }
    public Vector3 Offset => _buttonOffset;
    public AudioClip InteractSfx => null;

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
        _playerObj = _playerManager.transform;
        _loadingScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _playerSequenceTimer.UpdateTime();
        _animationTimer.UpdateTime();
        if(_playerSequenceIsRunning)
        {
            var elapsedTime = _playerSequenceDuration - _playerSequenceTimer.CurrentTime;
            var percent = elapsedTime / _playerSequenceDuration;
            var position = Vector3.Lerp(_startingPlayerPosition, _playerStandPoint.position, percent);
            _playerObj.position = position;
            _playerManager.AnimController.PlayStated(PlayerAnimationsNames.Run, _playerSequenceDuration);
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
    }

    void EndPlayerSequence()
    {
        _playerManager.AnimController.PlayStated(PlayerAnimationsNames.Iddle);
        _playerManager.RigidBody.simulated = true;
        _playerSequenceIsRunning = false;
        _astrolabeAnimator.Play("Animation");
        _animationTimer.Start();
    }

    private void OnDestroy() {
        _playerSequenceTimer.onEnd -= EndPlayerSequence;
        _animationTimer.onEnd -= LoadGame;
    }
}