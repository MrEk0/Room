using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SceneName
{
    StartScene,
    DarkRoom,
    LightRoom,
    Street,
    Garage
}

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float fadeTime;

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
        AudioManager.instance.PlayClickAudio();
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(FadeOut(sceneIndex));
    }

    public void LoadChoosenScene(SceneName scene)
    {
        AudioManager.instance.PlayClickAudio();
        int sceneIndex=0;
        switch (scene)
        {
            case SceneName.DarkRoom:
                sceneIndex = (int)SceneName.DarkRoom;
                break;
            case SceneName.LightRoom:
                sceneIndex = (int)SceneName.LightRoom;
                break;
            case SceneName.Street:
                sceneIndex = (int)SceneName.Street;
                break;
            case SceneName.Garage:
                sceneIndex = (int)SceneName.Garage;
                break;

        }
        StartCoroutine(FadeOut(sceneIndex));
    }

    public void UpdateDay()
    {
        SceneName sceneName = GameManager.instance.CurrentScene;

        int sceneIndex = sceneName == SceneName.DarkRoom ? (int)SceneName.LightRoom : (int)SceneName.DarkRoom;
        StartCoroutine(FadeOut(sceneIndex));
    }

    private IEnumerator FadeOut(int sceneIndex)
    {
        GameManager.instance.PauseGame();
        float t = 0f;

        while (alpha != 1)
        {
            t += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, t/fadeTime);
            panelImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }
        SceneManager.LoadScene(sceneIndex);
        GameManager.instance.UpdateCurrentScene(sceneIndex);
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

        SceneName name = GameManager.instance.CurrentScene;
        if (name == SceneName.LightRoom)
        {
            HUD.Instance.ShowGreetingPanel();
        }
        GameManager.instance.ResumeGame();
    }
}
