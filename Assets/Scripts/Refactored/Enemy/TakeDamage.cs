using UnityEngine;
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyStats))]
public class TakeDamage : MonoBehaviour, IDamageable
{
    private Enemy enemy;

    [SerializeField] private EnemyAttributeContainer attributeContainer;
    [SerializeField] private GameObject hitEffect;

    private HealthSystem healthSystem;
    public delegate void HitHandler(EnemyHitInfo hitInfo);
    public delegate void HealthChangeHandler(float currentHealth);
    public HealthChangeHandler onHealthChanged;
    public HitHandler onHit;

    private void Awake()
    {
        enemy = gameObject.GetComponent<Enemy>();
    }

    private void Start()
    {
        healthSystem = new(GetComponent<EnemyStats>(), attributeContainer);
    }

    public float GetCurrentHealth()
    {
        return healthSystem.CurrentHealth;
    }

    public void Heal(float amount)
    {
        float newHealth = healthSystem.Heal(amount);
        onHealthChanged?.Invoke(newHealth);
    }
    public void OnHit(float damage)
    {
        float remainingHealth = healthSystem.TakeDamage(damage);
        onHealthChanged?.Invoke(remainingHealth);
        onHit?.Invoke(new(damage, damage, remainingHealth));
        if (hitEffect != null)
        {
            var effect = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(effect, 2f);
        }
        if (remainingHealth <= 0f)
        {
            enemy.Die();
        }
    }

    public bool IsDead()
    {
        return enemy.isDead;
    }
}

public struct EnemyHitInfo
{
    public float damageGiven;
    public float damageTaken;
    public float remainingHealth;

    public EnemyHitInfo(float damageGiven, float damageTaken, float remainingHealth)
    {
        this.damageGiven = damageGiven;
        this.damageTaken = damageTaken;
        this.remainingHealth = remainingHealth;
    }
}