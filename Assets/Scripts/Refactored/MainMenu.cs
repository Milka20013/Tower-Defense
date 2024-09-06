using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string LevelToLoad = "MainLevel";
    public SceneFader SceneFader;
    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneFader.FadeTo(LevelToLoad);
        Time.timeScale = 1f;
    }
}
