using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeRift : MonoBehaviour
{
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    bool _playerInRange, _alreadyInteracted = false;
    KeyInput _interactKey;
    Timer _timer;

    private void Awake() {
        _interactKey = YYInputManager.GetKey(KeyInputTypes.Interact);
        _interactKey.OnKeyPressed += InitiateChallenge;
        _timer = new(20f);
        _timer.Stop();
        _timer.onEnd += EndChallenge;
    }

    private void Update() {
        _timer.UpdateTime();
    }

    void InitiateChallenge()
    {
        if(!_playerInRange) return;
        if(_alreadyInteracted) return;
        if(!_waveSpawner.gameObject.activeInHierarchy) _waveSpawner.gameObject.SetActive(true);
        _timer.Start();
        _waveSpawner.ResumeSpawning();
        _alreadyInteracted = true;
    }

    void EndChallenge()
    {
        _waveSpawner.StopSpawning();
        _alreadyInteracted = false;
        //destroy the rift instead of resetting it
    }

    private void OnDestroy() {
        _interactKey.OnKeyPressed -= InitiateChallenge;
        _timer.onEnd -= EndChallenge;
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
}
