using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameEventContainer eventContainer;
    [SerializeField] private TowerAmplifier[] globalTowerAmplifiers;
    [SerializeField] private EnemyAmplifier[] globalEnemyAmplifiers;


    private void Awake()
    {
        Time.timeScale = Settings.GameSpeed;
        GlobalEffects.towerAmplifiers.AddRange(globalTowerAmplifiers);
        GlobalEffects.enemyAmplifiers.AddRange(globalEnemyAmplifiers);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            eventContainer.onGamePaused.RaiseEvent();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Time.timeScale = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Time.timeScale = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            int trueNums = 0;
            for (int i = 0; i < 100000; i++)
            {
                if (Utility.RandomTrue(1 / 5000m))
                {
                    trueNums++;
                }
            }
            Debug.Log(trueNums);
        }
    }

}
