using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Computer : MonoBehaviour, IInteractable
{
    [FormerlySerializedAs("_interactionPromptUI")] [SerializeField] private WorldSpaceObjectUI worldSpaceObjectUI;
    [FormerlySerializedAs("_prompt")] [SerializeField] private string prompt;
    public string InteractionName => prompt;

    [Header("Computer Properties")]
    [SerializeField] private bool virusInfected;
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
        
        if (inventory.hasFDContamined || inventory.hasFDAntivirus)
        {
            DesktopEvent.current.OpenDesktopUI(computerId, virusInfected);
           
            return true;
        }

        dialogueFDWarning.StartDialogue();
        Debug.Log("You need to find the floppy disks!");
        return false;
    }

    public void InfectComputerById(int id)
    {
        if (id == computerId)
        {
            virusInfected = true;
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void CleanInfectedById(int id)
    {
        if (id == computerId)
        {
            virusInfected = false;
            gameObject.GetComponent<Outline>().enabled = false;
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
        gameObject.GetComponent<Outline>().enabled = true;
    }

    public void OnDestroy()
    {
        DesktopEvent.current.onInfectComputer.RemoveListener(InfectComputerById);
        DesktopEvent.current.onCleanVirus.RemoveListener(CleanInfectedById);
    }
}
