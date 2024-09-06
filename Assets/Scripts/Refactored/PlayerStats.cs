using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    private float money;
    private int lives;
    [SerializeField] private int startLives = 100;
    [SerializeField] private float startMoney = 400;

    [SerializeField] GameEventContainer eventContainer;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        AddMoney(startMoney);
        AddLives(startLives);
    }

    public float GetMoney()
    {
        return money;
    }

    public int GetLives()
    {
        return lives;
    }
    public void AddMoney(float amount)
    {
        money += amount;
        eventContainer.onMoneyAdded.RaiseEvent(amount);
        eventContainer.onMoneyChanged.RaiseEvent(money);
    }

    public void AddLives(int amount)
    {
        lives += amount;
        eventContainer.onLivesAdded.RaiseEvent(amount);
        eventContainer.onLivesChanged.RaiseEvent(lives);
        if (lives <= 0)
        {
            eventContainer.onGameLost.RaiseEvent(WaveSpawner.instance.currentRound);
        }
    }

    public void OnPseudoGameWon(object currentRound)
    {
        if (lives > 0)
        {
            eventContainer.onGameWon.RaiseEvent(currentRound);
        }
    }
}
