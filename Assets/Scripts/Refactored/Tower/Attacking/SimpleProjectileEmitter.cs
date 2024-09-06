using UnityEngine;

[RequireComponent(typeof(TowerAttacking))]
public class SimpleProjectileEmitter : MonoBehaviour
{
    [SerializeField] private Transform shootingPosition;
    [SerializeField] private GameObject projectilePrefab;

    private TowerStats stats;
    private void Start()
    {
        stats = GetComponent<TowerStats>();
    }
    public void OnAttack(Transform target)
    {
        var projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        ISimpleProjectile simpleProjectile = projectile.GetComponent<ISimpleProjectile>();
        simpleProjectile.Init(stats);
        simpleProjectile.SetTarget(target);
    }
}
