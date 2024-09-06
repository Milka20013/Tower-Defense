using System.Linq;
using UnityEngine;
using UnityEngine.Events;


//filters enemies, any amount can be the result
public enum PrimaryTargetingFilter
{
    Strong, Weak, HighestHealth, LowestHealth, Any
}

//selects 1 enemy
public enum FinalTargetingFilter
{
    First, Last, Close
}

public static class TargetingExtensions
{
    public static string ToFriendlyString(this PrimaryTargetingFilter filter)
    {
        return filter switch
        {
            PrimaryTargetingFilter.Strong => "Strong",
            PrimaryTargetingFilter.Weak => "Weak",
            PrimaryTargetingFilter.HighestHealth => "High Health",
            PrimaryTargetingFilter.LowestHealth => "Low Health",
            PrimaryTargetingFilter.Any => "Any",
            _ => "None"
        };
    }

    public static string ToFriendlyString(this FinalTargetingFilter filter)
    {
        return filter switch
        {
            FinalTargetingFilter.First => "First",
            FinalTargetingFilter.Last => "Last",
            FinalTargetingFilter.Close => "Close",
            _ => "None"
        };
    }
}

[RequireComponent(typeof(TowerStats))]
public class Targeting : MonoBehaviour
{
    public FinalTargetingFilter finalTargetingFilter = FinalTargetingFilter.First;
    public PrimaryTargetingFilter primaryTargetingFilter = PrimaryTargetingFilter.Any;
    [SerializeField] LayerMask enemyLayer = 1 << 6;
    [SerializeField] private bool faceTarget;
    [SerializeField] private float turnSpeed = 20f;

    private TowerStats stats;

    [SerializeField] private TowerAttributeContainer attributeContainer;
    private float range;

    [SerializeField] private UnityEvent<Transform> onEnemyDetection;

    private void Start()
    {
        stats = GetComponent<TowerStats>();

        stats.onValueChanged += GetRangeValue;
        GetRangeValue();
        InvokeRepeating(nameof(SelectTarget), 0f, 0.08f);
    }
    public void GetRangeValue()
    {
        stats.TryGetAttributeValue(attributeContainer.range, out range);
    }

    private void SelectTarget()
    {
        var colliders = CheckForEnemies();
        var enemies = colliders.Select(x => x.GetComponent<Enemy>()).ToArray();
        Enemy[] filteredEnemies = PrimaryFilter(enemies);
        Enemy selectedEnemy = SelectFinalTarget(filteredEnemies);
        if (selectedEnemy != null)
        {
            if (faceTarget)
            {
                FaceTarget(selectedEnemy.transform);
            }
        }
        if (selectedEnemy == null)
        {
            onEnemyDetection.Invoke(null);
            return;
        }
        onEnemyDetection.Invoke(selectedEnemy.transform);
    }

    public void SetTargeting(FinalTargetingFilter finalFilter, PrimaryTargetingFilter primaryFilter)
    {
        finalTargetingFilter = finalFilter;
        primaryTargetingFilter = primaryFilter;
    }
    private Enemy[] PrimaryFilter(Enemy[] enemies)
    {
        if (enemies == null)
        {
            return null;
        }
        if (enemies.Length == 0)
        {
            return null;
        }
        Enemy[] filteredEnemies;
        switch (primaryTargetingFilter)
        {
            case PrimaryTargetingFilter.Strong:
                float max = enemies.Max(x => x.GetRank());
                filteredEnemies = enemies.Where(x => x.GetRank() == max).ToArray();
                break;
            case PrimaryTargetingFilter.Weak:
                float min = enemies.Min(x => x.GetRank());
                filteredEnemies = enemies.Where(x => x.GetRank() == min).ToArray();
                break;
            case PrimaryTargetingFilter.HighestHealth:
                float maxHealth = enemies.Max(x => x.GetCurrentHealth());
                filteredEnemies = enemies.Where(x => x.GetCurrentHealth() == maxHealth).ToArray();
                break;
            case PrimaryTargetingFilter.LowestHealth:
                float minHealth = enemies.Min(x => x.GetCurrentHealth());
                filteredEnemies = enemies.Where(x => x.GetCurrentHealth() == minHealth).ToArray();
                break;
            case PrimaryTargetingFilter.Any:
                filteredEnemies = enemies;
                break;
            default:
                filteredEnemies = enemies;
                break;
        }
        return filteredEnemies;
    }
    private Enemy SelectFinalTarget(Enemy[] enemies)
    {
        if (enemies == null)
        {
            return null;
        }
        if (enemies.Length == 0)
        {
            return null;
        }
        switch (finalTargetingFilter)
        {
            case FinalTargetingFilter.First:
                float max = enemies.Max(x => x.GetDistanceTravelled());
                return enemies.Where(x => x.GetDistanceTravelled() == max).FirstOrDefault();
            case FinalTargetingFilter.Last:
                float min = enemies.Min(x => x.GetDistanceTravelled());
                return enemies.Where(x => x.GetDistanceTravelled() == min).FirstOrDefault();
            case FinalTargetingFilter.Close:
                float closest = float.PositiveInfinity;
                int index = 0;
                for (int i = 0; i < enemies.Length; i++)
                {
                    float dist = Vector3.Distance(enemies[i].transform.position, transform.position);
                    if (closest > dist)
                    {
                        closest = dist;
                        index = i;
                    }
                }
                return enemies[index];
            default:
                Debug.LogError("Final targeting did not specify a case");
                return null;
        }
    }
    private Collider[] CheckForEnemies()
    {
        Collider[] colliders = new Collider[20];
        int numberOfColliders = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, enemyLayer);
        Collider[] hitColliders = new Collider[numberOfColliders];
        for (int i = 0; i < numberOfColliders; i++)
        {
            hitColliders[i] = colliders[i];
        }
        return hitColliders;
    }

    private void FaceTarget(Transform target)
    {
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }

    private void OnDisable()
    {
        stats.onValueChanged -= GetRangeValue;
    }
}
