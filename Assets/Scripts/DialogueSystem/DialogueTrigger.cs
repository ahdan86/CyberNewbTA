using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // public List<DialogueScriptableObject dialogueObjects;
    public DialogueScriptableObject dialogueObject;

    public void StartDialogue()
    {
        FindObjectOfType<DialogueManager>().OpenDialogue(dialogueObject);
    }
}
