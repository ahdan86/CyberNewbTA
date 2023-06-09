using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;

    private Message[] _currentMessages;
    private Actor[] _currentActors;
    private int _activeMessage = 0;
    public static bool isActive = false;

    [SerializeField] private ThirdPersonController characterController;
    [SerializeField] private Animator playerAnimator;

    public void OpenDialogue(Dialogue dialogueObject)
    {
        _currentMessages = dialogueObject.messages;
        _currentActors = dialogueObject.actors;
        _activeMessage = 0;
        isActive = true;

        Debug.Log("Started Conversation with " + _currentActors[0].name);
        
        // turn off movement, animator and 'E' display
        characterController.setCanMove(false);
        playerAnimator.enabled = false;

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    public void DisplayMessage()
    {
        Message message = _currentMessages[_activeMessage];
        Actor actor = GetActor(message.actor.actorId);

        actorImage.sprite = actor.sprite;
        actorName.text = actor.name;
        messageText.text = message.message;
        
        for (int i = 0; i < message.choices.Length; i++)
        {
            // TODO: Display choices
        }

        AnimateTextColor();
    }
    
    public void EvaluateChoice(bool isCorrect)
    {
        if (isCorrect)
        {
            Debug.Log("Correct choice!");
        }
        else
        {
            Debug.Log("Wrong choice!");
        }

        NextMessage();
    }
    
    public void NextMessage()
    {
        _activeMessage++;
        if (_activeMessage < _currentMessages.Length)
        {
            // Hide choices
            //for (int i = 0; i < buttonArray.Length; i++)
            //{
            //     buttonArray[i].gameObject.SetActive(false);
            //}
            DisplayMessage();
        }
        else
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();

            if (_currentActors.Length > 1)
            {
                QuestEvent.current.Interact(_currentActors[1].name);
            }

            // turn on movement, animator and 'E' display
            characterController.setCanMove(true);
            playerAnimator.enabled = true;
            
            Debug.Log("Ended Conversation with " + _currentActors[0].name);
        }
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    Actor GetActor(int actorId)
    {
        foreach (Actor actor in _currentActors)
        {
            if (actor.actorId == actorId)
            {
                return actor;
            }
        }

        return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive)
        {
            NextMessage();
        }
    }

    private void Start()
    {
        backgroundBox.localScale = Vector3.zero;
    }
}
