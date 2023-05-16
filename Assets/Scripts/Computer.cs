using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using float_oat.Desktop90;
using UnityEngine.Rendering;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    // DesktopUI elements
    [Header("DesktopUI elements")]
    [SerializeField] private GameObject _desktopUI;
    private Transform contentTransform;
    private Transform iconsTransform;
    private Transform windowsTransform;
    
    private Transform virusIconsTransform;
    private Transform virusWindowTransform;
    
    private Transform antiVirusIconsTransform;
    private Transform antiVirusWindowTransform;

    [Header("Virus Properties Computer")]
    [SerializeField] private bool virusInfected = false;
    [SerializeField] private bool isAntiVirus = false;
    [SerializeField] private ProgressBar infectVirusProgressBar;

    [Header("AntiVirus Properties")]
    [SerializeField] private Calculator calculator;

    [Header("Dialogue Trigger")]
    public DialogueTrigger dialogueFDWarning;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory == null) return false;
        if (inventory.HasFDContamined || inventory.HasFDAntivirus)
        {
            Debug.Log("The Player have the FD in the inventory");
            Debug.Log("Interacting with computer");
            SetupDesktopUI(true, inventory);
            return true;
        }
        else
        {
            dialogueFDWarning.StartDialogue();
            Debug.Log("You need to find the floppy disks!");
            return false;
        }
    }

    private void Awake()
    {
        contentTransform = _desktopUI.transform.Find("Content");
        windowsTransform = contentTransform.Find("Windows");
        iconsTransform = contentTransform.Find("Icons");
        
        virusWindowTransform = windowsTransform.Find("VirusProgramWindow");
        virusIconsTransform = iconsTransform.Find("VirusProgramDesktopIcon");
        
        antiVirusWindowTransform = windowsTransform.Find("AntiVirusWindow");
        antiVirusIconsTransform = iconsTransform.Find("AntiVirusDesktopIcon");
    }

    private void Update()
    {
        if (_desktopUI.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetupDesktopUI(false);
            }
            
            if (virusInfected)
            {
                contentTransform.GetComponent<Image>().color = Color.Lerp(Color.blue, Color.red, Mathf.PingPong(Time.time * 1.5f, 1));
            }
        }
    }

    public void SetupDesktopUI(bool status, Inventory inventory = null)
    {
        if (status == true)
        {
            _desktopUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            if (inventory.HasFDContamined)
            {
                virusIconsTransform.gameObject.SetActive(true);
            } 
            else if (inventory.HasFDAntivirus)
            {
                antiVirusIconsTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            _desktopUI.SetActive(false);
            virusIconsTransform.gameObject.SetActive(false);
            antiVirusIconsTransform.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void InfectComputer()
    {
        if (!virusInfected)
        {
            StartCoroutine(VirusProgressCoroutine());
        }
        else
        {
            Debug.Log("Computer already infected");
            virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
        }
    }

    public void solveAntiVirusPuzzle()
    {
        if (virusInfected)
        {
            Debug.Log("Solving AntiVirus Puzzle");
        } else
        {
            Debug.Log("Computer already clean");
        }
    }

    IEnumerator VirusProgressCoroutine()
    {
        infectVirusProgressBar.SetProgressActive(true);
        yield return new WaitUntil(() => infectVirusProgressBar.isProgressCompleted() == true);
        virusInfected = true;
        virusWindowTransform.gameObject.GetComponent<WindowController>().Close();
    }

    public void SetUpPromptUI()
    {
        _interactionPromptUI.SetUp();
    }

    public void CloseUI()
    {
        _interactionPromptUI.Close();
    }
}
