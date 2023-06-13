using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestWrapper : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;
    [SerializeField] private ObjectiveAmountDictionary objectivesAmountCompletedTracker;
    [SerializeField] private Inventory inventory;
    private string _interactedWith;

    private void Start()
    {
        QuestEvent.current.onInteract.AddListener(Interact);
        QuestEvent.current.onSolve.AddListener(Solve);
        QuestEvent.current.onOpenFile.AddListener(OpenFile);

        objectivesAmountCompletedTracker = new ObjectiveAmountDictionary();
        foreach (var objective in quest.objectives)
        {
            objectivesAmountCompletedTracker.Add(objective, 0);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public bool IsConditionMet()
    {
        foreach(var objective in quest.objectives)
        {
            if (objectivesAmountCompletedTracker[objective] < objective.mustCompleted)
            {
                return false;
            }
        }

        if (quest.state == QuestState.QUEST1_PHASE3_GET_FD_FROM_FRANK)
        {
            inventory.SetHasContamined(true);
        }
        
        Debug.Log("Quest Condition Met");
        return true;
    }

    public void Interact(string name)
    {
        _interactedWith = name;
        Debug.Log("Wrapper interactedWith: " + _interactedWith);
        foreach (var objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.INTERACT && objective.interactQuest == _interactedWith)
            {
                objectivesAmountCompletedTracker[objective]++;
                ObjectiveUI.Instance.UpdateObjectiveList();
            }
        }
    }
    
    public void OpenFile(string fileName)
    {
        foreach (var objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.OPEN_FILE && fileName == objective.openFileQuest)
            {
                Debug.Log($"Berhasil Open File {fileName}, Condition: {objective.openFileQuest}");
                objectivesAmountCompletedTracker[objective]++;
                ObjectiveUI.Instance.UpdateObjectiveList();
            }
        }
    }

    public void Solve(int id)
    {
        foreach (var objective in quest.objectives)
        {
            if (id == (int)objective.type)
            {
                objectivesAmountCompletedTracker[objective]++;
                ObjectiveUI.Instance.UpdateObjectiveList();
            }
        }
    }

    public QuestState GetQuestState()
    {
        return quest.state;
    }

    public string GetQuestDescription()
    {
        return quest.questDescEng;
    }

    public List<Objective> GetObjective()
    {
        return quest.objectives;
    }

    public int GetAmountCompletedObjective(Objective objective)
    {
        //return objectivesAmountCompletedTracker[objective];
        return 0;
    }

    private void OnDestroy()
    {
        QuestEvent.current.onInteract.RemoveListener(Interact);
        
    }
}
