using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(GameEventListener))]
public class Supporter : MonoBehaviour
{
    [SerializeField] private SupporterBlueprint blueprint;
    [SerializeField] private TowerAttributeContainer attributeContainer;
    [SerializeField] private TowerTypeContainer typeContainer;

    private TowerStats stats;
    [SerializeField] private LayerMask towerLayer = 1 << 7;
    [SerializeField] private TowerType[] supportableTypes;
    [SerializeField] private TowerType[] nonSupportableTypes;
    private float range;


    private void Awake()
    {
        stats = GetComponent<TowerStats>();
        stats.onValueChanged += OnStatChange;
        OnStatChange();
        BuffTowers();
    }
    private void OnStatChange()
    {
        stats.TryGetAttributeValue(attributeContainer.range, out range);
    }

    private List<TowerStats> GetEligibleTowersStats()
    {
        List<TowerStats> statsList = new();
        Collider[] colliders = new Collider[100];
        int size = Physics.OverlapSphereNonAlloc(transform.position, range, colliders, towerLayer);
        for (int i = 0; i < size; i++)
        {
            if (colliders[i].TryGetComponent(out Tower tower))
            {
                if (!IsEligible(tower))
                {
                    continue;
                }
                var foundStats = colliders[i].GetComponent<TowerStats>();
                if (foundStats == this.stats)
                {
                    continue;
                }
                statsList.Add(foundStats);
            }
        }
        return statsList;
    }
    public void BuffTowers(object _ = null)
    {
        var statsList = GetEligibleTowersStats();
        foreach (var stats in statsList)
        {
            stats.RegisterAmplifiers(blueprint.amplifiers);
        }
    }

    public void DeBuffTowers(object _ = null)
    {
        var statsList = GetEligibleTowersStats();
        foreach (var stats in statsList)
        {
            stats.UnRegisterAmplifiers(blueprint.amplifiers);
        }
    }

    private bool IsEligible(Tower tower)
    {
        var bp = tower.blueprint;
        foreach (var type in bp.types)
        {
            if (nonSupportableTypes.Contains(type))
            {
                return false;
            }
        }

        if (supportableTypes.Contains(typeContainer.any))
        {
            return true;
        }
        foreach (var type in bp.types)
        {
            if (supportableTypes.Contains(type))
            {
                return true;
            }
        }
        return false;
    }

}
