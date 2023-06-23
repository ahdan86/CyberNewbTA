using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Calculator : MonoBehaviour
{
    [SerializeField] private Text totalText;
    [FormerlySerializedAs("Total")] public int total = 0;
    public BitToggle[] bitToggles;
    private void Start()
    {
        foreach (var toggle in bitToggles)
        {
            toggle.OnToggleChanged += Toggle_OnToggleChanged;
        }
    }

    private void Update()
    {
        totalText.text = total.ToString();
    }

    private void Toggle_OnToggleChanged(int number, bool enabled)
    {
        if (enabled)
        {
            total += number;
        } 
        else
        {
            total -= number;
        }
    }
}
