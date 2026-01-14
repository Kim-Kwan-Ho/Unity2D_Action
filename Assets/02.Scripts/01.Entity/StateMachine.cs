public class StateMachine
{
    private EntityState _currentState;
    public EntityState CurrentState { get { return _currentState; } }
    private bool _canChangeState;

    public void Initialize(EntityState startState)
    {
        _canChangeState = true;
        _currentState = startState;
        _currentState.Enter();
    }

    public void ChangeState(EntityState newState)
    {
        if (!_canChangeState)
            return;

        _currentState.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void UpdateActiveState()
    {
        _currentState.Update();
    }
    public void SwitchOffStateMachine()
    {
        _canChangeState = false;
    }
}
