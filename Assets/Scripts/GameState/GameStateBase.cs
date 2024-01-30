using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateBase
{
    protected GameStateManager _stateManager;
    public GameStateBase(GameStateManager manager)
    {
        _stateManager = manager;
    }
    public abstract void Start();
    public abstract void UpdateLogic();
    protected abstract void Transition();
    
}
