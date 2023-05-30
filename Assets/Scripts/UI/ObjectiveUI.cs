using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance { get; set; }
    public GameObject objectivePrefab;
    public Transform objectiveContainer;
    public Text questText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public void UpdateObjectiveList()
    {
        var activeQuests = QuestManager.Instance.GetActiveQuests();

        foreach (Transform child in objectiveContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var questPair in activeQuests)
        {
            QuestWrapper quest = questPair.Value;
            List<Objective> objectives = quest.GetObjective();
            questText.text = quest.GetQuestDescription();
            foreach(var objective in objectives)
            {
                GameObject objectiveUI = Instantiate(objectivePrefab, objectiveContainer);
                objectiveUI.GetComponentInChildren<Text>().text =
                    objective.description + " " +
                    quest.GetAmountCompletedObjective(objective)
                    + "/" + objective.mustCompleted;
                objectiveUI.GetComponentInChildren<Text>().enabled = true;
            }
        }
    }
}
