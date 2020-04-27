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

    private void Start()
    {
        if (GameManager.instance.isCutScenePlayed)
            return;

        StartCoroutine(StartCutScene());
    }

    private IEnumerator StartCutScene()
    {
        blockClickPanel.SetActive(true);
        yield return new WaitForSeconds(timeToStart);
        animator.SetTrigger("Start");
        phone.SetActive(true);
    }

    public void PlayFinishAnimation()
    {
        animator.SetTrigger("Finish");
    }

    public void StopAnimation()
    {
        GameManager.instance.isCutScenePlayed = true;
        blockClickPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PlayerGo()
    {
        player.StartCutScene();
    }
}
