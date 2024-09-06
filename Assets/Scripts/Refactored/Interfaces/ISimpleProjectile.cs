using UnityEngine;

public interface ISimpleProjectile
{
    public void Init(TowerStats towerStats);
    public void SetTarget(Transform target);
}
