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

    [Header("Computer Properties")]
    [SerializeField] private bool virusInfected = false;
    [SerializeField] private int computerId;

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
            
            DesktopEvent.current.OpenDesktopUI(computerId, virusInfected);
           
            return true;
        }
        else
        {
            dialogueFDWarning.StartDialogue();
            Debug.Log("You need to find the floppy disks!");
            return false;
        }
    }

    private void Start()
    {
        DesktopEvent.current.onInfectComputer.AddListener(InfectComputer);
        DesktopEvent.current.onCleanVirus.AddListener(CleanInfected);

        Debug.Log(computerId);
    }

    public void InfectComputer(int id)
    {
        if(id == computerId)
            virusInfected = true;
    }

    public void CleanInfected(int id)
    {
        if (id == computerId)
            virusInfected = false;
    }

    public void SetUpPromptUI()
    {
        _interactionPromptUI.SetUp();
    }

    public void CloseUI()
    {
        _interactionPromptUI.Close();
    }

    public void OnDestroy()
    {
        DesktopEvent.current.onInfectComputer.RemoveListener(InfectComputer);
        DesktopEvent.current.onCleanVirus.RemoveListener(CleanInfected);
    }
}
