using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
    public static ObjectiveUI Instance { get; set; }
    public GameObject questPrefab;
    public Transform questContainer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    public void UpdateQuestList()
    {
        var activeQuests = QuestManager.Instance.GetActiveQuests();

        foreach (Transform child in questContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var questPair in activeQuests)
        {
            Debug.Log("QuestUpdated in UI");
            QuestWrapper quest = questPair.Value;

            GameObject objectiveUI = Instantiate(questPrefab, questContainer);
            objectiveUI.GetComponentInChildren<Text>().text =
                quest.GetDescription() + " " + quest.amountCompleted + "/" + quest.GetMustCompleted();
            objectiveUI.GetComponentInChildren<Text>().enabled = true;
        }
    }
}
