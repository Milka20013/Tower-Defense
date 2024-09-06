using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TowerStats))]
public class IncomeGenerator : MonoBehaviour
{
    private TowerStats stats;
    [SerializeField] private TowerAttributeContainer attributeContainer;
    [SerializeField] private GameEventContainer eventContainer;


    private bool tickingMoneyGenerator;
    private bool endOfRoundMoneyGenerator;

    private float tickingMoney;
    private float tickrate;
    private float tickCount;
    private bool givingTickingMoney = false;

    private float endOfRoundMoney;


    private void Awake()
    {
        stats = GetComponent<TowerStats>();

        stats.onValueChanged += OnStatValueChanged;
        OnStatValueChanged();
        StartGivingTickMoney();
    }

    private void SetTickRate(float spawningDuration)
    {
        tickrate = tickCount / spawningDuration;
        if (tickrate > 60)
        {
            tickrate = 1 / Time.deltaTime;
        }
    }
    public void OnSpawningStarted(object spawningDurationObj)
    {
        SetTickRate((float)spawningDurationObj);
        StartGivingTickMoney();
    }
    public void OnRoundOver(object _)
    {
        GiveEndOfRoundMoney();
        StopCoroutine(GiveTickingMoney());
    }

    private void OnStatValueChanged()
    {
        if (stats.TryGetAttributeValue(attributeContainer.moneyPerTick, out float moneyPerTick))
        {
            tickingMoney = moneyPerTick;
            tickingMoneyGenerator = true;
            stats.TryGetAttributeValue(attributeContainer.tickCount, out tickCount);
            if (tickCount > 8)
            {
                tickingMoney *= tickCount / 8;
                tickCount = 8;
            }
            SetTickRate(TimeManager.instance.spawningDuration);
        }
        else
        {
            tickingMoneyGenerator = false;
        }

        if (stats.TryGetAttributeValue(attributeContainer.endOfRoundMoney, out float endOfRoundMoney))
        {
            this.endOfRoundMoney = endOfRoundMoney;
            endOfRoundMoneyGenerator = true;
        }
        else
        {
            endOfRoundMoneyGenerator = false;
        }
    }

    private void StartGivingTickMoney()
    {
        if (!tickingMoneyGenerator)
        {
            return;
        }
        if (givingTickingMoney)
        {
            return;
        }
        StartCoroutine(GiveTickingMoney());
    }

    IEnumerator GiveTickingMoney()
    {
        givingTickingMoney = true;
        for (int i = 0; i < tickCount; i++)
        {
            if (!TimeManager.instance.GameIsInSpawningTime())
            {
                break;
            }
            eventContainer.onTowerIncomeGenerated.RaiseEvent(tickingMoney);
            yield return new WaitForSeconds(1 / tickrate);
        }
        givingTickingMoney = false;
    }

    private void GiveEndOfRoundMoney()
    {
        if (!endOfRoundMoneyGenerator)
        {
            return;
        }
        eventContainer.onTowerIncomeGenerated.RaiseEvent(endOfRoundMoney);
    }

    private void OnDisable()
    {
        stats.onValueChanged -= OnStatValueChanged;
    }
}
