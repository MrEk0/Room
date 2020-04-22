using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI stat1Text;
    [SerializeField] TextMeshProUGUI stat2Text;
    [SerializeField] TextMeshProUGUI stat3Text;

    private void Awake()
    {
        UpdateHudStat();
    }

    private void UpdateHudStat()
    {
        Data statData = DataSaver.Load();
        if(statData!=null)
        {
            stat1Text.text = "Стат 1: " + statData.stat1.ToString();
            stat2Text.text = "Стат 2: " + statData.stat2.ToString();
            stat3Text.text = "Стат 3: " + statData.stat3.ToString();
        }
    }
}
