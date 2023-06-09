    using float_oat.Desktop90;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Mono.Cecil;

public class DesktopScript : MonoBehaviour
{
    [Header("Desktop Transform UI")]
    [SerializeField] private Transform contentTransform;
    [SerializeField] private Transform iconsTransform;
    [SerializeField] private Transform windowsTransform;

    [SerializeField] private Transform virusIconsTransform;
    [SerializeField] private Transform virusWindowTransform;

    [SerializeField] private Transform antiVirusIconsTransform;
    [SerializeField] private Transform antiVirusWindowTransform;

    [SerializeField] private Transform documentIconsTransform;
    [SerializeField] private Transform documentWindowTransform;

    public bool isActive;
    public bool isInfected;
    public bool fileRestored;
    
    [Header("Desktop Properties & UI")]
    [SerializeField] private ProgressBar infectVirusProgressBar;
    [SerializeField] private ProgressBar cleanVirusProgressBar;
    [SerializeField] private int computerId;

    [Header("AntiVirus Properties")]
    [SerializeField] private Calculator calculator;
    [SerializeField] private int targetNumber;
    [SerializeField] private Text targetText;

    [Header("Player Properties")]
    [SerializeField] private Inventory inventory;

    [SerializeField] private WindowController[] windowObjects;


    private void Start()
    {
        DesktopEvent.current.onOpenDesktopUI.AddListener(OnOpenDesktopUI);
        DesktopEvent.current.onInfectComputer.AddListener(OnInfectedAntivirus);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (isActive)
        {
            windowObjects = FindObjectsOfType(typeof(WindowController)) as WindowController[];
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetupDesktopUI(false);
            }

            if (isInfected)
            {
                contentTransform.GetComponent<Image>().color =
                    Color.Lerp(Color.blue, Color.red, Mathf.PingPong(Time.time * 1.5f, 1));
            }
            else
            {
                antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text =
                    "The Antivirus didn't find any virus on the computer.";
            }
        }
    }

    private void SetupDesktopUI(bool active)
    {
        if (active)
        {
            gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            if (inventory.HasFDContamined)
            {
                if(fileRestored)
                    documentIconsTransform.gameObject.SetActive(true);
                else 
                    virusIconsTransform.gameObject.SetActive(true);
            }
            if (inventory.HasFDAntivirus)
            {
                antiVirusIconsTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(CloseAllWindows());
            Cursor.lockState = CursorLockMode.Locked;
        }
        isActive = active;
    }
    
    IEnumerator CloseAllWindows()
    {
        foreach(var window in windowObjects)
        {
            Debug.Log("Closing window");
            window.Close();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        virusIconsTransform.gameObject.SetActive(false);
        antiVirusIconsTransform.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    
    public void OnInfectedAntivirus(int id)
    {
        if(id == computerId)
        {
            SetAntiVirusWindow(true);
        }
    }

    public void SolveAntiVirusPuzzle()
    {
        if (isInfected)
        {
            StartCoroutine(CleanVirusProgressCoroutine());           
        }
        else
        {
            Debug.Log("Computer already clean");
        }
    }

    IEnumerator CleanVirusProgressCoroutine()
    {
        antiVirusWindowTransform.Find("Content").Find("Puzzle").Find("Submit Button")
            .GetComponent<Button>().interactable = false;
        
        cleanVirusProgressBar.SetProgressActive(true);
        while (!cleanVirusProgressBar.IsProgressCompleted())
        {
            yield return null;
        }

        if (calculator.Total == targetNumber)
        {
            isInfected = false;
            DesktopEvent.current.CleanVirus(computerId);
            contentTransform.GetComponent<Image>().color = new Color32(0x91, 0xAB, 0x7E, 0xFF);
            
            SetAntiVirusWindow(false);
            yield return new WaitForSeconds(1f);
            antiVirusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        }
        else
        {
            targetText.text = "WRONG ANSWER";
            yield return new WaitForSeconds(1f);
            SetAntiVirusWindow(true);
        }
        
        cleanVirusProgressBar.SetProgressValue(0f);
    }
    
    private void SetAntiVirusWindow(bool infectedStatus)
    {
        calculator.Total = 0;
        foreach(BitToggle toggle in calculator.bitToggles)
        {
            toggle.SetToggle(false);
        }

        if (infectedStatus)
        {
            antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text =
                "Your PC has been infected with a virus. Solve the puzzle to clean it up.";
            targetNumber = Random.Range(0, 31);
            targetText.text = targetNumber.ToString();
        }
        else
        {
            antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text =
                "The Antivirus didn't find any virus on the computer.";
            targetText.text = "Not Infected";
        }
    }

    public void InfectComputer()
    {
        if (!isInfected)
        {
            StartCoroutine(VirusProgressCoroutine());
        }
        else
        {
            Debug.Log("Computer already infected");
            virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        }
    }

    private void OnOpenDesktopUI(int id, bool infected, bool fileRestored)
    {
        Debug.Log("Desktop Opened");
        isActive = true;
        computerId = id;
        isInfected = infected;

        SetupDesktopUI(true);

        if (isInfected)
        {
            SetVirusWindow(true);
            SetAntiVirusWindow(true);
        } 
        else
        {
            SetVirusWindow(false); 
            SetAntiVirusWindow(false);
            contentTransform.GetComponent<Image>().color = new Color32(0x91, 0xAB, 0x7E, 0xFF);
        }
    }

    IEnumerator VirusProgressCoroutine()
    {
        virusWindowTransform.Find("Content").Find("OK Button").GetComponent<Button>().interactable = false;
        infectVirusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => infectVirusProgressBar.IsProgressCompleted());
        
        isInfected = true;
        DesktopEvent.current.InfectComputer(computerId);
        SetVirusWindow(true);
        
        yield return new WaitForSeconds(1f);
        virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        
        infectVirusProgressBar.SetProgressValue(0f);
    }

    private void SetVirusWindow(bool infectedStatus)
    {
        if (infectedStatus)
        {
            virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text = "Computer infected!!!!!!!!!!!!!!!!";
            virusWindowTransform.Find("Content").Find("OK Button").GetComponent<Button>().interactable = false;
        }
        else
        {
            virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text = "You must install this software to open DocumentToWorkForGus.docx";
            virusWindowTransform.Find("Content").Find("OK Button").GetComponent<Button>().interactable = true;
        }
    }

    private void OnDestroy()
    {
        DesktopEvent.current.onOpenDesktopUI.RemoveListener(OnOpenDesktopUI);
        DesktopEvent.current.onInfectComputer.RemoveListener(OnInfectedAntivirus);
    }
}
