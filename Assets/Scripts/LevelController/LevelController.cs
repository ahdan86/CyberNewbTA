using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public List<Computer> computers;
    public List<ProgressBar> computerProgressBars;
    [SerializeField] private float money;
    [SerializeField] private float moneyReducer;
    [SerializeField] private bool isStarted = false;

    private void Update()
    {
        if (isStarted)
        {
            //Reduce money overtime
            money -= moneyReducer * Time.deltaTime;
            
            //Checking komputer lain
            foreach(var computer in computers)
            {
                var computerProgressBar = computerProgressBars[computer.getComputerId()];
                if (computer.getInfected())
                {
                    computerProgressBar.gameObject.SetActive(true);
                    StartCoroutine(InfectProgressCoroutine(computerProgressBar));
                }
                else
                {
                    computerProgressBar.gameObject.SetActive(false);
                }
            }
            
            if (money <= 0 && computers.Any(computer => computer.getInfected()))
            {
                isStarted = false;
                GameManager.Instance.GameOver();
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
    
    public void StartGame()
    {
        isStarted = true;
    }
}
