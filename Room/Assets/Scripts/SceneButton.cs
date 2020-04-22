using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
    [SerializeField] SceneName sceneTypeToGo;
    [SerializeField] SceneLoader sceneLoader;

    public void LoadScene()
    {
        sceneLoader.LoadChoosenScene(sceneTypeToGo);
    }
}
