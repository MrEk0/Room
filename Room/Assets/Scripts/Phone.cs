using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] GameObject dialougePanel;
    [SerializeField] GameObject cutScenePanel;

    private void OnEnable()
    {
        AudioManager.instance.PlayPhoneAudio();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            cutScenePanel.GetComponent<CutScene>().PlayFinishAnimation();
            dialougePanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        AudioManager.instance.StopAudioClip(ClipType.Phone);
    }
}
