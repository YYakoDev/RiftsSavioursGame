using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //States
    static GameStateBase CurrentGameState;
    ConvergenceState _convergenceState;
    //CraftState _craftState;
    RestState _restState;

    //Total Game Time
    float _currentRiftTime;
    public static event Action onRiftTimerEnd;
    [SerializeField]World _currentWorld;
    //[SerializeField] UpgradesMenu _upgradesMenu;


    public static event Action<GameStateBase> OnStateSwitch;
    public static event Action<GameStateBase> OnStateEnd;
    public static GameStateBase CurrentState => CurrentGameState;
    public float CurrentRiftTime => _currentRiftTime;
    public World CurrentWorld => _currentWorld;
    public ConvergenceState ConvergenceState => _convergenceState;
    //public CraftState CraftState => _craftState;
    public RestState RestState => _restState;

    // Start is called before the first frame update
    void Start()
    {
        _currentRiftTime = World.RiftDurationInSeconds;
        _convergenceState = new(this);
        //_craftState = new(this);
        _restState = new(this);

        SwitchState(_convergenceState);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentGameState.UpdateLogic();
        if(_currentRiftTime < 0)
        {
            _currentRiftTime = 0;
            onRiftTimerEnd?.Invoke();
        }
        UpdateRiftTimer();
    }

    void UpdateRiftTimer()
    {
        if(_currentRiftTime >= 0) _currentRiftTime -= Time.deltaTime;
    }

    public void SwitchState(GameStateBase state)
    {
        //Debug.Log("Switching to state:   " + state);
        OnStateEnd?.Invoke(CurrentGameState);
        CurrentGameState = state;
        CurrentGameState.Start();
        OnStateSwitch?.Invoke(CurrentGameState);
    }


}
