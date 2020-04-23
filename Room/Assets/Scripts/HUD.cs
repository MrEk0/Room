using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] Slider stressSlider;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI dayCounterText;
    [SerializeField] TextMeshProUGUI stat1Text;
    [SerializeField] TextMeshProUGUI stat2Text;
    [SerializeField] TextMeshProUGUI stat3Text;
    [SerializeField] GameObject GreetingPanel;

    float sliderValue;
    float dayCounter=0f;
    float dayTime = 20f;

    private void Awake()
    {
        Instance = this;

        //sliderValue = stressSlider.value;

        sliderValue = GameManager.instance.GetStressValue();
        dayCounter= GameManager.instance.GetDayNumber();
        dayTime = GameManager.instance.GetDayTime();
        stressSlider.value = sliderValue;
        LoadPlayerStat();
        UpdateText();
        //UpdateDay();
    }

    private void UpdateText()
    {
        //stressSlider.value = sliderValue;
        dayCounterText.text = "День: " + dayCounter;
        timeText.text = "Время: " + dayTime + ":00";
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

    public void UpdateDay()
    {
        //sliderValue++;
        dayCounter++;
        dayTime = 14f;

        UpdateText();
        GameManager.instance.SaveDayData(dayCounter, dayTime);
    }

    public void UpdateSliderValue(float value)
    {
        sliderValue = Mathf.Max(0, sliderValue += value);
        stressSlider.value = sliderValue;

        GameManager.instance.SaveStress(sliderValue);
    }

    public void ShowGreetingPanel()
    {
        GreetingPanel.SetActive(true);
    }
}
