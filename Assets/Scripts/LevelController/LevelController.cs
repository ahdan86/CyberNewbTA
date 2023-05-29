using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; set; }
    public List<Computer> computers;
    // public List<ProgressBar> computerProgressBars;
    [SerializeField] private int money;
    [SerializeField] private int moneyReducer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        
        // Infect Other Computer Functionality
       foreach(var computer in computers)
        {
            // var computerProgressBar = computerProgressBars[computer.getComputerId()];
            if (computer.getInfected())
            {
                //computerProgressBar.gameObject.SetActive(true);
                //StartCoroutine(InfectProgressCoroutine(computerProgressBar));
            }
            else
            {
                //computerProgressBar.gameObject.SetActive(false);
            }
        }

        // Checking if there is infected computer
        if (money <= 0 && computers.Any(computer => computer.getInfected()))
        {
            GameManager.Instance.GameOver();
        }
    }

    IEnumerator InfectProgressCoroutine(ProgressBar computerProgressBar)
    {
        computerProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => computerProgressBar.isProgressCompleted() == true);

        List<Computer> availableComputers = computers.Where(comp => !comp.getInfected()).ToList();
        if (availableComputers.Count > 0)
        {
            // Select a random computer from the available list
            int randomIndex = Random.Range(0, availableComputers.Count);
            var selected = availableComputers[randomIndex];

            // Infect the selected computer
            selected.setInfectComputer();
        }
    }
}
