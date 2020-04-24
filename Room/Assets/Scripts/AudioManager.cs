using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClipType
{
    Door,
    Phone,
    Text,
    //Click
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip clickClip;
    public AudioClip textTypingClip;
    public AudioClip phoneClip;
    public AudioClip doorClip;

    private AudioSource clickSource;
    private AudioSource textSource;
    private AudioSource objectSource;

    private void Awake()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        clickSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        textSource = gameObject.AddComponent<AudioSource>() as AudioSource;
        objectSource = gameObject.AddComponent<AudioSource>() as AudioSource;
    }

    public void PlayClickAudio()
    {
        clickSource.clip = clickClip;
        clickSource.Play();
    }
    public void PlayTextAudio()
    {
        textSource.clip = textTypingClip;
        textSource.loop = true;
        textSource.Play();
    }

    public void PlayDoorAudio()
    {
        objectSource.clip = doorClip;
        objectSource.loop = false;
        objectSource.Play();
        //objectSource.PlayOneShot(doorClip);
    }

    public void PlayPhoneAudio()
    {
        objectSource.clip = phoneClip;
        objectSource.loop = true;
        objectSource.Play();
    }

    public void StopAudioClip(ClipType clipType)
    {
        switch(clipType)
        {
            case ClipType.Door:
                objectSource.Stop();
                break;
            case ClipType.Phone:
                objectSource.Stop();
                break;
            //case ClipType.Click:
            //    objectSource.Stop();
            //    break;
            case ClipType.Text:
                textSource.Stop();
                break;
        }
    }
}
