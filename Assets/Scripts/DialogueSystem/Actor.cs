using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
public class Actor: ScriptableObject
{
    public int actorId;
    [FormerlySerializedAs("name")] public string actorName;
    [FormerlySerializedAs("sprite")] public Sprite actorSprite;
}
