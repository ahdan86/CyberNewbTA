using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestWrapper : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject quest;
    [SerializeField] private ObjectiveAmountDictionary objectivesAmountCompletedTracker;
    private string _interactedWith;

    private void Awake()
    {
        objectivesAmountCompletedTracker = new ObjectiveAmountDictionary();
        foreach (var objective in quest.objectives)
        {
            objectivesAmountCompletedTracker.Add(objective, 0);
        }
    }
    
    private void Start()
    {
        QuestEvent.current.onInteract.AddListener(Interact);
        QuestEvent.current.onSolve.AddListener(Solve);
        DesktopEvent.current.onOpenFile.AddListener(OpenFile);
        DesktopEvent.current.onAcceptDocument.AddListener(AcceptDocument);
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

        Debug.Log("Quest Condition Met");
        return true;
    }

    private void Interact(string name)
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
    
    private void OpenFile(string fileName)
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
    
    public void AcceptDocument()
    {
        foreach (var objective in quest.objectives)
        {
            if (objective.type == ObjectiveType.ACCEPT_FILE)
            {
                objectivesAmountCompletedTracker[objective]++;
                ObjectiveUI.Instance.UpdateObjectiveList();
            }
        }
    }

    private void Solve(int id)
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

    public int GetAmountCompletedObjective(int index)
    {
        return objectivesAmountCompletedTracker.ElementAt(index).Value;
        // return 0;
    }

    private void OnDestroy()
    {
        QuestEvent.current.onInteract.RemoveListener(Interact);
        
    }
}
