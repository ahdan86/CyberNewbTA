using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogueObjects;
    private Dictionary<QuestState, Dialogue> dialogueObjectsByQuestState;
    [SerializeField] private Dialogue defaultDialogue;

    private void Awake()
    {
        dialogueObjectsByQuestState = new Dictionary<QuestState, Dialogue>();
        foreach (var dialogueObject in dialogueObjects)
        {
            dialogueObjectsByQuestState[dialogueObject.questState] = dialogueObject;
        }
    }
    
    public void StartDialogue()
    {
        var activeQuests = QuestManager.Instance.GetActiveQuests();
        foreach (var quest in activeQuests)
        {
            if (dialogueObjectsByQuestState.TryGetValue(quest.Key, out var value))
            {
                FindObjectOfType<DialogueManager>().OpenDialogue(value);
                return;
            }
        }
        FindObjectOfType<DialogueManager>().OpenDialogue(defaultDialogue);
    }
}
