using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerData : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI inputNameText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI stat1PointText;
    [SerializeField] TextMeshProUGUI stat2PointText;
    [SerializeField] TextMeshProUGUI stat3PointText;

    string name = "";

    public void SavePlayerData()
    {
        name = inputNameText.text;
        float stat1 = float.Parse(stat1PointText.text);
        float stat2 = float.Parse(stat2PointText.text);
        float stat3 = float.Parse(stat3PointText.text);

        DataSaver.Save(name, stat1, stat2, stat3);
    }

    public void ConfirmPlayerName()
    {
        name = inputNameText.text;
        nameText.text = name;
    }
}
