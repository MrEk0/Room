using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject phone;
    [SerializeField] GameObject blockClickPanel;
    [SerializeField] float timeToStart = 0.5f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Start()
    {
        //GameManager.instance.PauseGame();
        blockClickPanel.SetActive(true);
        yield return new WaitForSeconds(timeToStart);
        animator.SetTrigger("Start");
        phone.SetActive(true);
        //AudioManager.instance.PlayPhoneAudio();
    }

    public void PlayFinishAnimation()
    {
        animator.SetTrigger("Finish");
    }

    public void StopAnimation()
    {
        //GameManager.instance.ResumeGame();
        blockClickPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PlayerGo()
    {
        player.StartCutScene();
    }
}
