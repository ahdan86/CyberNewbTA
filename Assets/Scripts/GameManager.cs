using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [FormerlySerializedAs("gameOverScreen")] public GameOverGameScreen gameOverGameScreen;
    [FormerlySerializedAs("pauseScreen")] public PauseGameScreen pauseGameScreen;
    [FormerlySerializedAs("finishScreen")] public FinishGameScreen finishGameScreen;
    
    public void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverGameScreen.SetupScreen();
    }

    public void Success(float money)
    {
        Time.timeScale = 0;
        finishGameScreen.SetupScreen();
        finishGameScreen.SetMoneyText(money);
    }
    
    public void NextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene(0);
            return;
        }
        
        var nextLevelLoad= SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevelLoad);

        if (nextLevelLoad > PlayerPrefs.GetInt("level"))
        {
            PlayerPrefs.SetInt("level", nextLevelLoad);
        }
    }

    public void QuitLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void Paused()
    {
        PauseGameScreen.isPaused = true;
        Time.timeScale = 0;
        pauseGameScreen.SetupScreen();
    }
    
    public void ResumeButton()
    {
        PauseGameScreen.isPaused = false;
        Time.timeScale = 1;
        pauseGameScreen.SetupScreen();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
