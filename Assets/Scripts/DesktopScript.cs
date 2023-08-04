using float_oat.Desktop90;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DesktopScript : MonoBehaviour
{
    [Header("Desktop Transform UI")]
    [SerializeField] private Transform contentTransform;

    [SerializeField] private Transform virusIconsTransform;
    [SerializeField] private Transform virusWindowTransform;

    [SerializeField] private Transform antiVirusIconsTransform;
    [SerializeField] private Transform antiVirusWindowTransform;

    [SerializeField] private Transform documentIconsTransform;
    [SerializeField] private Transform documentWindowTransform;

    public bool isActive;
    public bool isInfected;
    private bool _isPopupActive;

    [Header("Desktop Properties & UI")]
    [SerializeField] private ProgressBar infectVirusProgressBar;
    [SerializeField] private ProgressBar cleanVirusProgressBar;
    [SerializeField] private int computerId;

    [FormerlySerializedAs("calculator")]
    [Header("AntiVirus Properties")]
    [SerializeField] private BinaryCalculator binaryCalculator;
    [SerializeField] private int targetNumber;
    [SerializeField] private Text targetText;
    [SerializeField] private int minNumber;
    [SerializeField] private int maxNumber;
    private Coroutine _antiVirusCoroutine;
    private Coroutine _virusCoroutine;
    private Coroutine _popUpCoroutine;

    [Header("Player Properties")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private ThirdPersonController characterController;
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private WindowController[] windowObjects;
    [SerializeField] private WindowController popUpWindow;
    
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
                SetPlayerMovement(true);
            }

            if (isInfected)
            {
                contentTransform.GetComponent<Image>().color =
                    Color.Lerp(Color.blue, Color.red, Mathf.PingPong(Time.time * 1.5f, 1));
                if (!_isPopupActive)
                {
                    _popUpCoroutine = StartCoroutine(ShowPopUpCoroutine());
                    _isPopupActive = true;
                }
            }
            else
            {
                if (_isPopupActive)
                {
                    if(popUpWindow.isActiveAndEnabled)
                        popUpWindow.Close();
                    if(_popUpCoroutine != null)
                        StopCoroutine(_popUpCoroutine);
                    _isPopupActive = false;
                }
            }
        }
    }

    IEnumerator ShowPopUpCoroutine()
    {
        popUpWindow.Open();
        yield return new WaitForSeconds(1.5f);
        popUpWindow.Close();
        yield return new WaitForSeconds(0.5f);
        _isPopupActive = false;
    }

    private void SetPlayerMovement(bool status)
    {
        characterController.setCanMove(status);
        playerAnimator.enabled = status;
    }

    private void SetupDesktopUI(bool active)
    {
        if (active)
        {
            gameObject.SetActive(true);
            SetPlayerMovement(false);
            if (inventory.hasFDContamined)
            {
                virusIconsTransform.gameObject.SetActive(true);
            }
            if (inventory.hasFDAntivirus)
            {
                antiVirusIconsTransform.gameObject.SetActive(true);
            }
            if(inventory.hasFDContaminedCleaned)
            {
                documentIconsTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(CloseAllWindows());
        }
        isActive = active;
    }
    
    IEnumerator CloseAllWindows()
    {
        if (_virusCoroutine != null)
        {
            StopCoroutine(_virusCoroutine);
            infectVirusProgressBar.SetProgressValue(0f);
            infectVirusProgressBar.SetProgressActive(false);
        }

        if (_antiVirusCoroutine != null)
        {
            StopCoroutine(_antiVirusCoroutine);
            cleanVirusProgressBar.SetProgressValue(0f);
            cleanVirusProgressBar.SetProgressActive(false);
        }

        foreach(var window in windowObjects)
        {
            Debug.Log("Closing window");
            window.Close();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Turn off all core program
        virusIconsTransform.gameObject.SetActive(false);
        antiVirusIconsTransform.gameObject.SetActive(false);
        documentWindowTransform.gameObject.SetActive(false);
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
        if (isInfected || inventory.hasFDContamined)
        {
            _antiVirusCoroutine = StartCoroutine(CleanVirusProgressCoroutine());           
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
        foreach(BitToggle toggle in binaryCalculator.bitToggles)
        {
            toggle.GetComponent<Button>().interactable = false;
        }
        
        while (!cleanVirusProgressBar.IsProgressCompleted())
        {
            yield return null;
        }

        if (binaryCalculator.total == targetNumber)
        {
            if (isInfected)
            {
                isInfected = false;
                DesktopEvent.current.CleanVirus(computerId);
                contentTransform.GetComponent<Image>().color = new Color32(0x91, 0xAB, 0x7E, 0xFF);
            }
                
            else if (inventory.hasFDContamined)
            {
                inventory.hasFDContamined = false;
                inventory.hasFDContaminedCleaned = true;
                NotificationUI.Instance.AnimatePanel("FlashDrive Cleaned");
                
                virusIconsTransform.gameObject.SetActive(false);
                documentIconsTransform.gameObject.SetActive(true);
            }

            SetAntiVirusWindow(false);
            yield return new WaitForSeconds(0.5f);
            //antiVirusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        }
        else
        {
            targetText.text = "WRONG ANSWER";
            yield return new WaitForSeconds(1f);
            if(isInfected)
                SetAntiVirusWindow(true);
            else
                SetAntiVirusWindow(false);
        }
        
        cleanVirusProgressBar.SetProgressValue(0f);
    }
    
    private void SetAntiVirusWindow(bool infectedStatus)
    {
        antiVirusWindowTransform.Find("Content").Find("Puzzle").Find("Submit Button")
            .GetComponent<Button>().interactable = true;
        binaryCalculator.total = 0;
        
        foreach(BitToggle toggle in binaryCalculator.bitToggles)
        {
            toggle.GetComponent<Button>().interactable = true;
        }
        
        foreach(BitToggle toggle in binaryCalculator.bitToggles)
        {
            toggle.SetToggle(false);
        }

        if (infectedStatus)
        {
            antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text =
                "Your PC has been infected with a virus. Solve the puzzle to clean it up.";
            targetNumber = Random.Range(minNumber, maxNumber);
            targetText.text = targetNumber.ToString();
        }
        else if (inventory.hasFDContamined)
        {
            antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text =
                "You have a flash drive that is infected with a virus. Clean the virus to restore the files.";
            targetNumber = Random.Range(minNumber, maxNumber);
            targetText.text = targetNumber.ToString();
        }
        else
        {
            antiVirusWindowTransform.Find("Content").Find("DescText").GetComponent<Text>().text =
                "The antivirus didn't find any virus on the computer.";
            targetText.text = "Not Infected";
            antiVirusWindowTransform.Find("Content").Find("Puzzle").Find("Submit Button")
                .GetComponent<Button>().interactable = false;
        }
    }

    public void InfectComputer()
    {
        if (!isInfected)
        {
            _virusCoroutine = StartCoroutine(VirusProgressCoroutine());
        }
        else
        {
            Debug.Log("Computer already infected");
            virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
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

    private void OnOpenDesktopUI(int id, bool infected)
    {
        Debug.Log("Desktop Opened");
        isActive = true;
        computerId = id;
        isInfected = infected;
        _isPopupActive = false;

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

    public void AcceptDocument()
    {
        DesktopEvent.current.AcceptDocument();
    }

    private void OnDestroy()
    {
        DesktopEvent.current.onOpenDesktopUI.RemoveListener(OnOpenDesktopUI);
        DesktopEvent.current.onInfectComputer.RemoveListener(OnInfectedAntivirus);
    }
}
