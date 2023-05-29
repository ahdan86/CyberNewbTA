using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestWrapper : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject _quest;
    private string interactedWith;
    public int amountCompleted;
    
    private void Start()
    {
        QuestEvent.current.onInteract.AddListener(Interact);
        QuestEvent.current.onSolve.AddListener(Solve);
        QuestEvent.current.test.AddListener(TestListener);
    }

    public void TestListener(string text)
    {
        Debug.Log(text);
    }

    public bool isConditionMet()
    {
        if (amountCompleted >= _quest.mustCompleted)
        {
            Debug.Log("Questnya udah selesai nich");
            return true;
        }
        else
            return false;
    }

    public void Interact(string name)
    {
        interactedWith = name;
        Debug.Log("Wrapper interactedWith: " + interactedWith);
        Debug.Log("Wrapper interactQuest: " + _quest.interactQuest);
        if (interactedWith == _quest.interactQuest)
        {
            amountCompleted++;
            ObjectiveUI.Instance.UpdateQuestList();
        }
    }

    public void Solve(int id)
    {
        if (id == (int)_quest.type)
        {
            amountCompleted++;
            ObjectiveUI.Instance.UpdateQuestList();
        }
    }

    public QuestState GetState()
    {
        return _quest.state;
    }

    public string GetDescription()
    {
        return _quest.questDescEng;
    }

    public int GetMustCompleted()
    {
        return _quest.mustCompleted;
    }

    private void OnDestroy()
    {
        QuestEvent.current.onInteract.RemoveListener(Interact);
        QuestEvent.current.onSolve.RemoveListener(Solve);
    }
}
