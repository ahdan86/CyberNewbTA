using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Actor", menuName = "ScriptableObjects/Actor", order = 1)]
public class Actor: ScriptableObject
{
    public int actorId;
    public string name;
    public Sprite sprite;
}
