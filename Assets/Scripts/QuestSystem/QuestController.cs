using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public QuestScriptableObject starterQuest;

    private void Start()
    {
        QuestManager.Instance.StartQuest(starterQuest);
    }

    private void Update()
    {
        if (QuestManager.Instance.IsQuestActive(QuestState.QUEST1_PHASE1_TALK_TO_EDNA))
        {
            
        }
    }
}
