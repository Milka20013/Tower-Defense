using UnityEngine;

public class Trickster : MonoBehaviour
{
    [SerializeField] private EnemyAttributeContainer attributeContainer;
    private EnemyStats stats;
    private float fateValue;
    private float fateThreshold;

    private void Awake()
    {
        stats = GetComponent<EnemyStats>();

        stats.TryGetAttributeValue(attributeContainer.fateThreshold, out fateThreshold);
        stats.TryGetAttributeValue(attributeContainer.luck, out float luck);

        fateValue = Random.Range(0, fateThreshold * (luck + 1));

        RegisterHealthBonus();
        RegisterSpeedBonus();
        RegisterBountyBonus();
        RegisterRank();
    }

    private void RegisterSpeedBonus()
    {
        EnemyAmplifier speedAmplifier = new()
        {
            uniqueTag = "TricksterSpeed",
            attribute = attributeContainer.speed,
            amplifierType = AmplifierType.TruePercentage,
            value = fateValue,
            stackCount = 0,
        };
        stats.RegisterAmplifiers(speedAmplifier);
    }
    private void RegisterHealthBonus()
    {
        float value = fateValue / 2;
        EnemyAmplifier healthAmplifier = new()
        {
            uniqueTag = "TricksterHealth",
            attribute = attributeContainer.health,
            amplifierType = AmplifierType.Plus,
            value = value,
            stackCount = 0,
        };
        stats.RegisterAmplifiers(healthAmplifier);
    }

    private void RegisterBountyBonus()
    {
        EnemyAmplifier bountyAmplifier = new()
        {
            uniqueTag = "TricksterBounty",
            attribute = attributeContainer.bounty,
            amplifierType = AmplifierType.TruePercentage,
            value = fateValue / 2,
            stackCount = 0,
        };
        stats.RegisterAmplifiers(bountyAmplifier);
    }

    //tricksters rank randomly gets chosen to be high or low
    private void RegisterRank()
    {
        float value = fateValue <= fateThreshold / 2 ? 0 : 100;
        EnemyAmplifier rankAmplifier = new()
        {
            uniqueTag = "TricksterRank",
            attribute = attributeContainer.rank,
            amplifierType = AmplifierType.Plus,
            value = value,
            stackCount = 0,
        };
        stats.RegisterAmplifiers(rankAmplifier);
    }


}
