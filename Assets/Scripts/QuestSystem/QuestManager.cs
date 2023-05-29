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
        if (!activeQuests.ContainsKey(quest.GetState()))
        {
            activeQuests.Add(quest.GetState(), quest);
        }
        else
        {
            Debug.LogWarning("Quest is already active.");
        }
    }

    public void CompleteQuest(QuestWrapper quest)
    {
        if (activeQuests.ContainsKey(quest.GetState()))
        {
            activeQuests.Remove(quest.GetState());
            ObjectiveUI.Instance.UpdateQuestList();
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
