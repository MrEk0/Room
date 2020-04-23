using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GreetingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI greetingText;

    private void Start()
    {
        string playerName = GameManager.instance.GetPlayerName();

        greetingText.text = playerName + " прибыл из школы";
    }
}
