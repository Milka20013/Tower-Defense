using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Reloading))]
public class TowerAttacking : MonoBehaviour
{
    private Transform target;
    private bool readyToAttack = false;

    [SerializeField] private UnityEvent<Transform> onAttack;

    public void OnTargetDetection(Transform target)
    {
        if (target == null)
        {
            this.target = null;
            return;
        }
        this.target = target;
        TryAttack();
    }

    public void OnReadyToAttack()
    {
        readyToAttack = true;
        TryAttack();
    }

    private void TryAttack()
    {
        if (!readyToAttack)
        {
            return;
        }
        if (target == null)
        {
            return;
        }
        onAttack.Invoke(target);
        readyToAttack = false;
    }
}
