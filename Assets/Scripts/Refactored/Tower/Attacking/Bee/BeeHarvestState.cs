using UnityEngine;

public class BeeHarvestState : BeeState
{
    private Vector3 targetPosition;
    private bool hasTarget = false;
    private bool isTargetHarvestable = true;
    private bool finished = false;
    public BeeHarvestState(Bee bee, BeeStateMachine stateMachine) : base("harvest", bee, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        finished = false;
        hasTarget = false;
        isTargetHarvestable = true;
        Collider[] colliders = new Collider[20];
        int size = Physics.OverlapSphereNonAlloc(bee.transform.position, 100f, colliders, bee.towerLayer);
        for (int i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out IHarvestable harvestable))
            {
                targetPosition = harvestable.GetHarvestPoint();
                isTargetHarvestable = true;
                hasTarget = true;
                break;
            }
        }
        if (!hasTarget)
        {
            bee.OnStateFailed(this);
            return;
        }
    }

    public override void FrameUpdate()
    {
        if (finished)
        {
            return;
        }
        if (!hasTarget)
        {
            return;
        }
        MoveToTarget(targetPosition, 1.2f);
    }

    protected override void OnReachedTarget()
    {
        HarvestOrDeploy();
    }

    private void HarvestOrDeploy()
    {
        if (isTargetHarvestable)
        {
            targetPosition = bee.beehivePosition;
            isTargetHarvestable = false;
            return;
        }
        finished = true;
        bee.Deploy();
        bee.OnStateFinished(this);
    }
}
