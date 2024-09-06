

public class HealthSystem
{
    private readonly EnemyStats stats;
    private readonly EnemyAttributeContainer attributeContainer;
    public float InitialHealth { get; private set; } = 0f;
    public float CurrentHealth { get; private set; } = 0f;

    public HealthSystem(EnemyStats enemyStats, EnemyAttributeContainer attributeContainer)
    {
        stats = enemyStats;
        this.attributeContainer = attributeContainer;
        enemyStats.onValueChanged += OnInitialHealthChanged;
        OnInitialHealthChanged();
        CurrentHealth = InitialHealth;
    }

    private void OnInitialHealthChanged()
    {
        stats.TryGetAttributeValue(attributeContainer.health, out float value);
        InitialHealth = value;
    }
    public float TakeDamage(float amount)
    {
        CurrentHealth -= amount;
        return CurrentHealth;
    }

    public float Heal(float amount)
    {
        CurrentHealth += amount;
        return CurrentHealth;
    }

}
