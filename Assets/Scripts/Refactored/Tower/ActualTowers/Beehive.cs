using UnityEngine;

[RequireComponent(typeof(TowerStats))]
public class Beehive : MonoBehaviour
{
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private int numberOfBees;
    [SerializeField] private Transform spawnPoint;

    private TowerStats stats;

    private void Awake()
    {
        stats = GetComponent<TowerStats>();
    }
    private void Start()
    {
        SpawnBees();
    }

    private void SpawnBees()
    {
        for (int i = 0; i < numberOfBees; i++)
        {
            var bee = Instantiate(beePrefab, spawnPoint.position, Quaternion.identity);
            var projectile = bee.GetComponent<ISimpleProjectile>();
            projectile.Init(stats);
            projectile.SetTarget(spawnPoint);
        }
    }
}
