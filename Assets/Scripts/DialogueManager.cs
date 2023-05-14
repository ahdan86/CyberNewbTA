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

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    [SerializeField] private ThirdPersonController _characterController;
    [SerializeField] private Animator _playerAnimator;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Started Conversation with " + currentActors[0].name);
        
        // turn off movement, animator and 'E' display
        _characterController.setCanMove(false);
        _playerAnimator.enabled = false;

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    public void DisplayMessage()
    {
        Message message = currentMessages[activeMessage];
        Actor actor = GetActor(message.actorId);

        actorImage.sprite = actor.sprite;
        actorName.text = actor.name;
        messageText.text = message.message;

        AnimateTextColor();
    }
    
    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();

            // turn on movement, animator and 'E' display
            _characterController.setCanMove(true);
            _playerAnimator.enabled = true;
            
            Debug.Log("Ended Conversation with " + currentActors[0].name);
        }
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    Actor GetActor(int actorId)
    {
        foreach (Actor actor in currentActors)
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
