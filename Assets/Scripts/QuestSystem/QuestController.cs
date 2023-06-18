using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<QuestWrapper> questsList;
    [SerializeField] private QuestManager questManager;
    public QuestWrapper starterQuest;
    private bool _levelControllerMethodsCalled;
    
    private void Start()
    {
        questManager.StartQuest(starterQuest);
        QuestEvent.current.onIsInfecting.AddListener(Infecting);
        _levelControllerMethodsCalled = false;
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
                    else if (activeQuest.GetQuestState() == QuestState.QUEST2_PHASE3_COMPLETE_WEBCHECKS)
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
                    questManager.StartQuest(nextQuest);
                    questManager.CompleteQuest(activeQuest);
                    
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
        if (questManager.IsQuestActive(QuestState.QUEST1_PHASE1_TALK_TO_EDNA))
        {
            return questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST1_PHASE2_TALK_TO_BRYAN
                );
        } 
        
        if (questManager.IsQuestActive(QuestState.QUEST1_PHASE2_TALK_TO_BRYAN))
        {
            if (!_levelControllerMethodsCalled)
            {
                LevelController.Instance.StartGame();
                _levelControllerMethodsCalled = true;
            }
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
        if(questManager.IsQuestActive(QuestState.QUEST2_PHASE1_TALK_TO_EDNA))
        {
            return questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST2_PHASE2_TALK_TO_BRYAN
                );
        }

        if (questManager.IsQuestActive(QuestState.QUEST2_PHASE2_TALK_TO_BRYAN))
        {
            if (!_levelControllerMethodsCalled)
            {
                LevelController.Instance.StartGame();
                LevelController.Instance.StartInfectComputer(3);
                _levelControllerMethodsCalled = true;
            }
            return questsList
                .FirstOrDefault(
                    quest => quest.GetQuestState() == QuestState.QUEST2_PHASE3_COMPLETE_WEBCHECKS
                );
        }
        return null;
    }
}
