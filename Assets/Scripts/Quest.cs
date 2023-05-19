using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestSystem: MonoBehaviour
{
    public Quest[] quests;

    private void Awake()
    {
        quests = GetComponents<Quest>();
    }

    private void OnGUI()
    {
        foreach (var quest in quests)
        {
            // quests.DrawHUD();
        }
    }

    void Update()
    {
        foreach (var quest in quests)
        {
            if (quest.IsAchieved())
            {
                quest.Complete();
                Destroy(quest);
            }
        }
    }
}

public abstract class Quest: MonoBehaviour
{
    public string questName;
    public string questDescription;
    public bool isComplete;
    public bool isActive;

    public abstract bool IsAchieved();
    public abstract void Complete();
    public abstract void DrawHUD();
}

public class InteractQuest: Quest
{
    [SerializeField] private string _questPrompt;

    public override bool IsAchieved()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawHUD()
    {
        throw new System.NotImplementedException();
    }
}

