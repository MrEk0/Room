using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    [SerializeField] float startPoints;

    private float currentPoints;
    private TextMeshProUGUI pointsScore;

    private void Awake()
    {
        currentPoints = startPoints;
        pointsScore = GetComponent<TextMeshProUGUI>();
        pointsScore.text = currentPoints.ToString();
    }

    public void ChangeAvailablePoints(float points)
    {
        currentPoints += points;
        currentPoints = Mathf.Max(0f, currentPoints); ;
        pointsScore.text = currentPoints.ToString();
    }

    public float GetCurrentPoint()
    {
        return currentPoints;
    }
}

