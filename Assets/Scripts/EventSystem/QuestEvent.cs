using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class QuestEvent : MonoBehaviour
{
    public static QuestEvent current;
    private void Awake()
    {
        current = this;
    }

    public UnityEvent<string> onInteract;
    public void Interact(string name)
    {
        onInteract?.Invoke(name);
    }

    public UnityEvent<bool> onIsInfecting;
    public void IsInfecting(bool status)
    {
        onIsInfecting?.Invoke(status);
    }

    public UnityEvent<int> onSolve;
    public void Solve(int type)
    {
        onSolve?.Invoke(type);
    }
}
