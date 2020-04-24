using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayStation : MonoBehaviour
{
    [SerializeField] GameObject TVPanel;
    [SerializeField] Toggle toggle1;
    [SerializeField] float hoursFor1Game = 1f;
    [SerializeField] float stressFor1Game = 2f;
    [SerializeField] float hoursFor2Games = 2f;
    [SerializeField] float stressFor2Games = 4f;//improve?

    public void PlayButton()
    {
        TVPanel.SetActive(true);
        AudioManager.instance.PlayClickAudio();

        if(toggle1.isOn)
        {
            HUD.Instance.UpdateSliderValue(-stressFor1Game);
            HUD.Instance.UpdateDayTime(hoursFor1Game);
        }
        else
        {
            HUD.Instance.UpdateSliderValue(-stressFor2Games);
            HUD.Instance.UpdateDayTime(hoursFor2Games);
        }
    }
}
