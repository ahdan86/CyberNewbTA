using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance { get; set; }
    
    public GameObject questPrefab;
    public GameObject objectivePrefab;
    
    public Transform questContainer;
    public GameObject objectiveContainer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public void UpdateObjectiveList()
    {
        var activeQuests = QuestManager.Instance.GetActiveQuests();

        foreach (Transform child in questContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var questPair in activeQuests)
        {
            QuestWrapper quest = questPair.Value;
            GameObject questUI = Instantiate(questPrefab, questContainer);
            GameObject listObjectiveUI = Instantiate(objectiveContainer, questContainer);
            questUI.GetComponentInChildren<Text>().text = quest.GetQuestDescription();
            
            List<Objective> objectives = quest.GetObjective();
            
            foreach(var objective in objectives)
            {
                GameObject objectiveUI = Instantiate(objectivePrefab, listObjectiveUI.transform);
                objectiveUI.GetComponentInChildren<Text>().text =
                    objective.description + " " +
                    quest.GetAmountCompletedObjective(objective)
                    + "/" + objective.mustCompleted;
                objectiveUI.GetComponentInChildren<Text>().enabled = true;
            }
        }
    }
}
