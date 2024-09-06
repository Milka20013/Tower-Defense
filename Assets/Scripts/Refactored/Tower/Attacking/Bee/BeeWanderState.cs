using UnityEngine;

public class BeeWanderState : BeeState
{
    private Vector3 targetPosition;

    public BeeWanderState(Bee bee, BeeStateMachine stateMachine) : base("wander", bee, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        targetPosition = GetRandomPosition();
    }

    public override void FrameUpdate()
    {
        MoveToTarget(targetPosition);

    }
    protected override void OnReachedTarget()
    {
        bee.OnStateFinished(this);
    }
    private Vector3 GetRandomPosition()
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle * 5;
        float randomY = Random.Range(-2f, 5f);
        Vector3 randomPoint = new(randomPointInCircle.x, randomY, randomPointInCircle.y);
        Vector3 resultPoint = randomPoint + bee.beehivePosition;
        return resultPoint;
    }
}
