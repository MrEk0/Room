using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData 
{
    public float stat1;
    public float stat2;
    public float stat3;

    public string name;

    public GameData(string name, float stat1, float stat2, float stat3)
    {
        this.name = name;
        this.stat1 = stat1;
        this.stat2 = stat2;
        this.stat3 = stat3;
    }
}
