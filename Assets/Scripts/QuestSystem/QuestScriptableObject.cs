using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest", order = 1)]
public class QuestScriptableObject: ScriptableObject
{
    public QuestState state;
    public QuestType type;
    public string questDescEng;
    public int mustCompleted;

    [Header("Quest Interact Condition")]
    public string interactQuest;
}

public enum QuestType : int
{
    NONE = 0,
    INTERACT = 1,
    SOLVE_VIRUS = 2,
    SOLVE_PHISH = 3,
    SOLVE_QUIZ = 4
}

