using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance { get; set; }
    
    public Transform questContainer;
    public GameObject questObjectPrefab;
    public GameObject objectiveObjectPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
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
            GameObject questObjectUI = Instantiate(questObjectPrefab, questContainer);
            
            var objectiveContainer = questObjectUI.transform.Find("ListObjectiveContainer");
            var questText = questObjectUI.transform.Find("Quest Text").gameObject;
            questText.GetComponentInChildren<Text>().text = quest.GetQuestDescription();
            
            List<Objective> objectives = quest.GetObjective();
            
            int i = 1;
            foreach(var objective in objectives)
            {
                GameObject objectiveUI = Instantiate(objectiveObjectPrefab, objectiveContainer);
                objectiveUI.GetComponentInChildren<Text>().text =
                    i + ". " + objective.description + " " +
                    quest.GetAmountCompletedObjective(i-1)
                    + "/" + objective.mustCompleted;
                objectiveUI.GetComponentInChildren<Text>().enabled = true;
                i++;
            }
        }
    }
}
