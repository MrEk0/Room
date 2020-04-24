using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPoint : MonoBehaviour
{
    [SerializeField] GameObject pointsText;
    [SerializeField] TextMeshProUGUI statPointText;

    private float currentScore = 0f;
    Points points;

    private void Awake()
    {
        statPointText.text = currentScore.ToString();
        points = pointsText.GetComponent<Points>();
    }

    public void DecreaseStat()
    {
        AudioManager.instance.PlayClickAudio();
        points.ChangeAvailablePoints(currentScore);
        currentScore--;
        currentScore = Mathf.Max(0, currentScore);
        statPointText.text = currentScore.ToString();
    }

    public void IncreaseStat()
    {
        AudioManager.instance.PlayClickAudio();
        if (points.GetCurrentPoint() == 0f || points.GetCurrentPoint()<currentScore)
            return;

        currentScore++;
        statPointText.text = currentScore.ToString();
        points.ChangeAvailablePoints(-currentScore);
    }
}
