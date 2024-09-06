using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [HideInInspector] public float spawningDuration;
    [HideInInspector] public float currentRoundTime;

    private bool calculateRoundTime;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of" + instance.name);
        }
        instance = this;
    }

    private void Update()
    {
        if (calculateRoundTime)
        {
            currentRoundTime += Time.deltaTime;
        }
    }

    public bool GameIsInSpawningTime()
    {
        return (spawningDuration - currentRoundTime > 0f) && calculateRoundTime;
    }
    public void OnSpawningStarted(object spawningDurationObj)
    {
        spawningDuration = (float)spawningDurationObj;
        currentRoundTime = 0f;
        calculateRoundTime = true;
    }

    public void OnRoundOver(object _)
    {
        calculateRoundTime = false;
    }
}
