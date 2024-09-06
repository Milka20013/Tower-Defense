using UnityEngine;

//[CreateAssetMenu(menuName = "Containers/GameEvent")]
//Should be used like an enum
//Only one instance should be created
public class GameEventContainer : ScriptableObject
{
    [Header("Enemy")]
    public GameEvent onEnemyExited;
    public GameEvent onEnemyKilled;
    public GameEvent onEnemySelected;

    [Header("Game")]
    public GameEvent onGameStart;
    public GameEvent onGameOver;
    public GameEvent onGameLost;
    public GameEvent onGameWon;
    public GameEvent onGamePseudoWon;
    public GameEvent onGamePaused;
    public GameEvent onGameUnPaused;
    public GameEvent onGameRetry;

    [Header("Ingame")]
    public GameEvent onLivesChanged;
    public GameEvent onLivesAdded;
    public GameEvent onMoneyChanged;
    public GameEvent onMoneyAdded;
    public GameEvent onRoundStarted;
    public GameEvent onSpawningStarted;
    public GameEvent onRoundEnded;

    [Header("Node")]
    public GameEvent onNodeHovered;
    public GameEvent onNodeHoverExit;

    [Header("Tower")]
    public GameEvent onTowerDeselected;
    public GameEvent onTowerSelected;
    public GameEvent onTowerPlaced;
    public GameEvent onTowerMove;
    public GameEvent onTowerSold;
    public GameEvent onTowerToBuildSelected;
    public GameEvent onTowerToBuildDeSelected;
    public GameEvent onTowerIncomeGenerated;


}
