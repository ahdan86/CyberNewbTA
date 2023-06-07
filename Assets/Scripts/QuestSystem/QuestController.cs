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
        ObjectiveUI.Instance.UpdateObjectiveList();
    }

    private void Update()
    {
        foreach (var currentQuest in QuestManager.Instance.GetActiveQuests().Values.ToList())
        {
            if (currentQuest.isConditionMet())
            {
                var nextQuest = GetNextQuest();
                if (nextQuest == null)
                {
                    if (currentQuest.GetQuestState() == QuestState.QUEST1_PHASE4_OPEN_DOCUMENT)
                    {
                        questManager.CompleteQuest(currentQuest);
                        //TODO: GAME IS OVER: SUCCESS
                    }
                    else if (currentQuest.GetQuestState() == QuestState.QUEST1_PHASE5_CLEAN_INFECTED)
                    {
                        questManager.CompleteQuest(currentQuest);
                        NotificationUI.Instance.AnimatePanel("All Computers Cleaned");
                        //TODO: Break the update loop
                    }
                    else
                    {
                        Debug.Log("Next quest is null");
                    }
                }
                else
                {
                    questManager.CompleteQuest(currentQuest);
                    questManager.StartQuest(nextQuest);

                    ObjectiveUI.Instance.UpdateObjectiveList();
                    NotificationUI.Instance.AnimatePanel("Objective Updated");
                }
            }
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
        if(questManager.IsQuestActive(QuestState.QUEST1_PHASE4_OPEN_DOCUMENT))
        {
            return questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST1_PHASE5_CLEAN_INFECTED
                );
        }
        return null;
    }
}
