using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject doorPanel;
    //[SerializeField] AudioClip doorClip;

    //private AudioSource doorSource;

    //private void Awake()
    //{
    //    doorSource = GetComponent<AudioSource>();
    //    if(!doorSource)
    //    {
    //        doorSource = gameObject.AddComponent<AudioSource>();
    //    }

    //    //doorSource.clip = doorClip;
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>())
        {
            //doorSource.PlayOneShot(doorClip);
            //AudioManager.instance.PlayDoorAudio();
            doorPanel.SetActive(true);
        }
    }
}
