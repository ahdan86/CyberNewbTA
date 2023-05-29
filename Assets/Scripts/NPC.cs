using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
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
        _interactionPromptUI.SetUp();
    }

    public void CloseUI()
    {
        _interactionPromptUI.Close();
    }
}
