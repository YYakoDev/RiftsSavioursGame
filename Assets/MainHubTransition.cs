using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHubTransition : MonoBehaviour
{
    [SerializeField]MainHubTransitionTrigger _transition;
    [SerializeField] protected Transform _transitionSpawnPoint;
    Camera _mainCamera;
    CameraFollowAtTarget _cameraScript;
    [SerializeField] FadeImage _fadeEffect;
    private void Start() {
        _mainCamera = Camera.main;
        _cameraScript = _mainCamera.GetComponent<CameraFollowAtTarget>();
        _transition._onPlayerEnter += TransitionEffect;
    }

    void TransitionEffect(Transform player)
    {
        YYInputManager.StopInput();
        _cameraScript.enabled = false;
        _fadeEffect.FadeIn(() => 
        {
            TransitionTo(player);
        });
    }
    void TransitionTo(Transform player)
    {
        YYInputManager.ResumeInput();
        var spawnPosition = _transitionSpawnPoint.position;
        player.position = spawnPosition;
        _fadeEffect.FadeOut();
        spawnPosition.z = _mainCamera.transform.position.z;
        _mainCamera.transform.position = spawnPosition;
        _cameraScript.enabled = true;
    }

    private void OnDestroy() {
        _transition._onPlayerEnter -= TransitionEffect;
    }
}
