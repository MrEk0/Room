using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    //[SerializeField] 
    [SerializeField] TextMeshProUGUI stat1Text;
    [SerializeField] TextMeshProUGUI stat2Text;
    [SerializeField] TextMeshProUGUI stat3Text;

    //private string stat1 = "Стат 1: ";
    //private string stat2 = "Стат 2: ";
    //private string stat3 = "Стат 3: ";

    private void Awake()
    {
        LoadPlayerStat();
    }

    private void LoadPlayerStat()
    {
        GameData statData = DataSaver.Load();
        if(statData!=null)
        {
            stat1Text.text = "Стат 1: " + statData.stat1.ToString();
            stat2Text.text = "Стат 2: " + statData.stat2.ToString();
            stat3Text.text = "Стат 3: " + statData.stat3.ToString();
        }
    }

    //public void UpdateStat()
    //{
    //    stat1Text.text
    //}
}
