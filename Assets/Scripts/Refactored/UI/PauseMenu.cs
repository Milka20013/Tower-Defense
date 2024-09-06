using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    [SerializeField] private GameEventContainer eventContainer;
    public void OpenMenu(object _)
    {
        container.SetActive(true);
        Time.timeScale = 0f;
    }

    public void CloseMenu(object _)
    {
        container.SetActive(false);
        Time.timeScale = 1f;
    }

    public void UnpauseGame()
    {
        eventContainer.onGameUnPaused.RaiseEvent();
    }

    public void RetryGame()
    {
        eventContainer.onGameRetry.RaiseEvent();
    }

    public void MenuClicked()
    {
        Application.Quit();
    }
}
