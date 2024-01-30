using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : GameStateCountdown
{
    public RestState(GameStateManager manager) : base(manager)
    {}

    public override void Start()
    {
        base.Start();
        _countdownTime = _stateManager.CurrentWorld.RestInterval;
    }
    protected override void Transition()
    {
        base.Transition();
        _stateManager.SwitchState(_stateManager.CraftState);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
