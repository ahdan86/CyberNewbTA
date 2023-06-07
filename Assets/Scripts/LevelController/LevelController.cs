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
            moneyText.text = money.ToString("F1");
            
            //Checking komputer lain
            // linq check if any of the computer is infected
            if (computers.Any(comp => comp.getInfected()))
            {
                StartCoroutine(InfectProgressCoroutine());
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
        yield return new WaitUntil(() => virusProgressBar.isProgressCompleted());

        List<Computer> availableComputers = computers.Where(comp => !comp.getInfected()).ToList();
        if (availableComputers.Count > 0)
        {
            int randomIndex = Random.Range(0, availableComputers.Count);
            var selected = availableComputers[randomIndex];
            selected.setInfectComputer();
        }
    }
    
    public void StartGame()
    {
        isMainStarted = true;
    }
}
