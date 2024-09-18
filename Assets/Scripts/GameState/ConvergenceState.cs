using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvergenceState : GameStateCountdown
{
    int _convergencesCount = 2;
    const int MaxConvergences = 2; 

    public ConvergenceState(GameStateManager manager) : base(manager)
    {}
    public override void Start()
    {
        base.Start();
        _countdownTime = _stateManager.CurrentWorld.CurrentWave.WaveDuration + _stateManager.DifficultyScaler.CurrentStats.Duration;
    }
    protected override void Transition()
    {
        base.Transition();
        _stateManager.CurrentWorld.AdvanceWave();
        _countdownTime = _stateManager.CurrentWorld.CurrentWave.WaveDuration + _stateManager.DifficultyScaler.CurrentStats.Duration;
        _convergencesCount++;
        if(_convergencesCount >= MaxConvergences)
        {
            _stateManager.SwitchState(_stateManager.RestState);
            _convergencesCount = 0;
        }
        else
        {
            _stateManager.SwitchState(_stateManager.ConvergenceState);
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
