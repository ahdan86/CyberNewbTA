using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    [FormerlySerializedAs("gameoverScreen")] public GameOverScreen gameOverScreen;
    public PauseScreen pauseScreen;
    
    public void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    
    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        gameOverScreen.Setup();
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
        pauseScreen.Setup();
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResumeButton()
    {
        Time.timeScale = 1;
        PauseScreen.isPaused = false;
        pauseScreen.Setup();
    }

    public void ExitButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
