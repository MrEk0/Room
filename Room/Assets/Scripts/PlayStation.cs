using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayStation : MonoBehaviour
{
    [SerializeField] Slider stressSlider;
    [SerializeField] GameObject TVPanel;
    [SerializeField] Toggle toggle1;
    [SerializeField] float stressFor1Game = 2f;
    [SerializeField] float stressFor2Games = 4f;

    public void PlayButton()
    {
        TVPanel.SetActive(true);

        float sliderValue = stressSlider.value;

        if(toggle1.isOn)
        {
            sliderValue = Mathf.Max(0, sliderValue -= stressFor1Game);
            stressSlider.value = sliderValue;
        }
        else
        {
            sliderValue = Mathf.Max(0, sliderValue -= stressFor2Games);
            stressSlider.value = sliderValue;
        }
    }
}
