using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    public SceneFader sceneFader;
    public Button[] mapButtons;
    private void Awake()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached",1);
        for (int i = 0; i < mapButtons.Length; i++)
        {
            if (i+1> levelReached)
            {
                mapButtons[i].interactable = false;
            }
        }
    }
    public void Select(string sceneName)
    {
        sceneFader.FadeTo(sceneName);
    }

}
