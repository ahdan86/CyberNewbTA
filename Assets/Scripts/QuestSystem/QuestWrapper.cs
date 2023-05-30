using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestWrapper : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;
    [SerializeField] private ObjectiveAmountDictionary objectivesAmountCompletedTracker;
    private string _interactedWith;

    private void Start()
    {
        QuestEvent.current.onInteract.AddListener(Interact);
        QuestEvent.current.onSolve.AddListener(Solve);
        QuestEvent.current.test.AddListener(TestListener);
        
        objectivesAmountCompletedTracker = new ObjectiveAmountDictionary();
        foreach (var objective in quest.objectives)
        {
            objectivesAmountCompletedTracker.Add(objective, 0);
        }
    }

    public void TestListener(string text)
    {
        Debug.Log(text);
    }

    public bool isConditionMet()
    {
        foreach(var objective in quest.objectives)
        {
            if (objectivesAmountCompletedTracker[objective] < objective.mustCompleted)
            {
                return false;
            }
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
            if (objective.interactQuest == _interactedWith)
            {
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
        QuestEvent.current.onSolve.RemoveListener(Solve);
    }
}
