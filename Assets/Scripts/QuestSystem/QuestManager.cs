using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }
    [SerializeField] private QuestDictionary activeQuests = new();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    public void StartQuest(QuestWrapper quest)
    {
        if(quest == null)
        {
            Debug.Log("Quest is Null");
        }
        if (!activeQuests.ContainsKey(quest.GetQuestState()))
        {
            activeQuests.Add(quest.GetQuestState(), quest);
        }
        else
        {
            Debug.LogWarning("Quest is already active.");
        }
    }

    public void CompleteQuest(QuestWrapper quest)
    {
        if (activeQuests.ContainsKey(quest.GetQuestState()))
        {
            activeQuests.Remove(quest.GetQuestState());
            ObjectiveUI.Instance.UpdateObjectiveList();
        }
    }

    public QuestDictionary GetActiveQuests()
    {
        return activeQuests;
    }

    public bool IsQuestActive(QuestState state)
    {
        return activeQuests.ContainsKey(state);
    }
}
