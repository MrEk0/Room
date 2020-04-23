using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

[InitializeOnLoad]
public class SpriteSorter 
{
    static SpriteSorter()
    {
        SortSprite();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void SortSprite()
    {
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
    }
}
