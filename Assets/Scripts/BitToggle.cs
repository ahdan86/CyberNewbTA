using float_oat.Desktop90;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BitToggle : MonoBehaviour
{
    [SerializeField] private int number;
    public event Action<int, bool> OnToggleChanged = delegate { };
    private bool isEnabled = false;

    private void HandleToggleChanged(bool enabled)
    {
        OnToggleChanged(number, enabled);
        if(isEnabled)
            GetComponentInChildren<Text>().text = "1";
        else
            GetComponentInChildren<Text>().text = "0";
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            isEnabled = !isEnabled;
            HandleToggleChanged(isEnabled);
        });
    }
}
