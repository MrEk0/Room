using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPoint : MonoBehaviour
{
    //[SerializeField] float startPoints = 10f;
    [SerializeField] GameObject pointsText;
    [SerializeField] TextMeshProUGUI statPointText;

    //private float statLevel = 0f;
    private float currentScore = 0f;
    private float availablePoints;
    Points points;

    private void Awake()
    {
        statPointText.text = currentScore.ToString();
        points = pointsText.GetComponent<Points>();
        availablePoints = points.GetCurrentPoint();
    }

    public void DecreaseStat()
    {
        points.ChangeAvailablePoints(currentScore);
        currentScore--;
        currentScore = Mathf.Max(0, currentScore);
        statPointText.text = currentScore.ToString();
    }

    public void IncreaseStat()
    {
        if (points.GetCurrentPoint() == 0f)
            return;

        currentScore++;
        statPointText.text = currentScore.ToString();
        points.ChangeAvailablePoints(-currentScore);
    }
}
