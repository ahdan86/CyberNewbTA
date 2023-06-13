using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<QuestWrapper> questsList;
    [SerializeField] private QuestManager questManager;
    public QuestWrapper starterQuest;
    
    private void Start()
    {
        questManager.StartQuest(starterQuest);
        QuestEvent.current.onIsInfecting.AddListener(Infecting);
        ObjectiveUI.Instance.UpdateObjectiveList();
    }

    private void Update()
    {
        foreach (var activeQuest in QuestManager.Instance.GetActiveQuests().Values.ToList())
        {
            if (activeQuest.IsConditionMet())
            {
                var nextQuest = GetNextQuest();
                if (nextQuest == null)
                {
                    if (activeQuest.GetQuestState() == QuestState.QUEST1_PHASE4_OPEN_DOCUMENT)
                    {
                        questManager.CompleteQuest(activeQuest);
                    }
                    else
                    {
                        Debug.Log("Next quest is null");
                    }
                }
                else
                {
                    questManager.CompleteQuest(activeQuest);
                    questManager.StartQuest(nextQuest);

                    ObjectiveUI.Instance.UpdateObjectiveList();
                    NotificationUI.Instance.AnimatePanel("Objective Updated");
                }
            }
        }
    }
    
    public void Infecting(bool status)
    {
        if (status)
        {
            questManager.StartQuest(questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST_CLEAN_INFECTED
                ));
            ObjectiveUI.Instance.UpdateObjectiveList();
            NotificationUI.Instance.AnimatePanel("Objective Updated");
        }
        else
        {
            questManager.CompleteQuest(questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST_CLEAN_INFECTED
                ));
            ObjectiveUI.Instance.UpdateObjectiveList();
            NotificationUI.Instance.AnimatePanel("Objective Updated");
        }
    }

    private QuestWrapper GetNextQuest()
    {
        Debug.Log("Masuk sini");
        if (questManager.IsQuestActive(QuestState.QUEST1_PHASE1_TALK_TO_EDNA))
        {
            return questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST1_PHASE2_TALK_TO_BRYAN
                );
        } 
        
        if (questManager.IsQuestActive(QuestState.QUEST1_PHASE2_TALK_TO_BRYAN))
        {
            LevelController.Instance.StartGame();
            return questsList
                .FirstOrDefault(   
                    quest => quest.GetQuestState() == QuestState.QUEST1_PHASE3_GET_FD_FROM_FRANK
                );
        }
        if(questManager.IsQuestActive(QuestState.QUEST1_PHASE3_GET_FD_FROM_FRANK))
        {
            return questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST1_PHASE4_OPEN_DOCUMENT
                );
        }
        return null;
    }
}
