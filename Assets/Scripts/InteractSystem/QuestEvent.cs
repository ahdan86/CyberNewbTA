using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public UnityEvent<int> onSolve;
    public void Solve(int type)
    {
        onSolve?.Invoke(type);
    }

    public UnityEvent<string> test;
    public void TestListener(string text)
    {
        test?.Invoke(text);
    }
}
