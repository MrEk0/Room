using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialogue", menuName ="Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] DialogueInitiator dialogueInitiator;
    [SerializeField] HeroTypeName leftPersonName;
    [SerializeField] HeroTypeName rightPersonName;
    [TextArea(3, 10)]
    [SerializeField] string[] sentences;
    [SerializeField] Sprite leftPersonSprite;
    [SerializeField] Sprite rightPersonSprite;

    public Queue<string> GetDialogue()
    {
        Queue<string> dialogue = new Queue<string>();
        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }
        return dialogue;
    }

    public Sprite GetLeftSprite()
    {
        return leftPersonSprite;
    }

    public Sprite GetRightSprite()
    {
        return rightPersonSprite;
    }

    public HeroTypeName GetLeftName()
    {
        return leftPersonName;
    }

    public HeroTypeName GetRightName()
    {
        return rightPersonName;
    }

    public DialogueInitiator GetInitiator()
    {
        return dialogueInitiator;
    }
}

public enum DialogueInitiator
{
    Left,
    Right
}

public enum HeroTypeName
{
    Player,
    PlayerFriend,
    Father,
    Mother
}
