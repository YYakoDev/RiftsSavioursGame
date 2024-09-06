using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrototypeRift : MonoBehaviour
{
    [SerializeField] EnemyWaveSpawner _waveSpawner;
    bool _playerInRange, _alreadyInteracted = false;
    [SerializeField]InputActionReference _interactKey;
    Timer _timer;

    private void Awake() {
        _interactKey.action.performed += InitiateChallenge;
        _timer = new(20f);
        _timer.Stop();
        _timer.onEnd += EndChallenge;
    }

    private void Update() {
        _timer.UpdateTime();
    }

    void InitiateChallenge(InputAction.CallbackContext obj)
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
        _interactKey.action.performed -= InitiateChallenge;
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
