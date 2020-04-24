using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public SceneName CurrentScene { get; private set; } = SceneName.StartScene;
    public bool isGamePaused { get; private set; } = false;

    private string playerName = "";

    private float amountOfStress = 5f;
    private float dayCounter = 1f;
    private float dayTime = 20f;

    private void Awake()
    {
        if (instance != this || instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadPlayerName();

        //GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        //GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
    }

    private void LoadPlayerName()
    {
        GameData playerdata = DataSaver.Load();
        if (playerdata != null)
        {
            playerName = playerdata.name;
        }
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void PauseGame()
    {
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        isGamePaused = false;
    }

    public void UpdateCurrentScene(int sceneIndex)
    {
        CurrentScene = (SceneName)sceneIndex;

        //if(CurrentScene==SceneName.LightRoom ||
        //    CurrentScene == SceneName.DarkRoom)
        //{
        //    HUD.Instance.UpdateDay();
        //}
    }

    public void SaveDayData(float dayCounter, float time)
    {
        this.dayCounter = dayCounter;
        dayTime = time;
    }

    public void SaveStress(float value)
    {
        amountOfStress = value;
    }

    public float GetStressValue()
    {
        return amountOfStress;
    }

    public float GetDayNumber()
    {
        return dayCounter;
    }

    public float GetDayTime()
    {
        return dayTime;
    }
}
