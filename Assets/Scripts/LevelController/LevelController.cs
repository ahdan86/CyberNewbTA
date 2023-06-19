using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; set; }
    
    [Header("Game Core Management")]
    public List<Computer> computers;
    public ProgressBar virusProgressBar;
    
    [SerializeField] private float money;
    [SerializeField] private Text moneyText;
    [SerializeField] private float moneyReducer;
    
    [SerializeField] private int infectedComputer;
    [SerializeField] private Text computerInfectedText;
    
    private Coroutine _virusCoroutine;

    [Header("Level Management")]
    [SerializeField] private bool isInfecting;
    [SerializeField] private bool isMainStarted;

    [Header("Dialogue Trigger")]
    [SerializeField] private Dialogue infectionDialogue;
    
    [Header("Completed Quiz Dialogue")]
    [SerializeField] private List<Dialogue> completedQuizDialogues = new List<Dialogue>();

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
        moneyText.text = "$ " + money.ToString("F1");
    }

    private void Update()
    {
        if (isMainStarted)
        {
            infectedComputer = computers.Count(comp => comp.GetInfectedStatus());
            computerInfectedText.text = $"{infectedComputer} / {computers.Count}";

            //Checking komputer apakah ada yang terinfeksi
            if (infectedComputer > 0)
            {
                money -= moneyReducer * Time.deltaTime;
                moneyText.text = "$ " + money.ToString("F1");
                
                if (!isInfecting && infectedComputer < computers.Count)
                {
                    isInfecting = true;
                    _virusCoroutine = StartCoroutine(InfectProgressCoroutine());
                }
                
                if (!InfectedPanelUI.Instance.isOpen)
                {
                    QuestEvent.current.IsInfecting(true);
                    InfectedPanelUI.Instance.AnimateOpenPanel();
                    InfectedPanelUI.Instance.isOpen = true;
                    
                    FindObjectOfType<DialogueManager>().OpenDialogue(infectionDialogue);
                } 
            }
            else
            {
                money -= moneyReducer * 1.2f * Time.deltaTime;
                moneyText.text = "$ " + money.ToString("F1");
                
                if (InfectedPanelUI.Instance.isOpen)
                {
                    QuestEvent.current.IsInfecting(false);
                    InfectedPanelUI.Instance.isOpen = false;
                    InfectedPanelUI.Instance.AnimateClosePanel();
                    
                    StopCoroutine(_virusCoroutine);
                    virusProgressBar.SetProgressActive(false);
                }
            }
            
            if (money <= 0)
            {
                isMainStarted = false;
                GameManager.Instance.GameOver();
            }
        }
        if (QuestManager.Instance.GetActiveQuests().Count <= 0)
        {
            GameManager.Instance.Success(money);
        }
    }

    IEnumerator InfectProgressCoroutine()
    {
        virusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => virusProgressBar.IsProgressCompleted());

        List<Computer> availableComputers = computers.Where(comp => !comp.GetInfectedStatus()).ToList();
        if (availableComputers.Count > 0)
        {
            int randomIndex = Random.Range(0, availableComputers.Count);
            var selected = availableComputers[randomIndex];
            selected.SetInfectComputer();
        }
        virusProgressBar.SetProgressValue(0);
        isInfecting = false;
    }
    
    public void ReduceMoney(float amount)
    {
        money -= amount;
    }
    
    public void StartGame()
    {
        isMainStarted = true;
    }
    
    public void SetCompletedQuiz(Dialogue dialogue)
    {
        completedQuizDialogues.Add(dialogue);
    }
    
    public bool IsCompletedQuiz(Dialogue dialogue)
    {
        return completedQuizDialogues.Contains(dialogue);
    }

    public void StartInfectComputer(int amount)
    {
        if (amount <= computers.Count)
        {
            for (int i = 0; i < amount; i++)
            {
                computers[i].SetInfectComputer();
            }
        }
        else
        {
            Debug.Log("More than computer count");
        }
    }
}
