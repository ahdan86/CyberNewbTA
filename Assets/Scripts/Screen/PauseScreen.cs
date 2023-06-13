using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour, Screen
{
    public static bool isPaused = false;
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
