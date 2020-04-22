using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string playerName = "";

    private void Awake()
    {
        Instance = this;

        LoadPlayerName();
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
}
