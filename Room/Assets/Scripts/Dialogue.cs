using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Dialogue", menuName ="Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 10)]
    [SerializeField] string[] sentences;

    public Queue<string> GetDialogue()
    {
        Queue<string> dialogue = new Queue<string>();
        foreach (string sentence in sentences)
        {
            dialogue.Enqueue(sentence);
        }
        return dialogue;
    }
}
