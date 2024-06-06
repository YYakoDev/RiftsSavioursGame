using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeInteractableRift : PrototypeStructure
{
    [SerializeField] Transform _player;
    //Camera _mainCamera;
    [SerializeField] EnemyWaveSpawner _enemySpawner;
    [SerializeField] FadeImage _fadeEffect;
    [SerializeField] Dropper _dropper;
    bool _destructionInitiated = false;
    float _destructionTime = 1.65f, _countdown;
    //Vector3 _previousPlayerPosition;
    Timer _challengeTimer;


    private void Awake() {
        _challengeTimer = new(21f);
        _challengeTimer.Stop();
        _challengeTimer.onEnd += EndChallenge;
    }

    private void Start() {
        if(_enemySpawner == null)
        {
            _enemySpawner = GameObject.FindObjectOfType<EnemyWaveSpawner>(true);
        }
        if(_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        _fadeEffect = GameObject.FindObjectOfType<FadeImage>(true);
        //_mainCamera = Camera.main;
        _enemySpawner.gameObject.SetActive(false);
    }

    private void Update() {
        _challengeTimer.UpdateTime();
        //Debug.Log(_challengeTimer.CurrentTime);
        if(!_destructionInitiated) return;
        if(_countdown >= 0f)
        {
            _countdown -= Time.deltaTime;
            var percent = (1f - _countdown) / _destructionTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, percent);
        }else
        {
            Destroy(gameObject);
        }
        
    }

    public override void Interact()
    {
        _challengeTimer.Start();
        _enemySpawner.gameObject.SetActive(true);
        //_previousPlayerPosition = _player.position;
        //_player.position = Vector3.zero;
        //_mainCamera.SetPosition(Vector3.zero);
        base.Interact();
        
    }

    void EndChallenge()
    {
        CameraShake.Shake(2f, 0.55f);
        _fadeEffect.FadeIn(() =>
        {
            _fadeEffect.FadeOut(duration: 0.1f);
        }, 0.2f);
        //_gameStateManager.KillEnemies();
        _enemySpawner.gameObject.SetActive(false);
        //_player.position = _previousPlayerPosition;
        //_mainCamera.SetPosition(_previousPlayerPosition);
        //AlreadyInteracted = false;
        _destructionInitiated = true;
        _countdown = _destructionTime;
        _dropper.Drop();
    }

    private void OnDestroy() {
        _challengeTimer.onEnd -= EndChallenge;
    }
}
