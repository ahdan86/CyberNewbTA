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
            if (dialogueObjectsByQuestState.ContainsKey(quest.Key))
            {
                FindObjectOfType<DialogueManager>().OpenDialogue(dialogueObjectsByQuestState[quest.Key]);
                return;
            }
        }
        FindObjectOfType<DialogueManager>().OpenDialogue(defaultDialogue);
    }
}
