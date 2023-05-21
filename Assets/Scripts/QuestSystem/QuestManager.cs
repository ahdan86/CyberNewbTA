using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }
    private Dictionary<QuestState, QuestScriptableObject> activeQuests;

    private void Awake()
    {
        activeQuests = new Dictionary<QuestState, QuestScriptableObject>();
    }
    
    public void StartQuest(QuestScriptableObject quest)
    {
        if (!activeQuests.ContainsKey(quest.state))
        {
            activeQuests.Add(quest.state, quest);
        }
        else
        {
            Debug.LogWarning("Quest is already active.");
        }
    }

    public void CompleteQuest(QuestScriptableObject quest)
    {
        if (activeQuests.ContainsKey(quest.state))
        {
            activeQuests.Remove(quest.state);
        }
    }

    public bool IsQuestActive(QuestState state)
    {
        return activeQuests.ContainsKey(state);
    }
}
