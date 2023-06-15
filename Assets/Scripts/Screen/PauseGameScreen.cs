using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameScreen : MonoBehaviour, GameScreen
{
    public static bool isPaused;
    public void SetupScreen()
    {
        if (isPaused)
        {
            gameObject.SetActive(true);
            isPaused = true;
        }

        else
        {
            gameObject.SetActive(false);
            isPaused = false;
        }
    }
}
