using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<QuestWrapper> questsList;
    [SerializeField] private QuestManager questManager;
    public QuestWrapper currentQuest;
    public QuestWrapper starterQuest;

    private void Start()
    {
        currentQuest = starterQuest;
        questManager.StartQuest(starterQuest);
        ObjectiveUI.Instance.UpdateObjectiveList();
    }

    private void Update()
    {
        if (currentQuest.isConditionMet())
        {
            var nextQuest = GetNextQuest();
            if (nextQuest == null)
            {
                Debug.Log("Next quest is null");
            }
            questManager.CompleteQuest(currentQuest);

            currentQuest = nextQuest;
            questManager.StartQuest(nextQuest);
            
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
