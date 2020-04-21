using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float timeToStart = 0.5f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeToStart);
        animator.SetTrigger("Start");
    }

    public void PlayFinishAnimation()
    {
        animator.SetTrigger("Finish");
    }

    public void StopAnimation()
    {
        gameObject.SetActive(false);
    }

    public void PlayerGo()
    {
        player.StartCutScene();
    }
}
