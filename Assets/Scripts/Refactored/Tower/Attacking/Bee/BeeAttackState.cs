using UnityEngine;

public class BeeAttackState : BeeState
{
    private Transform target;
    private bool finished = false;

    private float accelaration = 1f;
    public BeeAttackState(Bee bee, BeeStateMachine stateMachine) : base("attack", bee, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        finished = false;
        accelaration = 1f;
        Collider[] colliders = new Collider[1];
        int size = Physics.OverlapSphereNonAlloc(bee.transform.position, bee.detectionRange, colliders, bee.enemyLayer);
        if (size == 0)
        {
            bee.OnStateFailed(this);
            return;
        }
        target = colliders[0].transform;
    }

    public override void ExitState()
    {
        base.ExitState();
        finished = true;
    }

    public override void FrameUpdate()
    {
        if (finished)
        {
            return;
        }
        if (target == null)
        {
            bee.OnStateFailed(this);
            return;
        }
        MoveToTarget(target.position, accelaration * 2.5f, accelaration, 1.75f);
        accelaration += Time.deltaTime * 0.25f;
    }

    protected override void OnReachedTarget()
    {
        Attack();
        bee.OnStateFinished(this);
    }

    private void Attack()
    {
        if (target.TryGetComponent(out IDamageable damageable))
        {
            damageable.OnHit(bee.damage);
        }
        finished = true;
    }
}
