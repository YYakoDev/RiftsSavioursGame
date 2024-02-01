using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatePopup : MonoBehaviour
{
    GameStateBase _currentState;
    private void Awake() {
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
