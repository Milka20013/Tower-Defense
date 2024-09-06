using UnityEngine;
[RequireComponent(typeof(EnemyStats))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;
    private EnemyStats stats;
    private EnemyMovement enemyMovement;
    private TakeDamage takeDamage;

    public EnemyBlueprint blueprint;

    [SerializeField] private EnemyAttributeContainer attributeContainer;


    [SerializeField] private GameEventContainer eventContainer;


    [HideInInspector] public bool isDead = false;
    private void Start()
    {
        stats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        takeDamage = GetComponent<TakeDamage>();
    }

    public float GetCurrentHealth()
    {
        return takeDamage.GetCurrentHealth();
    }
    public float GetRank()
    {
        stats.TryGetAttributeValue(attributeContainer.rank, out float value);
        return value;
    }
    public float GetDistanceTravelled()
    {
        return enemyMovement.distanceTravelled;
    }
    public void ReachEnd()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        stats.TryGetAttributeValue(attributeContainer.damageToPlayer, out float damage);
        PlayerStats.instance.AddLives(-Mathf.RoundToInt(damage));
        eventContainer.onEnemyExited.RaiseEvent(this);
        Destroy(gameObject);
    }
    public void Die()
    {
        if (isDead)
        {
            return;
        }
        isDead = true;
        eventContainer.onEnemyKilled.RaiseEvent(this);
        stats.TryGetAttributeValue(attributeContainer.bounty, out float money);
        PlayerStats.instance.AddMoney(money);
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        eventContainer.onEnemySelected.RaiseEvent(this);
    }
}
