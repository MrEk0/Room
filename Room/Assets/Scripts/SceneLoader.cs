using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneName
{
    StartScene,
    Home,
    Street,
    Garage
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float fadeTime;
    //[SerializeField] Color fadeOutColor;

    Image panelImage;
    float alpha=1f;

    private void Awake()
    {
        panelImage = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void LoadNextScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(FadeOut(sceneIndex));
    }

    public void LoadChoosenScene(SceneName scene)
    {
        int sceneIndex=0;
        switch (scene)
        {
            case SceneName.Home:
                sceneIndex = (int)SceneName.Home;
                break;
            case SceneName.Street:
                sceneIndex = (int)SceneName.Street;
                break;

        }
        StartCoroutine(FadeOut(sceneIndex));
    }

    private IEnumerator FadeOut(int sceneIndex)
    {
        float t = 0f;

        while (alpha != 1)
        {
            t += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, t/fadeTime);
            panelImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeIn()
    {
        float t = 1f*fadeTime;

        while (alpha > 0)
        {
            t -= Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, t/fadeTime);
            panelImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
    }
}
