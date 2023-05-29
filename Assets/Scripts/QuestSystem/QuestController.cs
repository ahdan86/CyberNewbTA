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
        questManager.StartQuest(starterQuest);
        ObjectiveUI.Instance.UpdateQuestList();
        currentQuest = starterQuest;
    }

    private void Update()
    {
        if (questManager.IsQuestActive(QuestState.QUEST1_PHASE1_TALK_TO_EDNA))
        {
            if (currentQuest.isConditionMet())
            {
                questManager.CompleteQuest(currentQuest);
                ObjectiveUI.Instance.UpdateQuestList();
                var nextQuest = questsList
                    .FirstOrDefault(
                        quest => quest.GetState() == QuestState.QUEST1_PHASE2_TALK_TO_BRYAN
                    );
                currentQuest = nextQuest;
                questManager.StartQuest(
                    nextQuest
                );
                ObjectiveUI.Instance.UpdateQuestList();
                NotificationUI.Instance.AnimatePanel("Objective Updated");
            }
        } 
        else if (questManager.IsQuestActive(QuestState.QUEST1_PHASE2_TALK_TO_BRYAN))
        {
            if (currentQuest.isConditionMet())
            {
                questManager.CompleteQuest(currentQuest);
                ObjectiveUI.Instance.UpdateQuestList();
                var nextQuest = questsList
                    .FirstOrDefault(
                        quest => quest.GetState() == QuestState.QUEST1_PHASE3_GET_FD_FROM_FRANK
                    );
                currentQuest = nextQuest;
                questManager.StartQuest(
                    nextQuest
                );
                ObjectiveUI.Instance.UpdateQuestList();
                NotificationUI.Instance.AnimatePanel("Objective Updated");
            }
        }
    }
}
