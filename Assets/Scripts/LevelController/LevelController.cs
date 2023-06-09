using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; set; }
    public List<Computer> computers;
    public ProgressBar virusProgressBar;
    
    [SerializeField] private float money;
    [SerializeField] private Text moneyText;
    [SerializeField] private float moneyReducer;
    
    [SerializeField] private int infectedComputer;
    [SerializeField] private Text computerInfectedText;

    [SerializeField] private bool isInfecting;
    [FormerlySerializedAs("isStarted")] [SerializeField] private bool isMainStarted = false;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    private void Start()
    {
        moneyText.text= money.ToString();
    }

    private void Update()
    {
        if (isMainStarted)
        {
            //Reduce money overtime
            money -= moneyReducer * Time.deltaTime;
            moneyText.text = money.ToString("F2");
            
            infectedComputer = computers.Count(comp => comp.GetInfectedStatus());
            computerInfectedText.text = $"{infectedComputer} / {computers.Count}";

            //Checking komputer lain
            if (infectedComputer > 0)
            {
                if (!isInfecting && infectedComputer < computers.Count)
                {
                    isInfecting = true;
                    StartCoroutine(InfectProgressCoroutine());
                }
                
                if (!InfectedPanelUI.Instance.isOpen)
                {
                    InfectedPanelUI.Instance.AnimateOpenPanel();
                    InfectedPanelUI.Instance.isOpen = true;
                } 
            }
            else
            {
                if (InfectedPanelUI.Instance.isOpen)
                {
                    InfectedPanelUI.Instance.isOpen = false;
                    InfectedPanelUI.Instance.AnimateClosePanel();
                }
            }
            
            if (money <= 0)
            {
                isMainStarted = false;
                GameManager.Instance.GameOver();
            }
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
    
    public void StartGame()
    {
        isMainStarted = true;
    }
}
