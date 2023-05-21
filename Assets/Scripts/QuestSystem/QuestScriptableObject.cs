using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest", order = 1)]
public class QuestScriptableObject: ScriptableObject
{
    public QuestState state;
    public QuestType type;
    public string questDesc;
    public int amountCompleted = 0;
    public int mustCompleted = 1;

    public bool isConditionMet()
    {
        switch(state)
        {
            case QuestState.QUEST1_PHASE1_TALK_TO_EDNA:
                DialogueScriptableObject dialogue = Resources.Load<DialogueScriptableObject>("Dialogue/Dialogue1");
                return true;
            default:
                return false;
        }
    }
}

public enum QuestType : int
{
    INTERACT,
    SOLVE,
}

