using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    public Image image;
    public bool doFade = true;
    private void Awake()
    {
        instance = this;
        doFade = Settings.SceneFadeEnabled;
        if (doFade)
        {
            StartCoroutine(FadeIn());
        }
    }
    public void FadeInOut()
    {
        FadeTo(SceneManager.GetActiveScene().name);
    }
    public void FadeTo(string SceneName)
    {
        if (doFade)
        {
            StartCoroutine(FadeOut(SceneName));
        }
        else
        {
            SceneManager.LoadScene(SceneName);
        }
    }
    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime;
            image.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
    }
    IEnumerator FadeOut(string SceneName)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            image.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
        SceneManager.LoadScene(SceneName);
    }
}
