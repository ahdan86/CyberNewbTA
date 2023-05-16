using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    [SerializeField] private Text totalText;
    public int Total = 0;
    public BitToggle[] bitToggles;
    private void Start()
    {
        foreach (var toggle in bitToggles)
        {
            toggle.OnToggleChanged += Toggle_OnToggleChanged;
        }
    }

    private void Toggle_OnToggleChanged(int number, bool enabled)
    {
        if (enabled)
        {
            Total += number;
        } 
        else
        {
            Total -= number;
        }

        totalText.text = Total.ToString();
    }
}
