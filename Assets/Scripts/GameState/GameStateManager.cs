using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    //States
    GameStateBase _currentState;
    ConvergenceState _convergenceState;
    CraftState _craftState;
    RestState _restState;
    public static event Action<float> OnRestStart;

    //Total Game Time
    float _currentRiftTime;
    public static event Action onRiftTimerEnd;
    [SerializeField]World _currentWorld;
    [SerializeField] UpgradesMenu _upgradesMenu;


    public float CurrentRiftTime => _currentRiftTime;
    public World CurrentWorld => _currentWorld;
    public UpgradesMenu UpgradesMenu => _upgradesMenu;
    public ConvergenceState ConvergenceState => _convergenceState;
    public CraftState CraftState => _craftState;
    public RestState RestState => _restState;

    // Start is called before the first frame update
    void Start()
    {
        _currentRiftTime = World.RiftDurationInSeconds;
        _convergenceState = new(this);
        _craftState = new(this);
        _restState = new(this);

        SwitchState(_convergenceState);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateLogic();
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
        Debug.Log("Switching to state:   " + state);
        if(state.GetType() == typeof(RestState)) OnRestStart?.Invoke(_currentWorld.RestInterval);
        _currentState = state;
        _currentState.Start();
    }


}
