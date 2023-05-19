using float_oat.Desktop90;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isActive = false;
                gameObject.SetActive(false);
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

    public void SetupDesktopUI(bool active)
    {
        isActive = active;
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
            virusIconsTransform.gameObject.SetActive(false);
            antiVirusIconsTransform.gameObject.SetActive(false);
            gameObject.SetActive(false);

            // DesktopEvent.current.onOpenDesktopUI.RemoveListener(OnOpenDesktopUI);

            Cursor.lockState = CursorLockMode.Locked;
        }
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
            if (calculator.Total == targetNumber)
            {
                DesktopEvent.current.CleanVirus(computerId);
                contentTransform.GetComponent<Image>().color = Color.white;
            }
            else
            {
                cleanVirusProgressBar.SetProgressValue(0f);   
                targetText.text = "WRONG ANSWER";
                targetNumber = Random.Range(0, 31);
                targetText.text = targetNumber.ToString();
            }
        }
        else
        {
            Debug.Log("Computer already clean");
        }
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

        if (isInfected)
        {
            //calculator.bitToggles.ForEach(toggle => toggle.SetToggle(false)) ;
            targetNumber = Random.Range(0, 31);
            targetText.text = targetNumber.ToString();
        } else
        {
            contentTransform.GetComponent<Image>().color = new Color(149f, 180f, 125f, 1.0f);
            targetText.text = "Not Infected";
        }
    }

    IEnumerator CleanVirusProgressCoroutine()
    {
        cleanVirusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => cleanVirusProgressBar.isProgressCompleted() == true);
        
        isInfected = false;
        contentTransform.GetComponent<Image>().color = new Color(149f, 180f, 125f);
        
        DesktopEvent.current.CleanVirus(computerId);
        
        virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text = "You must install this software to open DocumentToWorkForGus.docx";
        
        yield return new WaitForSeconds(1f);
        antiVirusWindowTransform.gameObject.GetComponent<WindowController>().Close();
    }

    IEnumerator VirusProgressCoroutine()
    {
        infectVirusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => infectVirusProgressBar.isProgressCompleted() == true);
        
        isInfected = true;
        
        DesktopEvent.current.InfectComputer(computerId);

        virusWindowTransform.Find("Content").Find("VirusDesc").GetComponent<Text>().text = "Computer infected!!!!!!!!!!!!!!!!";
        yield return new WaitForSeconds(1f);
        virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
    }

    private void OnDestroy()
    {
        DesktopEvent.current.onOpenDesktopUI.RemoveListener(OnOpenDesktopUI);
        DesktopEvent.current.onInfectComputer.RemoveListener(SetAntiVirus);
    }
}
