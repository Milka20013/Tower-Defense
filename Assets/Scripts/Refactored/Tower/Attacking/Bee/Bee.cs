using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bee : MonoBehaviour, ISimpleProjectile
{
    [SerializeField] private TowerAttributeContainer attributeContainer;
    [SerializeField] private GameEventContainer eventContainer;

    public LayerMask enemyLayer = 1 << 6;
    public LayerMask towerLayer = 1 << 7;

    [HideInInspector] public Vector3 beehivePosition;
    [HideInInspector] public float damage;
    [HideInInspector] public float speed;
    [HideInInspector] public float detectionRange;
    private float initialTickCount;
    private float tickCount;
    private float moneyPerTick;

    private BeeStateMachine stateMachine;
    private BeeIdleState idleState;
    private BeeAttackState attackState;
    private BeeWanderState wanderState;
    private BeeHarvestState harvestState;
    public void Init(TowerStats towerStats)
    {
        towerStats.TryGetAttributeValue(attributeContainer.damage, out damage);
        towerStats.TryGetAttributeValue(attributeContainer.projectileSpeed, out speed);
        towerStats.TryGetAttributeValue(attributeContainer.projectileSize, out detectionRange);
        towerStats.TryGetAttributeValue(attributeContainer.tickCount, out initialTickCount);
        tickCount = initialTickCount;
        towerStats.TryGetAttributeValue(attributeContainer.moneyPerTick, out moneyPerTick);


        stateMachine = new();
        idleState = new(this, stateMachine);
        wanderState = new(this, stateMachine);
        attackState = new(this, stateMachine);
        harvestState = new(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    public void OnRoundStarted(object _)
    {
        tickCount = initialTickCount;
    }

    private void Update()
    {
        stateMachine.FrameUpdate();
    }

    public void SetTarget(Transform transform)
    {
        beehivePosition = transform.position;
    }

    public void OnStateFinished(BeeState state)
    {
        stateMachine.ChangeState(GetRandomState(state));
    }

    public void OnStateFailed(BeeState state)
    {
        StartCoroutine(DelayedStateChange(state));
    }

    IEnumerator DelayedStateChange(BeeState state)
    {
        yield return null;
        OnStateFinished(state);
    }
    private BeeState GetRandomState(BeeState previousState)
    {
        List<BeeState> states = new() { idleState, wanderState, attackState, harvestState };
        if (tickCount <= 0)
        {
            states.Remove(harvestState);
        }
        states.Remove(previousState);
        int index = Random.Range(0, states.Count);
        return states[index];
    }

    public void Deploy()
    {
        eventContainer.onTowerIncomeGenerated.RaiseEvent(moneyPerTick * 2);
        tickCount--;
    }
}
