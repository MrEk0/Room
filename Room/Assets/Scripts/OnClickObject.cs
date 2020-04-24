using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickObject : MonoBehaviour
{
    [SerializeField] GameObject panelToShow;
    private void OnMouseDown()
    {
        panelToShow.SetActive(true);
    }
}
