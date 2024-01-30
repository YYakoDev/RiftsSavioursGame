using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvergenceState : GameStateCountdown
{
    int _convergencesCount = 3;
    const int MaxConvergences = 3; 

    public ConvergenceState(GameStateManager manager) : base(manager)
    {}
    public override void Start()
    {
        base.Start();
        _countdownTime = _stateManager.CurrentWorld.CurrentWave.WaveDuration;
    }
    protected override void Transition()
    {
        base.Transition();
        _stateManager.CurrentWorld.AdvanceWave();
        _countdownTime = _stateManager.CurrentWorld.CurrentWave.WaveDuration;
        _convergencesCount++;
        if(_convergencesCount >= MaxConvergences)
        {
            _stateManager.SwitchState(_stateManager.RestState);
            _convergencesCount = 0;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
