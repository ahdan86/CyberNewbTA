using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Computer : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    [SerializeField] private string _prompt;
    // init canvas UI
    [SerializeField] private GameObject _desktopUI;
    public string InteractionPrompt => _prompt;

    [Header("Virus Properties Computer")]
    [SerializeField] private bool virusInfected = false;
    [SerializeField] private ProgressBar infectVirusProgressBar;

    [Header("Dialogue Trigger")]
    public DialogueTrigger dialogueTrigger;

    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory == null) return false;
        if (inventory.HasFDContamined || inventory.HasFDClean)
        {
            Debug.Log("The Player have the FD in the inventory");
            Debug.Log("Interacting with computer");
            SetupDesktopUI(true, inventory);
            return true;
        }
        else
        {
            Debug.Log("You need to find the floppy disks!");
            return false;
        }
    }

    private void Update()
    {
        if (_desktopUI.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetupDesktopUI(false);
            }
        }
    }

    public void SetupDesktopUI(bool status, Inventory inventory = null)
    {
        Transform contentTransform = _desktopUI.transform.Find("Content");
        Transform iconsTransform = contentTransform.Find("Icons");
        Transform virusIconsTransform = iconsTransform.Find("VirusProgramDesktopIcon");

        if (status == true)
        {
            _desktopUI.SetActive(true);
            if (inventory.HasFDContamined)
            {
                virusIconsTransform.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            _desktopUI.SetActive(false);
            virusIconsTransform.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void InfectComputer()
    {
        virusInfected = true;
        infectVirusProgressBar.SetProgressActive(true);
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
