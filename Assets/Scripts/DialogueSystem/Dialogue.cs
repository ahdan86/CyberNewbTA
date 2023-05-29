using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    public QuestState questState;
    public Message[] messages;
    public Actor[] actors;
}


[System.Serializable]
public class Message
{
    [FormerlySerializedAs("actorId")] public Actor actor;
    public string message;
}