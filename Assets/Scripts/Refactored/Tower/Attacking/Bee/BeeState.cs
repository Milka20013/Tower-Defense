using UnityEngine;

public abstract class BeeState
{
    protected string name;
    protected Bee bee;
    protected BeeStateMachine stateMachine;
    protected bool doLog = false;

    public BeeState(string name, Bee bee, BeeStateMachine stateMachine)
    {
        this.name = name;
        this.bee = bee;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
        Log("entered " + name);
    }
    public virtual void ExitState()
    {
        Log("Exited " + name);
    }

    public virtual void FrameUpdate()
    {

    }

    protected virtual void MoveToTarget(Vector3 target, float extraSpeed = 1f, float threshold = 0.1f, float turningSpeed = 1)
    {
        Vector3 direction = target - bee.transform.position;
        if (direction.magnitude <= threshold)
        {
            OnReachedTarget();
            return;
        }
        Quaternion lookRotation = Quaternion.LookRotation(target - bee.transform.position);
        lookRotation = Quaternion.RotateTowards(bee.transform.rotation, lookRotation, 90 * Time.deltaTime * turningSpeed);
        bee.transform.Translate(bee.speed * extraSpeed * Time.deltaTime * direction.normalized, Space.World);
        bee.transform.rotation = lookRotation;
    }

    protected abstract void OnReachedTarget();

    protected virtual void Log(string message)
    {
        if (!doLog)
        {
            return;
        }
        Debug.Log(message);
    }
}
