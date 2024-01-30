using UnityEngine;

public class GameStateCountdown : GameStateBase
{
    protected float _countdownTime;

    public GameStateCountdown(GameStateManager manager) : base(manager)
    {
    }

    protected override void Transition()
    {
        _countdownTime = 0;
    }

    public override void UpdateLogic()
    {
        _countdownTime -= Time.deltaTime;
        if(_countdownTime < 0) Transition();
    }

    public override void Start()
    {  }
}
