using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRift : MonoBehaviour, IInteractable
{
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    bool _alreadyInteracted = false;
    Timer _timer;

    [SerializeField] Vector3 _buttonOffset;

    public bool AlreadyInteracted { get => _alreadyInteracted; set => _alreadyInteracted = value; }

    public Vector3 Offset => _buttonOffset;

    public AudioClip InteractSfx => null;

    private void Awake() {
        _timer = new(20f);
        _timer.Stop();
        _timer.onEnd += EndChallenge;
    }

    private void Update() {
        _timer.UpdateTime();
    }

    public void Interact()
    {
        //if(_alreadyInteracted) return;
        if(!_waveSpawner.gameObject.activeInHierarchy) _waveSpawner.gameObject.SetActive(true);
        _timer.Start();
        _waveSpawner.ResumeSpawning();
        _alreadyInteracted = true;
        gameObject.SetActive(false);
    }

    void EndChallenge()
    {
        //_waveSpawner.StopSpawning();
        //_alreadyInteracted = false;
        //destroy the rift instead of resetting it
    }

    private void OnDestroy() {
        _timer.onEnd -= EndChallenge;
    }


}
