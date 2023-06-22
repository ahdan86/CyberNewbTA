using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private Button[] levelButtons;
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private GameObject levelSelectorGroup;
    [SerializeField] private string playerName;
    void Start()
    {
        var level = PlayerPrefs.GetInt("level", 1);
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 > level)
            {
                levelButtons[i].interactable = false;
            }

            var index = i;
            levelButtons[i].onClick.AddListener(() => EnterLevel(index + 1));
        }
    }
    
    public void SetPlayerName()
    {
        if (playerNameInputField.text == "")
        {
            playerName = "Player";
            playerNameInputField.text = "Player";
        }
        else
        {
            playerName = playerNameInputField.text;
        }
        playerName = playerNameInputField.text;
        levelSelectorGroup.SetActive(true);
    }
    
    public void EnterLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
        StaticData.playerName = playerName;
    }
}
