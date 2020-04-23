using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    //[SerializeField] float stressForBed = 1f;
    private void OnMouseDown()
    {
        sceneLoader.UpdateDay();
        //HUD.Instance.UpdateSliderValue(stressForBed);
        HUD.Instance.UpdateDay();
    }
}
