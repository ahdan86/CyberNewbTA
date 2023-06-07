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

    public bool isActive = false;
    public bool isInfected = false;
    
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
        DesktopEvent.current.onInfectComputer.AddListener(SetAntiVirus);
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
                contentTransform.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, Mathf.PingPong(Time.time * 1.5f, 1));
            }
            else
            {
                antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text = "The Antivirus didn't find any virus on the computer.";
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
    
    public void SetAntiVirus(int id)
    {
        if(id == computerId)
        {
            targetNumber = Random.Range(0, 31);
            targetText.text = targetNumber.ToString();
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
        
        cleanVirusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => cleanVirusProgressBar.isProgressCompleted() == true);

        if (calculator.Total == targetNumber)
        {
            isInfected = false;
            DesktopEvent.current.CleanVirus(computerId);
            virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text
                = "You must install this software to open DocumentToWorkForGus.docx";
            contentTransform.GetComponent<Image>().color = new Color32(0x91, 0xAB, 0x7E, 0xFF);
        }
        else
        {
            targetText.text = "WRONG ANSWER";
            yield return new WaitForSeconds(1f);
            targetNumber = Random.Range(0, 31);
            targetText.text = targetNumber.ToString();
        }
        
        cleanVirusProgressBar.SetProgressValue(0f);
    }

    public void InfectComputer()
    {
        if (!isInfected)
        {
            StartCoroutine(VirusProgressCoroutine());
            DesktopEvent.current.InfectComputer(computerId);
        }
        else
        {
            Debug.Log("Computer already infected");
            virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        }
    }

    private void OnOpenDesktopUI(int id, bool infected)
    {
        Debug.Log("Desktop Opened");
        isActive = true;
        computerId = id;
        isInfected = infected;
        calculator.Total = 0;

        SetupDesktopUI(true);
        
        foreach(BitToggle toggle in calculator.bitToggles)
        {
            toggle.SetToggle(false);
        }

        if (isInfected)
        {
            targetNumber = Random.Range(0, 31);
            targetText.text = targetNumber.ToString();
        } else
        {
            virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text =
                "You must install this software to open DocumentToWorkForGus.docx";
            virusWindowTransform.Find("Content").Find("OK Button").GetComponent<Button>().enabled = true;
            contentTransform.GetComponent<Image>().color = new Color32(0x91, 0xAB, 0x7E, 0xFF);
            targetText.text = "Not Infected";
        }
    }

    IEnumerator VirusProgressCoroutine()
    {
        virusWindowTransform.Find("Content").Find("OK Button").GetComponent<Button>().enabled = false;
        infectVirusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => infectVirusProgressBar.isProgressCompleted() == true);
        
        isInfected = true;
        
        DesktopEvent.current.InfectComputer(computerId);

        virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text = "Computer infected!!!!!!!!!!!!!!!!";
        yield return new WaitForSeconds(1f);
        virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        infectVirusProgressBar.SetProgressValue(0f);
    }

    private void OnDestroy()
    {
        DesktopEvent.current.onOpenDesktopUI.RemoveListener(OnOpenDesktopUI);
        DesktopEvent.current.onInfectComputer.RemoveListener(SetAntiVirus);
    }
}
