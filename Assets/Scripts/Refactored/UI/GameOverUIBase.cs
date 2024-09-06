using UnityEngine;

public abstract class GameOverUIBase : MonoBehaviour
{
    [SerializeField] protected GameEventContainer eventContainer;
    [SerializeField] protected GameObject container;
    public void Retry()
    {
        eventContainer.onGameRetry.RaiseEvent();
        Time.timeScale = 1f;
    }
    public void Menu()
    {
        Application.Quit();
    }

    public virtual void Show(object currentRoundObj)
    {
        container.SetActive(true);
        Time.timeScale = 0f;
    }
}
