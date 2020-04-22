using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVPanelAnimation : MonoBehaviour
{
    [SerializeField] GameObject Hud;

    public void ShowHud()
    {
        Hud.SetActive(true);
    }
}
