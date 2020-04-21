using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StatDescriptionText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject descriptionText;

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.SetActive(true);
    }
}
