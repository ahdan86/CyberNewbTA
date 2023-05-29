using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public string InteractionName { get; }
    public bool Interact(Interactor interactor);
    public void SetUpPromptUI();
    public void CloseUI();
}
