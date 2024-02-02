using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class GameStatePopup : MonoBehaviour
{
    GameStateBase _currentState;
    TweenAnimatorMultiple _animator;
    private void Awake() {
        _animator = GetComponent<TweenAnimatorMultiple>();
        GameStateManager.OnStateSwitch += SwitchedState;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchedState(GameStateBase newState)
    {
        _currentState = newState;
    }

    private void OnDestroy() {
        GameStateManager.OnStateSwitch -= SwitchedState;
    }
}
