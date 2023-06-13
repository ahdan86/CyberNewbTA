using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public GameOverScreen gameOverScreen;
    public PauseScreen pauseScreen;
    public FinishScreen finishScreen;
    
    public void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        gameOverScreen.SetupScreen();
    }

    public void Success(float money)
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        // TODO: Add Winning Screen UI (Both gameObject and Script)
    }

    public void Paused()
    {
        PauseScreen.isPaused = true;
        Time.timeScale = 0;
        pauseScreen.SetupScreen();
    }
    
    public void ResumeButton()
    {
        PauseScreen.isPaused = false;
        Time.timeScale = 1;
        pauseScreen.SetupScreen();
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
