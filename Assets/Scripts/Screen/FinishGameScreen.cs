using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGameScreen : MonoBehaviour, GameScreen
{
    [SerializeField] private Text moneyText;
    public void SetupScreen()
    {
        gameObject.SetActive(true);
    }

    public void SetMoneyText(float money)
    {
        moneyText.text = "Money Left: $ " + money.ToString("F1");
    }
}
