using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using float_oat.Desktop90;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Computer : MonoBehaviour, IInteractable
{
    [FormerlySerializedAs("_interactionPromptUI")] [SerializeField] private WorldSpaceObjectUI worldSpaceObjectUI;
    [SerializeField] private string _prompt;
    public string InteractionName => _prompt;

    [Header("Computer Properties")]
    [SerializeField] private bool virusInfected;

    [SerializeField] private bool fileRestored;
    [SerializeField] private int computerId;

    [Header("Dialogue Trigger")]
    public DialogueTrigger dialogueFDWarning;

    private void Start()
    {
        DesktopEvent.current.onInfectComputer.AddListener(InfectComputerById);
        DesktopEvent.current.onCleanVirus.AddListener(CleanInfectedById);
    }
    
    public bool Interact(Interactor interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory == null) 
            return false;
        
        if (inventory.HasFDContamined || inventory.HasFDAntivirus)
        {
            Debug.Log("The Player have the FD in the inventory");
            Debug.Log("Interacting with computer");
            
            DesktopEvent.current.OpenDesktopUI(computerId, virusInfected, fileRestored);
           
            return true;
        }

        dialogueFDWarning.StartDialogue();
        Debug.Log("You need to find the floppy disks!");
        return false;
    }

    public void InfectComputerById(int id)
    {
        if(id == computerId)
            virusInfected = true;
    }

    public void CleanInfectedById(int id)
    {
        if (id == computerId)
        {
            virusInfected = false;
            fileRestored = true;
        }
    }

    public void SetUpPromptUI()
    {
        worldSpaceObjectUI.SetUp();
    }

    public void CloseUI()
    {
        worldSpaceObjectUI.Close();
    }

    public bool GetInfectedStatus()
    {
        return virusInfected;
    }

    public void SetInfectComputer()
    {
        virusInfected = true; 
    }

    public void OnDestroy()
    {
        DesktopEvent.current.onInfectComputer.RemoveListener(InfectComputerById);
        DesktopEvent.current.onCleanVirus.RemoveListener(CleanInfectedById);
    }
}
