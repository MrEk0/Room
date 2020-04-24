using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] GameObject dialougePanel;
    [SerializeField] GameObject cutScenePanel;
    //[SerializeField] AudioClip phoneClip;

    //private AudioSource phoneSource;

    //private void Awake()
    //{
    //    phoneSource = GetComponent<AudioSource>();
    //    if(!phoneSource)
    //    {
    //        phoneSource = gameObject.AddComponent<AudioSource>();
    //    }

    //    phoneSource.clip = phoneClip;
    //    phoneSource.loop = true;
    //}

    private void OnEnable()
    {
        //phoneSource.Play();
        AudioManager.instance.PlayPhoneAudio();
    }
    //private void Start()
    //{
    //    gameObject.AddComponent<AudioSource>();
    //}

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
        //phoneSource.Stop();
        AudioManager.instance.StopAudioClip(ClipType.Phone);
    }
}
