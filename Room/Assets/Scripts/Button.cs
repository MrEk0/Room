using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void PlayButtonAudio()
    {
        AudioManager.instance.PlayClickAudio();
    }
}
