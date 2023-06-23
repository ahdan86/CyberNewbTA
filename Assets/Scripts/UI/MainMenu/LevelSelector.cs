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
    [SerializeField] private TextMeshProUGUI[] highScoreTexts;
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
            
            //set highscore value to text
            string jsonString = PlayerPrefs.GetString("highscoreTable_Level " + (i+1));
            Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
            if(highscores != null && highscores.highscoreEntryList.Count > 0)
                highScoreTexts[i].text = "Highscore: $" + highscores.highscoreEntryList[0].score.ToString("F1");
            else
                highScoreTexts[i].text = "Highscore: $0.0";

            var index = i;
            levelButtons[i].onClick.AddListener(() => EnterLevel(index + 1));
        }
        levelSelectorGroup.SetActive(false);
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
