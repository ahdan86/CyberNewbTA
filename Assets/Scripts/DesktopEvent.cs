using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DesktopEvent : MonoBehaviour
{
    public static DesktopEvent current;

    private void Awake()
    {
        current = this;
    }

    public UnityEvent<int> onInfectComputer;
    public void InfectComputer(int id)
    {
        onInfectComputer?.Invoke(id);
    }

    public UnityEvent<int> onCleanVirus;
    public void CleanVirus(int id)
    {
        onCleanVirus?.Invoke(id);
    }
    
    public UnityEvent<int, bool> onOpenDesktopUI;
    public void OpenDesktopUI(int id, bool infected)
    {
        Debug.Log("Event Computer id: " + id);
        onOpenDesktopUI?.Invoke(id, infected);
    }
}
