using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest System/Quest", order = 1)]
public class QuestScriptableObject: ScriptableObject
{
    public QuestState state;
    public List<Objective> objectives;
    public string questDescEng;
}

public enum ObjectiveType : int
{
    NONE = 0,
    INTERACT = 1,
    SOLVE = 2,
    CLEAN_INFECTED = 3,
    OPEN_FILE = 4,
    RUN_FILE = 5,
}

[Serializable]
public class Objective
{
    public ObjectiveType type;
    public string description;
    public int mustCompleted;

    [Header("Quest Condition")]
    public string interactQuest;
    public string openFileQuest;
    
}


            