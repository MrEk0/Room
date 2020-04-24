using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    [SerializeField] TextMeshProUGUI leftNameText;
    [SerializeField] TextMeshProUGUI leftPersonText;
    [SerializeField] TextMeshProUGUI rightNameText;
    [SerializeField] TextMeshProUGUI rightPersonText;
    [SerializeField] Image leftPersonImage;
    [SerializeField] Image rightPersonImage;
    [SerializeField] float textVelocity = 0.5f;

    private Queue<string> startDialogue;
    private Animator animator;
    private TextMeshProUGUI dialogueText;
    private IEnumerator sentenceCoroutine;
    private string nextSentence = "";
    private bool isDialogueFinished=false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        SetUpDialogue();
    }

    private void SetUpDialogue()
    {
        leftPersonImage.sprite = dialogue.GetLeftSprite();
        rightPersonImage.sprite = dialogue.GetRightSprite();

        DialogueInitiator initiator = dialogue.GetInitiator();
        dialogueText = initiator == DialogueInitiator.Right ? leftPersonText : rightPersonText;//especially wrong behaviour to change it further

        HeroTypeName leftPerson = dialogue.GetLeftName();
        InitializePersonName(leftPerson, leftNameText);
        HeroTypeName rightPerson = dialogue.GetRightName();
        InitializePersonName(rightPerson, rightNameText);
    }

    private void InitializePersonName(HeroTypeName heroName, TextMeshProUGUI nameText)
    {
        switch(heroName)
        {
            case HeroTypeName.Player:
                nameText.text = GameManager.instance.GetPlayerName();
                break;
            case HeroTypeName.PlayerFriend:
                nameText.text = "Friend";//get name from settings
                break;
            case HeroTypeName.Father:
                nameText.text = "Father";//get name from settings
                break;
            case HeroTypeName.Mother:
                nameText.text = "Mother";//get name from settings
                break;
        }
    }

    private void Start()
    {
        GameManager.instance.PauseGame();
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
            dialogueText = dialogueText == leftPersonText ? rightPersonText : leftPersonText;
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
            GameManager.instance.ResumeGame();
        }
    }
}
