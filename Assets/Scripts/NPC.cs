using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NPC : MonoBehaviour, IInteractable
{
    [FormerlySerializedAs("_interactionPromptUI")] [SerializeField] private WorldSpaceObjectUI worldSpaceObjectUI;
    [SerializeField] private string _name;
    public string InteractionName => _name;

    public DialogueTrigger dialogueTrigger;
    
    public bool Interact(Interactor interactor)
    {
        Debug.Log("Interacting with NPC");
        SetupDialogue();
        return true;
    }

    private void SetupDialogue()
    {
        dialogueTrigger.StartDialogue();
    }

    private void Update()
    {
        
    }

    public void SetUpPromptUI()
    {
        worldSpaceObjectUI.SetUp();
    }

    public void CloseUI()
    {
        worldSpaceObjectUI.Close();
    }
}
