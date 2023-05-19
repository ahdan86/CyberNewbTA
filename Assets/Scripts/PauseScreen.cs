using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static bool isPaused = false;
    public void Setup()
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
