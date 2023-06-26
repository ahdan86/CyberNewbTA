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
        var levelName = SceneManager.GetActiveScene().name;
        Time.timeScale = 0;
        finishGameScreen.SetupScreen();
        finishGameScreen.SetMoney(money, levelName);
        
        var nextLevelLoad= SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelLoad > PlayerPrefs.GetInt("level"))
        {
            PlayerPrefs.SetInt("level", nextLevelLoad);
        }
    }
    
    public void NextLevel()
    {
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        
        var nextLevelLoad= SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevelLoad);
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
