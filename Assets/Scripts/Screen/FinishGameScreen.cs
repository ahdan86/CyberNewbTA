using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FinishGameScreen : MonoBehaviour, GameScreen
{
    [SerializeField] private Text moneyText;
    [SerializeField] private Text highscoreText;
    public void SetupScreen()
    {
        gameObject.SetActive(true);
    }

    public void SetMoney(float money, string levelName)
    {
        moneyText.text = "Money Left: $" + money.ToString("F1");

        string jsonString = PlayerPrefs.GetString("highscoreTable_" + levelName);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores != null && highscores.highscoreEntryList.Count > 0)
        {
            float highestMoney = highscores.highscoreEntryList.Max(t => t.score);
            if (money > highestMoney)
            {
                highscoreText.text = "NEW HIGHSCORE!: $" + money.ToString("F1");
            }
            else
            {
                highscoreText.text = "Highscore: $" + highestMoney.ToString("F1");
            }
        }
        else
        {
            highscoreText.text = "NEW HIGHSCORE!: $" + money.ToString("F1");
        }
        HighscoreTable.AddHighscoreEntry(money, StaticData.playerName, levelName);
    }
}
