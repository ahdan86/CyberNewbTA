using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class DialogueScriptableObject : ScriptableObject
{
    public QuestState questState;
    public Message[] messages;
    public Actor[] actors;
}


[System.Serializable]
public class Message
{
    public int actorId;
    public string message;
}

[System.Serializable]
public class Actor
{
    public int actorId;
    public string name;
    public Sprite sprite;
}
