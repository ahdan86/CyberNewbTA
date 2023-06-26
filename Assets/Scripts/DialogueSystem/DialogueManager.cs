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
    private int _activeMessage;
    private Dialogue _dialogue;
    public static bool isActive;

    [SerializeField] private Text continueText;
    [SerializeField] private ThirdPersonController characterController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Button[] buttonArray;
    [SerializeField] private Inventory inventory;

    public void OpenDialogue(Dialogue dialogueObject)
    {
        _dialogue = dialogueObject;
        _currentMessages = _dialogue.messages;
        _currentActors = _dialogue.actors;

        _activeMessage = 0;
        isActive = true;

        // turn off movement, animator and 'E' display
        characterController.setCanMove(false);
        playerAnimator.enabled = false;

        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void DisplayMessage()
    {
        Message message = _currentMessages[_activeMessage];
        Actor actor = GetActor(message.actor.actorId);

        actorImage.sprite = actor.actorSprite;
        actorName.text = actor.actorName;
        messageText.text = message.message;

        AnimateTextColor();
        for (int i = 0; i < buttonArray.Length; i++)
        {
            buttonArray[i].gameObject.SetActive(false);
        }

        if (message.inventoryGiveType == InventoryGiveType.FDVirus)
        {
            inventory.SetHasContamined(true);
        }
        else if (message.inventoryGiveType == InventoryGiveType.FDAntivirus)
        {
            inventory.SetHasAntivirus(true);
        }

        if (message.choices == null)
        {
            message.choices = new string [0];
        }
        for (int i = 0; i < message.choices.Length; i++)
        {
            int choiceIndex = i;
            buttonArray[i].gameObject.SetActive(true);
            buttonArray[i].GetComponentInChildren<Text>().text = message.choices[i];
            buttonArray[i].onClick.AddListener(() => EvaluateChoice(choiceIndex));
        }
    }

    private void ChangeDialogue(Dialogue dialogue)
    {
        _dialogue = dialogue;
        _currentMessages = _dialogue.messages;
        _currentActors = _dialogue.actors;
        _activeMessage = 0;
        DisplayMessage();
    }
    
    private void EvaluateChoice(int choiceIndex)
    {
        var message = _currentMessages[_activeMessage];
        bool isCorrect = (choiceIndex == message.correctChoiceIndex);

        if (message.choices != null)
        {
            for (int i = 0; i < message.choices.Length; i++)
            {
                buttonArray[i].onClick.RemoveAllListeners();
            }
        }

        if(!isCorrect)
        {
            if (_dialogue.isContainQuiz)
            {
                LevelController.Instance.ReduceMoney(50f);
                NotificationUI.Instance.AnimatePanel("You lost 50$!");   
            }
            ChangeDialogue(message.ifIncorrectDialogue);
        }
        else
        {
            NextMessage(); 
        }
    }
    
    private void NextMessage()
    {
        _activeMessage++;
        if (_activeMessage < _currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            isActive = false;
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();

            if (_currentActors.Length > 1)
            {
                QuestEvent.current.Interact(_currentActors[1].actorName);
                if (_dialogue.isContainQuiz)
                {
                    QuestEvent.current.Solve((int)ObjectiveType.SOLVE_QUIZ);
                    LevelController.Instance.SetCompletedQuiz(_dialogue);
                }
            }

            // turn on movement, animator and 'E' display
            characterController.setCanMove(true);
            playerAnimator.enabled = true;
            
            Debug.Log("Ended Conversation with " + _currentActors[0].actorName);
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
            if(_currentMessages[_activeMessage].choices == null)
            {
                _currentMessages[_activeMessage].choices = new string[0];
            }
            if (_currentMessages[_activeMessage].choices.Length < 1)
            {
                NextMessage();
            }
        }
    }

    private void Start()
    {
        backgroundBox.localScale = Vector3.zero;
    }
}
