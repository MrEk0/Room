using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI friendText;
    [SerializeField] Dialogue dialogue;
    [SerializeField] float textVelocity = 0.5f;

    private Queue<string> startDialogue;
    private Animator animator;
    private TextMeshProUGUI dialogueText;
    private IEnumerator sentenceCoroutine;
    private string nextSentence = "";
    private bool isDialogueFinished=false;

    private void Awake()
    {
        PlayerName();
        animator = GetComponent<Animator>();
    }

    private void PlayerName()
    {
        Data playerdata = DataSaver.Load();
        if (playerdata != null)
        {
            playerName.text = playerdata.name;
        }
    }

    public void StartDialogue()
    {
        StartCoroutine(TypeDialogue());
    }

    private IEnumerator TypeDialogue()
    {
        startDialogue = dialogue.GetDialogue();
        int sentenceCount = startDialogue.Count;

        for (int i = 0; i < sentenceCount; i++)
        {
            nextSentence = startDialogue.Dequeue();
            dialogueText = i % 2 == 0 ? playerText : friendText;
            sentenceCoroutine = TypeSentence(nextSentence, dialogueText);
            StartCoroutine(sentenceCoroutine);

            float waitTime = CalculateWaitTime(nextSentence);
            yield return new WaitForSeconds(waitTime);
        }

        isDialogueFinished = true;
        CloseWindow();
    }

    private void CloseWindow()
    {
        animator.SetTrigger("DialogueFinished");
    }

    private float CalculateWaitTime(string sentence)
    {
        float waitTime = 0f;

        foreach (char letter in sentence)
        {
            waitTime += textVelocity;
        }

        return waitTime;
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI dialogueText)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
           dialogueText.text += letter;
           yield return new WaitForSeconds(textVelocity);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (isDialogueFinished)
            {
                CloseWindow();
            }
            else
            {
                FinishAllText();
            }
        }


    }

    private void FinishAllText()
    {
        StopCoroutine(sentenceCoroutine);

        dialogueText.text = "";
        foreach (char letter in nextSentence.ToCharArray())
        {
            dialogueText.text += letter;
        }

        if(startDialogue.Count==0)
        {
            isDialogueFinished = true;
        }
    }
}
