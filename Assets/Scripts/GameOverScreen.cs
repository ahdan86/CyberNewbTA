using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Text moneyLeftText;
    public static bool isOver = false;
    public void Setup()
    {
        gameObject.SetActive(true);
        isOver = true;
    }
}
