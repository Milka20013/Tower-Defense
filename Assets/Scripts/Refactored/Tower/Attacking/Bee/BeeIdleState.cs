using UnityEngine;

public class BeeIdleState : BeeState
{
    private float delaySeconds;
    private bool finished = false;
    public BeeIdleState(Bee bee, BeeStateMachine stateMachine) : base("idle", bee, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        finished = false;
        delaySeconds = Random.Range(1.5f, 3f);
    }

    protected override void OnReachedTarget()
    {

    }

    public override void FrameUpdate()
    {
        if (finished)
        {
            return;
        }
        if (delaySeconds <= 0f)
        {
            bee.OnStateFinished(this);
            finished = true;
            return;
        }
        delaySeconds -= Time.deltaTime;
    }


}
