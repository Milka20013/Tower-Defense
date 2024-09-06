public class BeeStateMachine
{
    public BeeState currentState;

    public void Initialize(BeeState startState)
    {
        currentState = startState;
        currentState.EnterState();
    }
    public void ChangeState(BeeState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        nextState.EnterState();
    }

    public void FrameUpdate()
    {
        currentState.FrameUpdate();
    }
}
