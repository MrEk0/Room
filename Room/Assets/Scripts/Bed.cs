using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;

    private void OnMouseDown()
    {
        sceneLoader.UpdateDay();
        HUD.Instance.UpdateDay();
    }
}
