using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance { get; set; }
    public List<Computer> computers;
    public List<ProgressBar> computerProgressBars;
    public int moneyLeft;
    

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
       foreach(var computer in computers)
        {
            var computerProgressBar = computerProgressBars[computer.getComputerId()];
            if (computer.getInfected())
            {
                computerProgressBar.gameObject.SetActive(true);
                StartCoroutine(InfectProgressCoroutine(computerProgressBar));
                // computerProgressBar[computer.getComputerId()].SetProgress(computer.getProgress());
            }
            else
            {
                computerProgressBar.gameObject.SetActive(false);
            }
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
